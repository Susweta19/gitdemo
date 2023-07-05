using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XmlExecutor.Attributes;

namespace XmlExecutor.Factories
{
    public class ReflectionUtils
    {

        /// <summary>
        /// Puts the named parameters from a dictionnary into an array
        /// with the right order for them to be used to invoke a method base.
        /// If a parameter is not present in the dictionnary it will be added 
        /// as missing, this allows the invocation to procede as long as there 
        /// is a default value for this parameter
        /// </summary>
        /// <param name="method">the method</param>
        /// <param name="namedParameters">the parameters dictionary</param>
        /// <returns>the parameters values array</returns>
        public static object[] MapParameters(MethodBase method, IDictionary<string, object> namedParameters, IDictionary<string, object> valuesToReplace = null)
        {
            string[] paramNames = method.GetParameters().Select(p => p.Name).ToArray();
            object[] parameters = new object[paramNames.Length];
            for (int i = 0; i < parameters.Length; ++i)
            {
                parameters[i] = Type.Missing;
            }
            foreach (var item in namedParameters)
            {
                var paramName = item.Key;
                var paramIndex = Array.IndexOf(paramNames, paramName);
                parameters[paramIndex] = item.Value;
                if (valuesToReplace != null)
                {
                    if (item.Value is IList)
                    {
                        ReplaceCollValue((IList)item.Value, valuesToReplace);
                    }
                    else
                    {
                        object replacingValue = null;

                        if (valuesToReplace.TryGetValue(item.Value.ToString(), out replacingValue))
                        {
                            if (item.Value.GetType() != replacingValue.GetType() && item.Value is string)
                            {
                                replacingValue = replacingValue.ToString();
                            }
                            parameters[paramIndex] = replacingValue;
                        }
                        else if (item.Value is string)
                        {
                            string resultValue = "";
                            string[] parts = item.Value.ToString().Split('#');
                            //TODO Hashtag Concatenation with better way.
                            if (item.Value.ToString().IndexOf('#')>0 && item.Value.ToString().Count(c => c == '#')%2==0)
                            {
                                foreach (string data in parts)
                                {
                                    if (valuesToReplace.TryGetValue("#" + data + "#", out replacingValue))
                                        resultValue += replacingValue;
                                    else
                                        resultValue += data;
                                }
                                parameters[paramIndex] = resultValue;
                            }
                            else
                            {
                                foreach (var val in valuesToReplace)
                                {
                                    if (item.Value.ToString().Contains(val.Key))
                                    {
                                        replacingValue = item.Value.ToString().Replace(val.Key, val.Value.ToString());
                                        parameters[paramIndex] = replacingValue;
                                    }
                                }
                            }
                        }

                    }
                }
            }
            return parameters;
        }

        private static void ReplaceCollValue(IList values, IDictionary<string, object> valuesToReplace)
        {
            for ( int i = 0; i < values.Count; i++)
            {
                object value = values[i];
                if (value is IList)
                {
                    ReplaceCollValue((IList)value, valuesToReplace);
                }
                else
                {
                    object replacingValue = null;

                    if (valuesToReplace.TryGetValue(value.ToString(), out replacingValue))
                    {
                        if (value.GetType() != replacingValue.GetType() && value is string)
                        {
                            replacingValue = replacingValue.ToString();
                        }
                        values[i] = replacingValue;
                    }
                    else if (value is string)
                    {
                        //TODO Hashtag Concatenation with better way.
                        foreach (var val in valuesToReplace)
                        {
                            if (value.ToString().Contains(val.Key))
                            {
                                replacingValue = value.ToString().Replace(val.Key, val.Value.ToString());
                                values[i] = replacingValue;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the constructor with the TxConstAttribute from a type
        /// or the first constructor found if there is none.
        /// </summary>
        /// <param name="type">the type</param>
        /// <returns>the constructor</returns>
        public static ConstructorInfo GetConstructor(Type type)
        {
            ConstructorInfo returnConst;

            Func<ConstructorInfo, bool> selector =
                constr => constr.GetCustomAttributes().Any(attr => attr.GetType() == typeof(TxConstAttribute));
            if (type.GetConstructors().Any(selector))
            {
                returnConst = type.GetConstructors().First(selector);
            }
            else
            {
                returnConst = type.GetConstructors().First();
            }
            return returnConst;
        }

        /// <summary>
        /// Gets the types implementing every interfaces from an array in all currently loaded assemblies.
        /// </summary>
        /// <param name="interfaceTypes">the array of interfaces types</param>
        /// <returns>an enumerable over every type implementing all interfaces</returns>
        public static IEnumerable<Type> GetImplementingTypes(Type[] interfaceTypes)
        {
            bool acceptInterface = true;
            for (int i = 0; i < interfaceTypes.Length; i++)
            {
                Type interf = interfaceTypes[i];
                acceptInterface = interf.IsDefined(typeof(TxGenericAttribute)) && interf.GetCustomAttribute<TxGenericAttribute>().AllowInterfaces;
                if ( ! acceptInterface)
                {
                    break;
                }
            }
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            IEnumerable<Type> returnEnum = Enumerable.Empty<Type>();
            foreach (Assembly assembly in assemblies)
            {
                try
                {
                    returnEnum = returnEnum.Concat(GetImplementingTypes(assembly, interfaceTypes, acceptInterface));
                }
                catch (Exception)
                {
                    //TODO find better logging system or a way to exclude the test library which causes this exception
                    Console.WriteLine("GetImplementingTypes : Failed to get type from assembly : " + assembly.FullName);
                }
            }
            return returnEnum;
        }

        /// <summary>
        /// Get all types implementing every passed interface types.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="interfaceTypes"></param>
        /// <param name="acceptInterface"></param>
        /// <returns></returns>
        private static IEnumerable<Type> GetImplementingTypes(Assembly assembly, Type[] interfaceTypes, bool acceptInterface)
        {
            return from type in assembly.GetTypes()
                   let interfaces = type.GetInterfaces()
                   where (acceptInterface || (!type.IsAbstract && !type.IsInterface)) &&
                   interfaceTypes.All(constraint => interfaces.Any(interf => interf == constraint))
                   select type;
        }

        /// <summary>
        /// Computes the cartesian product of an enumerable of enumerable.
        /// taken from http://ericlippert.com/2010/06/28/computing-a-cartesian-product-with-linq/
        /// </summary>
        /// <typeparam name="T">the enumerable content type</typeparam>
        /// <param name="sequences">the enumerable of enumerable</param>
        /// <returns>the cartesian product</returns>
        public static IEnumerable<IEnumerable<T>> CartesianProduct<T>(IEnumerable<IEnumerable<T>> sequences)
        {
            IEnumerable<IEnumerable<T>> emptyProduct = new[] { Enumerable.Empty<T>() };
            return sequences.Aggregate(
              emptyProduct,
              (accumulator, sequence) =>
                from accseq in accumulator
                from item in sequence
                select accseq.Concat(new[] { item }));
        }

        /// <summary>
        /// Checks whether or not a type is a generic collection type
        /// </summary>
        /// <param name="type">the type to check</param>
        /// <returns>true if the type is a generic collection type</returns>
        public static bool IsGenericColl(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>);
        }

        /// <summary>
        /// Creates a generic list with a specified inner type.
        /// </summary>
        /// <param name="innerType">the innertype</param>
        /// <returns>the generic list</returns>
        public static IList CreateGenericList(Type innerType)
        {
            Type listType = typeof(List<>).MakeGenericType(innerType);
            IList list = (IList)Activator.CreateInstance(listType);
            return list;
        }

    }
}
