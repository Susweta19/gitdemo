using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TxSelenium.NativeTests.WebElements;

namespace TxSelenium.NativeTests.Handlers
{
    public class HandlersFactory
    {
        private static string AssoParserMethodName = "GetAssoParser";
        private static IDictionary<Type, Type> writingHandlersDictionnary;
        private static IDictionary<Type, Type> readingHandlersDictionnary;

        public static IReadingHandler<T> GetReadingHandler<T>()
        {
            Type type;
            if (readingHandlersDictionnary == null)
            {
                InitializeTypesDictionnaries();
            }
            if (!readingHandlersDictionnary.TryGetValue(typeof(T), out type))
            {
                throw new Exception("No implementation found for : " + typeof(T).ToString());
            }
            ConstructorInfo info = type.GetConstructor(new Type[] { });
            return (IReadingHandler<T>)info.Invoke(new object[] { });
        }

        public static IWritingHandler<T> GetWritingHandler<T>()
        {
            Type type;
            if (writingHandlersDictionnary == null)
            {
                InitializeTypesDictionnaries();
            }
            if (!writingHandlersDictionnary.TryGetValue(typeof(T), out type))
            {
                throw new Exception("No implementation found for : " + typeof(T).ToString());
            }
            ConstructorInfo info = type.GetConstructor(new Type[] {});
            return (IWritingHandler<T>)info.Invoke(new object[] {});
        }

        public static Func<WERefreshed, object> GetAssoParser(Type outputType)
        {
            Type type;
            if (readingHandlersDictionnary == null)
            {
                InitializeTypesDictionnaries();
            }
            if (!readingHandlersDictionnary.TryGetValue(outputType, out type))
            {
                throw new Exception("No implementation found for : " + outputType.ToString());
            }
            ConstructorInfo info = type.GetConstructor(new Type[] { });
            object handler = info.Invoke(new object[] { });
            MethodInfo theMethod = type.GetMethod(AssoParserMethodName);
            return (Func<WERefreshed, object>)theMethod.Invoke(handler, new object[] { });
        }

        private static void InitializeTypesDictionnaries()
        {
            writingHandlersDictionnary = new Dictionary<Type, Type>();
            readingHandlersDictionnary = new Dictionary<Type, Type>();
            
            IEnumerable<Type> allHandlerTypes = GetHandlersTypes(typeof(IWritingHandler<>));
            RegisterTypes(typeof(IWritingHandler<>), allHandlerTypes, writingHandlersDictionnary);

            allHandlerTypes = GetHandlersTypes(typeof(IReadingHandler<>));
            RegisterTypes(typeof(IReadingHandler<>), allHandlerTypes, readingHandlersDictionnary);
        }

        private static IEnumerable<Type> GetHandlersTypes(Type handlerInterfaceType)
        {
            return from x in Assembly.GetAssembly(typeof(HandlersFactory)).GetTypes()
                   let y = x.GetInterfaces()
                   where !x.IsAbstract && !x.IsInterface &&
                   y.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterfaceType)
                   select x;
        }

        private static void RegisterTypes(Type handlerType, IEnumerable<Type> allHandlerTypes, IDictionary<Type, Type> typeDictionnary)
        {
            foreach (Type implementationType in allHandlerTypes)
            {
                List<Type> dataTypes = new List<Type>();
                foreach (Type intType in implementationType.GetInterfaces())
                {
                    if (intType.IsGenericType && intType.GetGenericTypeDefinition() == handlerType)
                    {
                        dataTypes.Add(intType.GetGenericArguments()[0]);
                    }
                }
                foreach (Type dataType in dataTypes)
                {
                    if (typeDictionnary.ContainsKey(dataType))
                    {
                        throw new Exception("There is more than one handler implementation for : " + dataType.ToString());
                    }
                    typeDictionnary.Add(dataType, implementationType);
                }
            }
        }
    }
}
