using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using XmlExecutor.DataTypes;
using XmlExecutor.Factories;
using XmlExecutor.Utils;

namespace XmlExecutor
{
    public delegate void ParseErrorLogger(string fileName, Exception e);

    /// <summary>
    /// This class is used to parse xml files into actions executors.
    /// </summary>
    public class ActionsParser
    {
        private static Func<XElement, bool> _isResult = elem => elem.Name.LocalName == XmlNames.ExpectedResult;
        private static Func<XElement, bool> _isAction = elem => elem.Name.LocalName == XmlNames.Actions;

        private Type _rootType;
        private IDictionary<Type, Func<string, object>> _baseConvDic;
        private readonly IDictionary<string, string> _stringsToReplace;
        private readonly XmlSchemaSet _schemaSet;
        private AttributeUtils _attributeUtils;

        /// <summary>
        /// Construct an action parser from a root type.
        /// </summary>
        /// <param name="attributeUtils"></param>
        /// <param name="rootType">the root type</param>
        public ActionsParser(AttributeUtils attributeUtils, Type rootType, IDictionary<string, string> stringsToReplace = null)
        {
            this._rootType = rootType;
            this._baseConvDic = CreateBaseConverters();
            this._stringsToReplace = stringsToReplace;
            _attributeUtils = attributeUtils;
            SchemaGenerator generator = new SchemaGenerator(_attributeUtils, rootType);
            XmlSchema schema = generator.Schema;
            this._schemaSet = new XmlSchemaSet();
            _schemaSet.Add(schema);
        }

        /// <summary>
        /// Parses xml files from a directory and all its children into action executors.
        /// </summary>
        /// <param name="xmlFolderOrFile">the path to the folder</param>
        /// <returns>the collection of xml files</returns>
        public ICollection<ActionsExecutor> ParseFolder(string xmlFolderOrFile, ParseErrorLogger logParseError)
        {
            ICollection<ActionsExecutor> testParameters = new List<ActionsExecutor>();

            if (xmlFolderOrFile.Contains(".xml"))
            {
                try
                {
                    testParameters.Add(ParseFile(xmlFolderOrFile));
                }
                catch (XmlSchemaValidationException e)
                {
                    logParseError?.Invoke(xmlFolderOrFile, e);
                }
                catch (XmlException e)
                {
                    logParseError?.Invoke(xmlFolderOrFile, e);
                }
            }
            else {
                DirectoryInfo dirInfo = new DirectoryInfo(xmlFolderOrFile);
                IEnumerable<FileInfo> files = dirInfo.EnumerateFiles("*.xml", SearchOption.AllDirectories).OrderBy(file => file.FullName);
                foreach (FileInfo file in files)
                {
                    try
                    {
                        testParameters.Add(ParseFile(file.FullName));
                    }
                    catch (XmlSchemaValidationException e)
                    {
                        logParseError?.Invoke(file.FullName, e);
                    }
                    catch (XmlException e)
                    {
                        logParseError?.Invoke(file.FullName, e);
                    }
                }
            }
            return testParameters;
        }

        /// <summary>
        /// Parse a single file into an action executor.
        /// </summary>
        /// <param name="xmlPath">the path to the xml file</param>
        /// <returns>the action executor parsed from the file</returns>
        public ActionsExecutor ParseFile(string xmlPath)
        {
            XDocument xDoc = XDocument.Load(xmlPath);
            xDoc.Validate(_schemaSet, null);
            XElement elem = xDoc.Element(XmlNames.Actions);
            MethodCall firstCall = new MethodCall(_rootType.Name, null);
            TxNode<MethodCall> rootCall = new TxNode<MethodCall>(firstCall, null);
            SignatureStorage storage = _attributeUtils.GetSignatureStorage();
            IEnumerable<MethodSignature> signatures = storage.GetSignatures(_rootType);
            ParseActions(elem, rootCall, _rootType);
            int index = xmlPath.LastIndexOf(".");
            string name = Path.GetFileNameWithoutExtension(xmlPath);
            return new ActionsExecutor(_attributeUtils, name, rootCall);
        }

        /// <summary>
        /// Parses the children nodes of an element into method call nodes
        ///  and add them as children to the call passed as a parameter.
        /// </summary>
        /// <param name="actionsElement">the element containing action nodes</param>
        /// <param name="parentCall">the parent call node</param>
        /// <param name="returnType">the type returned by the parent call</param>
        private void ParseActions(XElement actionsElement, TxNode<MethodCall> parentCall, Type returnType)
        {
            IEnumerable<XElement> elements = actionsElement.Elements();
            SignatureStorage storage = _attributeUtils.GetSignatureStorage();
            IEnumerable<MethodSignature> signatures = storage.GetSignatures(returnType);
            foreach (XElement element in elements)
            {
                string elemName = element.Name.LocalName;
                MethodSignature childSign = signatures.First(s => s.Name == elemName);
                TxNode<MethodCall> childCall = ParseAction(element, elemName, childSign, parentCall);
                ParseActionsNode(element, childCall, childSign);
            }
        }

        /// <summary>
        /// Parses a single method call node from an xml element and add it, as a child, to its parent.
        /// </summary>
        /// <param name="element">the element representing the action</param>
        /// <param name="name">the name of the action</param>
        /// <param name="signature">the call's signature</param>
        /// <param name="parentCall">the call's parent </param>
        /// <returns>the node containing the parsed call</returns>
        private TxNode<MethodCall> ParseAction(XElement element, string name, MethodSignature signature, TxNode<MethodCall> parentCall)
        {
            MethodInfo methodInfo = signature.Info;
            TxNode<MethodCall> childCall;
            IEnumerable<XElement> elements = element.Elements();
            if (methodInfo.IsGenericMethodDefinition)
            {
                XElement genericNode = elements.First();
                Type[] genericTypes = methodInfo.GetGenericArguments();
                Type[] actualTypes = new Type[genericTypes.Count()];
                string[] typeNames = genericNode.Name.LocalName.Split('-');
                for (int i = 0; i < genericTypes.Count(); i++)
                {
                    Type genericType = genericTypes[i];
                    Type[] constraints = genericType.GetGenericParameterConstraints();
                    IEnumerable<Type> types = ReflectionUtils.GetImplementingTypes(constraints);
                    actualTypes[i] = types.First(t => t.Name == typeNames[i]);
                }
                MethodInfo constMethodInfo = methodInfo.MakeGenericMethod(actualTypes);
                //Parsing so no need for a comment
                MethodSignature constSign = new MethodSignature(name, "", constMethodInfo);
                childCall = ParseAction(genericNode, name, constSign, parentCall);
                ParseActionsNode(genericNode, childCall, constSign);
            }
            else
            {
                childCall = ParseNonGenericAction(name, element, methodInfo, parentCall);
            }
            return childCall;
        }

        /// <summary>
        /// Parses an action for a non generic call or a defined generic call.
        /// </summary>
        /// <param name="element">the element representing the action</param>
        /// <param name="name">the name of the action</param>
        /// <param name="methodInfo">the call's method info</param>
        /// <param name="parentCall">the call's parent </param>
        /// <returns></returns>
        private TxNode<MethodCall> ParseNonGenericAction(string name, XElement element, MethodInfo methodInfo, TxNode<MethodCall> parentCall)
        {
            IEnumerable<XElement> elements = element.Elements();
            object resultValue = null;
            if (elements.Any(_isResult))
            {
                Type returnType = methodInfo.ReturnType;
                returnType = OverloadUtils.OverLoadType(returnType);
                XElement resultElement = elements.First(_isResult);
                IDictionary<string, object> resultDict = new Dictionary<string, object>();
                if (ReflectionUtils.IsGenericColl(returnType))
                {
                    AddListToDic(returnType, XmlNames.ExpectedResult, resultDict);
                }
                ParseParameter(returnType, resultElement, resultDict);
                resultValue = resultDict.Values.First();
            }

            Type[] actualTypes = null;
            if (methodInfo.IsGenericMethod)
            {
                actualTypes = methodInfo.GetGenericArguments();
            }

            XElement parametersNode = element.Element(XmlNames.Parameters);
            IDictionary<string, object> values;
            if (parametersNode != null)
            {
                values = ParseParameters(methodInfo, parametersNode);
            }
            else
            {
                values = new Dictionary<string, object>();
            }

            XElement messageNode = element.Element(XmlNames.Message);
            string message = null;
            if (messageNode != null)
            {
                message = messageNode.Value;
            }

            XElement hashtagNode = element.Element(XmlNames.Hashtag);
            string hashtag = null;
            if (hashtagNode != null)
            {
                hashtag = hashtagNode.Value;
            }

            ParametersValues paramValues = new ParametersValues(values, resultValue, actualTypes, hashtag);
            MethodCall call = new MethodCall(name, paramValues, message);
            return new TxNode<MethodCall>(call, parentCall);
        }

        /// <summary>
        /// Searches for an action node in an element's children and if there is one parses it.
        /// </summary>
        /// <param name="parentXML">an element which possibly contains an action node</param>
        /// <param name="parentCall">the parent call</param>
        /// <param name="parentSign">the parent signature</param>
        private void ParseActionsNode(XElement parentXML, TxNode<MethodCall> parentCall, MethodSignature parentSign)
        {
            if (parentXML.Elements().Any(_isAction))
            {
                XElement actionsElement = parentXML.Elements().First(_isAction);
                ParseActions(actionsElement, parentCall, parentSign.Info.ReturnType);
            }
        }

        /// <summary>
        /// Parses parameters from a parameters node.
        /// </summary>
        /// <param name="methodBase">the object containing the parameters informations</param>
        /// <param name="parametersNode">the node</param>
        /// <returns>a dictionary associating the parameters names to their value</returns>
        private IDictionary<string, object> ParseParameters(MethodBase methodBase, XElement parametersNode)
        {
            IDictionary<string, object> paramDict = new Dictionary<string, object>();
            IEnumerable<XElement> paramElements = parametersNode.Elements();
            ParameterInfo[] paramsInfo = methodBase.GetParameters();
            //First initialize lists parameters
            IEnumerable<ParameterInfo> collParams = paramsInfo.Where(p => ReflectionUtils.IsGenericColl(p.ParameterType));
            foreach (ParameterInfo collParam in collParams)
            {
                AddListToDic(collParam.ParameterType, collParam.Name, paramDict);
            }

            foreach (XElement paramElement in paramElements.Where(e => !(_isResult(e) || _isAction(e))))
            {
                string paramName = paramElement.Name.LocalName;
                Type type = paramsInfo.First(s => s.Name == paramName).ParameterType;
                ParseParameter(type, paramElement, paramDict);
            }

            return paramDict;
        }

        /// <summary>
        /// Parses one parameter from its node equivalent.
        /// If the parameter is a list the dictionary should 
        /// already contain a list associated with its name.
        /// </summary>
        /// <param name="type">the parameter's type</param>
        /// <param name="paramElement">the parameter's element</param>
        /// <param name="paramDict">the dictionnary the parameter's value should be added to</param>
        private void ParseParameter(Type type, XElement paramElement, IDictionary<string, object> paramDict)
        {
            string paramName = paramElement.Name.LocalName;
            if (ReflectionUtils.IsGenericColl(type))
            {
                IList list = (IList)paramDict[paramName];
                Type innerType = type.GetGenericArguments()[0];
                object value = GetParamValue(innerType, paramElement);
                list.Add(value);
            }
            else
            {
                object value = GetParamValue(type, paramElement);
                paramDict.Add(paramName, value);
            }
        }

        /// <summary>
        /// Get the value of a parameter from its xml node an type.
        /// </summary>
        /// <param name="param">the parameter's type</param>
        /// <param name="paramElem">the xml element representing the parameter</param>
        /// <returns>the parameter's value</returns>
        private object GetParamValue(Type param, XElement paramElem)
        {
            object returnValue;
            if (paramElem.HasElements)
            {
                ConstructorInfo constInfo = ReflectionUtils.GetConstructor(param);
                IDictionary<string, object> paramDict = ParseParameters(constInfo, paramElem);
                returnValue = constInfo.Invoke(ReflectionUtils.MapParameters(constInfo, paramDict));
            }
            else
            {
                try
                {
                    returnValue = ConvertParameter(param, paramElem.Value);
                }
                catch (Exception e)
                {
                    throw new Exception(String.Format("Failed to parse parameter name : {0}, type : {1}, value : {2}", paramElem.Name, param.FullName, paramElem.Value), e);
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Converts the string representation of a parameter with a basic type into a value
        /// </summary>
        /// <param name="type">the type</param>
        /// <param name="value">the string representation</param>
        /// <returns>the value</returns>
        private object ConvertParameter(Type type, string value)
        {
            object returnValue;
            if (_baseConvDic.ContainsKey(type))
            {
                if (_stringsToReplace != null)
                {
                    foreach (string key in _stringsToReplace.Keys)
                    {
                        if (value.Contains(key))
                        {
                            value = value.Replace(key, _stringsToReplace[key]);
                        }
                    }
                }
                returnValue = _baseConvDic[type].Invoke(value);
            }
            else
            {
                throw new Exception("Unsupported parameter type  \"" + type + "\" for value \"" + value + "\"");
            }
            return returnValue;
        }

        /// <summary>
        /// Adds a generic list to a parameters dictionary.
        /// </summary>
        /// <param name="paramType">the list constructed generic type</param>
        /// <param name="name">the parameter's name</param>
        /// <param name="paramDict">the dictionnary</param>
        private void AddListToDic(Type paramType, string name, IDictionary<string, object> paramDict)
        {
            Type innerType = paramType.GetGenericArguments()[0];
            IList list = ReflectionUtils.CreateGenericList(innerType);
            paramDict.Add(name, list);
        }

        /// <summary>
        /// Creates a dictionnary containing functions used to convert a string into a basic type.
        /// </summary>
        /// <returns>the dictionnary</returns>
        private IDictionary<Type, Func<string, object>> CreateBaseConverters()
        {
            IDictionary<Type, Func<string, object>> convertersDic = new Dictionary<Type, Func<string, object>>();
            convertersDic.Add(typeof(string), value => value);
            convertersDic.Add(typeof(int), value => int.Parse(value));
            convertersDic.Add(typeof(double), value => double.Parse(value));
            convertersDic.Add(typeof(bool), value => bool.Parse(value));
            return convertersDic;
        }

    }
}
