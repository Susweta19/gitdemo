using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;

namespace XmlExecutor.Factories
{
    public class AttributeUtils
    {

        private readonly Type _attributeType;
        private SignatureStorage _signatureStorage;

        public AttributeUtils(Type attributeType)
        {
            this._attributeType = attributeType;
        }

        /// <summary>
        /// Checks whether or not a type contains at least one method with the attribute.
        /// </summary>
        /// <param name="type">the type to check</param>
        /// <returns>true if the type contains at least one method with the attribute</returns>
        public bool IsActionType(Type type)
        {
            return type.GetMethods().Any(x => IsActionMethod(x));
        }

        /// <summary>
        /// Check whether or not the method is an action method
        /// </summary>
        /// <param name="info">the info object of the method to check</param>
        /// <returns>true if the method is an action method</returns>
        public bool IsActionMethod(MethodInfo info)
        {
			bool isActionMethod = info.GetCustomAttribute(_attributeType) != null;
			return isActionMethod;
        }

        /// <summary>
        /// Gets the signature storage which is initialized at the first call to this method.
        /// </summary>
        /// <returns>the signature storage</returns>
        public SignatureStorage GetSignatureStorage()
        {
            if (_signatureStorage == null)
            {
                var signaturesDic = new Dictionary<Type, IEnumerable<MethodSignature>>();
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                string path = System.AppDomain.CurrentDomain.BaseDirectory;

                //This code is useful only if one wishes to use specific code for projects
                //foreach (string dll in Directory.GetFiles(path, "*.dll", SearchOption.AllDirectories))
                //{
                //    try
                //    {
                //        Console.WriteLine($"Loading assembly {dll}");
                //        Assembly assembly = Assembly.LoadFile(dll);
                //        ParseAssembly(assembly, signaturesDic);
                //        Console.WriteLine($"Loaded assembly {dll}");
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine("Failed to load assembly: " + e.Message);
                //        Console.WriteLine("Failed to load assembly: " + e.StackTrace);
                //    }
                //}

                foreach (Assembly assembly in assemblies)
                {
                    try
                    {
                        ParseAssembly(assembly, signaturesDic);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Failed to load assembly: " + e.Message);
                        Console.WriteLine("Failed to load assembly: " + e.StackTrace);
                    }
                }

                _signatureStorage = new SignatureStorage(this, signaturesDic);
            }
            return _signatureStorage;
        }

        /// <summary>
        /// Parses an assembly and associates every action type found to a list of its action method signatures
        /// </summary>
        /// <param name="assembly">the assembly</param>
        /// <param name="signaturesDic">this dictionnary will be filled with the found action types and signatures</param>
        private void ParseAssembly(Assembly assembly, Dictionary<Type, IEnumerable<MethodSignature>> signaturesDic)
        {
            Type[] types = assembly.GetTypes();
            foreach (Type type in types)
            {
                Type realType = type;// OverloadUtils.OverLoadType(type);
                if (IsActionType(realType))
                {
                    ICollection<MethodSignature> signatures = GetSignatures(realType);
                    try
                    {
                        signaturesDic.Add(realType, signatures);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Failed to add signatures: " + e.StackTrace);
                    }
                }
            }
            AssemblyName[] assemblyNames = assembly.GetReferencedAssemblies();
            foreach (AssemblyName assemblyName in assemblyNames)
            {
                bool isLoaded = AppDomain.CurrentDomain.GetAssemblies().Any(a => a.GetName().FullName == assemblyName.FullName);
                if (!isLoaded)
                {
                    try
                    {
                        Assembly refAssembly = AppDomain.CurrentDomain.Load(assemblyName);
                        ParseAssembly(refAssembly, signaturesDic);
                    }
                    catch (Exception)
                    {
                        //TODO one of the referenced assemblies cannot be loaded find out why or log the problem
                    }
                }
            }
        }

        /// <summary>
        /// Gets the signatures for a specific type.
        /// </summary>
        /// <param name="type">the type</param>
        /// <returns>a collection of signatures</returns>
        public ICollection<MethodSignature> GetSignatures(Type type)
        {
            ICollection<MethodSignature> signatures = new List<MethodSignature>();
            IEnumerable<MethodInfo> methodInfos = ExtractFromType(type);
            foreach (MethodInfo methodInfo in methodInfos)
            {
                TxActionAttribute attribute = ((TxActionAttribute)methodInfo.GetCustomAttribute(typeof(TxActionAttribute)));
                signatures.Add(new MethodSignature(attribute.Name, attribute.Comment, methodInfo));
            }
            return signatures;
        }

        /// <summary>
        /// Extracts every method signatures (MethodInfo) with the attribute from a given type.
        /// </summary>
        /// <param name="type">the type</param>
        /// <returns>an enumerable over the methods</returns>
        private IEnumerable<MethodInfo> ExtractFromType(Type type)
        {
            return from x in type.GetMethods()
                   where x.GetCustomAttribute(_attributeType) != null
                   select x;
        }
    }
}
