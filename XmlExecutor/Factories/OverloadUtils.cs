using System;
using System.Collections.Generic;
using System.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;
using XmlExecutor.Interfaces;
using System.ComponentModel.Composition;

namespace XmlExecutor.Factories
{


    public static class OverloadUtils
    {
        private class Importer
        {
            [TxOverloadImport]
            public IEnumerable<Lazy<Func<object, object>, IOverloadMetadata>> Overloaders { get; set; }            
        }

        private static readonly IDictionary<Type, TypeOverload> _overloads = GenerateOverloads();   

        public static object OverloadInstance(object originInstance)
        {
            object overloadedInstance;

            TypeOverload typeOverLoad;
            if (originInstance != null && _overloads.TryGetValue(originInstance.GetType(), out typeOverLoad))
            {
                overloadedInstance = typeOverLoad.Overloader(originInstance);
            }
            else
            {
                overloadedInstance = originInstance;
            }
            return overloadedInstance;
        }

        public static Type OverLoadType(Type baseType)
        {
            Type returnType = baseType;

            TypeOverload typeOverLoad;
            if (_overloads.TryGetValue(baseType, out typeOverLoad))
            {
                returnType = typeOverLoad.NewType;
            }
            return returnType;
        }

        private static IDictionary<Type, TypeOverload> GenerateOverloads()
        {
            IDictionary<Type, TypeOverload> overloads = new Dictionary<Type, TypeOverload>();
            Importer importer = new Importer();
            var catalog = new DirectoryCatalog(".");
            var container = new CompositionContainer(catalog);
            container.ComposeParts(importer);
            var overloaders = importer.Overloaders;
            if (overloaders != null)
            {
                foreach (Lazy<Func<object, object>, IOverloadMetadata> imports in overloaders)
                {
                    Func<object, object> overloader = imports.Value;
                    IOverloadMetadata metadata = imports.Metadata;
                    TypeOverload overload = new TypeOverload(metadata.BaseType, metadata.NewType, overloader);
                    overloads[metadata.BaseType] = overload;
                }
            }
            return overloads;
        }

        private static IEnumerable<MethodInfo> GetMethods(Assembly assembly)
        {
            Type[] types = assembly.GetTypes();
            List<MethodInfo> methods = new List<MethodInfo>();
            foreach (Type type in types)
            {
                IEnumerable<MethodInfo> typeMethods = from method in type.GetMethods()
                where (method.GetCustomAttribute(typeof(TxOverloadAttribute)) != null)
                select method;
                methods.AddRange(typeMethods);
            }
            return methods;
        }
    }
}
