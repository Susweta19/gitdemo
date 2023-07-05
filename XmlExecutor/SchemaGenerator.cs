using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using XmlExecutor.DataTypes;
using XmlExecutor.Factories;
using XmlExecutor.Utils;

namespace XmlExecutor
{
    /// <summary>
    /// This class manages the generation of an XmlSchema from an action type and its xsd file.
    /// </summary>
    public class SchemaGenerator
    {
        /// <summary>
        /// Generates a xsd file from a root type to a file path.
        /// </summary>
        /// <param name="attributeUtils"></param>
        /// <param name="rootType">the root type</param>
        /// <param name="filePath">the path of the file to generate</param>
        public static void GenerateXsd(AttributeUtils attributeUtils, Type rootType, string filePath)
        {
            SchemaGenerator generator = new SchemaGenerator(attributeUtils, rootType);
            XmlSchema schema = generator.Schema;
            FileStream file = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            XmlTextWriter xwriter = new XmlTextWriter(file, new UTF8Encoding()) {Formatting = Formatting.Indented};
            schema.Write(xwriter);
        }

        private IDictionary<Type, XmlQualifiedName> _generatedTypes;
        private readonly AttributeUtils _attributeUtils;

        /// <summary>
        /// The schema of this generator.
        /// </summary>
        public XmlSchema Schema { get; private set; }

        /// <summary>
        /// The constructor.
        /// </summary>
        /// <param name="attributeUtils"></param>
        /// <param name="rootType">the type from which the actions tree will be computed</param>
        public SchemaGenerator(AttributeUtils attributeUtils, Type rootType)
        {
            Schema = new XmlSchema();
            _attributeUtils = attributeUtils;
            _generatedTypes = new Dictionary<Type, XmlQualifiedName>();
            SignatureStorage storage = _attributeUtils.GetSignatureStorage();
            IEnumerable<MethodSignature> signatures = storage.GetSignatures(rootType);
            XmlSchemaChoice rootChoice = new XmlSchemaChoice
            {
                MinOccurs = 1,
                MaxOccursString = "unbounded"
            };
            foreach (MethodSignature signature in signatures)
            {
                XmlSchemaElement element = GenerateElement(signature.Name, signature.Comment, signature.Info);
                rootChoice.Items.Add(element);
            }

            XmlSchemaElement rootElement = new XmlSchemaElement {Name = "Actions"};
            XmlSchemaComplexType rootSchemaType = new XmlSchemaComplexType {Particle = rootChoice};
            rootElement.SchemaType = rootSchemaType;
            Schema.Items.Add(rootElement);
        }

        /// <summary>
        /// Generates the schema element for one method.
        /// </summary>
        /// <param name="name">the name of the element</param>
        /// <param name="comment">the comment for the element</param>
        /// <param name="methodInfo">the method signature</param>
        /// <returns>the element</returns>
        private XmlSchemaElement GenerateElement(string name, string comment, MethodInfo methodInfo)
        {
            XmlSchemaElement element = new XmlSchemaElement {Name = name};

            if (!string.IsNullOrEmpty(comment))
            {
                XmlSchemaAnnotation annotation = new XmlSchemaAnnotation();
                XmlSchemaDocumentation documentation = new XmlSchemaDocumentation();
                //Passing by a document object creation to add an annotation is the way advised by the msdn
                XmlDocument xmlDoc = new XmlDocument();
                documentation.Markup = new XmlNode[] { xmlDoc.CreateTextNode(comment) };
                annotation.Items.Add(documentation);
                element.Annotation = annotation;
            }

            XmlSchemaGroupBase group;
            if (methodInfo.IsGenericMethodDefinition)
            {
                group = GenerateGenericElements(methodInfo);
            }
            else
            {
                ParameterInfo[] paramsInfos = methodInfo.GetParameters();

                group = new XmlSchemaSequence();
                XmlSchemaElement messageElement = new XmlSchemaElement
                {
                    Name = XmlNames.Message,
                    SchemaTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema"),
                    MaxOccurs = 1,
                    MinOccurs = 0
                };
                group.Items.Add(messageElement);

                if (paramsInfos.Count() != 0)
                {
                    XmlSchemaElement parametersElement = new XmlSchemaElement
                    {
                        Name = XmlNames.Parameters,
                        MaxOccurs = 1,
                        MinOccurs = 1
                    };
                    XmlSchemaSequence paraSeq = new XmlSchemaSequence();
                    GenerateParameters(paraSeq, methodInfo);

                    XmlSchemaComplexType paramType = new XmlSchemaComplexType {Particle = paraSeq};
                    parametersElement.SchemaType = paramType;

                    group.Items.Add(parametersElement);
                }

                Type returnType = methodInfo.ReturnType;
                if (returnType != typeof(void))
                {
                    returnType = OverloadUtils.OverLoadType(returnType);
                    group.Items.Add(GenerateReturn(returnType));
                    XmlSchemaElement hashtagElem = GenerateHashtag(returnType);
                    if (hashtagElem != null) group.Items.Add(hashtagElem);
                }
            }

            XmlSchemaComplexType elemType = new XmlSchemaComplexType {Particle = @group};
            element.SchemaType = elemType;
            return element;
        }

        /// <summary>
        /// Generates the element for one generic method allowing to choose
        /// between every combination of the allowed types definition.
        /// </summary>
        /// <param name="methodInfo">the method signature</param>
        /// <returns>the choice element</returns>
        private XmlSchemaChoice GenerateGenericElements(MethodInfo methodInfo)
        {
            Type[] genericTypes = methodInfo.GetGenericArguments();
            ICollection<IEnumerable<Type>> implementingTypesList = new List<IEnumerable<Type>>();

            foreach (Type genericType in genericTypes)
            {
                Type[] constraints = genericType.GetGenericParameterConstraints();
                IEnumerable<Type> types = ReflectionUtils.GetImplementingTypes(constraints);
                implementingTypesList.Add(types);
            }

            XmlSchemaChoice genChoice = new XmlSchemaChoice();
            IEnumerable<IEnumerable<Type>> genericParamCombis = ReflectionUtils.CartesianProduct<Type>(implementingTypesList);

            foreach (IEnumerable<Type> genericParamCombi in genericParamCombis)
            {
                MethodInfo constMethodInfo = methodInfo.MakeGenericMethod(genericParamCombi.ToArray());
                string name = "";
                foreach (Type genType in genericParamCombi)
                {
                    if (name.Count() != 0)
                    {
                        name = name + "-";
                    }
                    name = name + genType.Name;
                }
                XmlSchemaElement typeElem = GenerateElement(name, "", constMethodInfo);
                genChoice.Items.Add(typeElem);
            }
            return genChoice;
        }

        /// <summary>
        /// Generates the element for a return value.
        /// </summary>
        /// <param name="returnType">the type of the return value</param>
        /// <returns>the element</returns>
        private XmlSchemaElement GenerateReturn(Type returnType)
        {
            XmlSchemaElement element = new XmlSchemaElement();
            if (_attributeUtils.IsActionType(returnType))
            {
                element = GenerateActions(returnType);
            }
            else
            {
                element = GenerateParameter(XmlNames.ExpectedResult, returnType);
                element.MinOccursString = "0";
            }
            return element;
        }

        /// <param name="returnType">the type of the return value</param>
        /// <returns>the element</returns>
        private XmlSchemaElement GenerateHashtag(Type returnType)
        {
            XmlSchemaElement element = null;
            if (!_attributeUtils.IsActionType(returnType))
            {
                element = new XmlSchemaElement();
                element = GenerateParameter(XmlNames.Hashtag, typeof(string));
                element.MinOccursString = "0";
            }
            return element;
        }

        /// <summary>
        /// Generates the actions element for a specific action type.
        /// </summary>
        /// <param name="returnType">the action type</param>
        /// <returns>the action element</returns>
        private XmlSchemaElement GenerateActions(Type returnType)
        {
            if (!_generatedTypes.ContainsKey(returnType))
            {
                XmlSchemaComplexType childrenType = new XmlSchemaComplexType();
                if (returnType.IsGenericType)
                {
                    string name = "";
                    name = name + returnType.GetGenericTypeDefinition().FullName;
                    foreach (Type type in returnType.GenericTypeArguments)
                    {
                        name = name + type.FullName;
                    }
                    name = name.Replace('`', '_');
                    childrenType.Name = name;
                }
                else
                {
                    childrenType.Name = returnType.FullName;
                }
                //The dictionary must be filled first to avoid an infinite recursive call loop when encountering cyclic return types.
                _generatedTypes.Add(returnType, new XmlQualifiedName(childrenType.Name));

                XmlSchemaChoice childrenChoice = new XmlSchemaChoice
                {
                    MinOccurs = 0,
                    MaxOccursString = "unbounded"
                };
                IEnumerable<MethodSignature> signatures = _attributeUtils.GetSignatureStorage().GetSignatures(returnType);
                foreach (MethodSignature signature in signatures)
                {
                    XmlSchemaElement childElement = GenerateElement(signature.Name, signature.Comment, signature.Info);
                    childrenChoice.Items.Add(childElement);
                }
                childrenType.Particle = childrenChoice;
                Schema.Items.Add(childrenType);
            }

            XmlSchemaElement childrenElement = new XmlSchemaElement
            {
                Name = XmlNames.Actions,
                SchemaTypeName = _generatedTypes[returnType]
            };
            return childrenElement;
        }

        /// <summary>
        /// Adds every parameter element for a constructor or a method to an Xml group.
        /// </summary>
        /// <param name="parentGroup">the group</param>
        /// <param name="methodBase">the method or constructor signature</param>
        private void GenerateParameters(XmlSchemaGroupBase parentGroup, MethodBase methodBase)
        {
            ParameterInfo[] parametersInfo = methodBase.GetParameters();
            foreach (ParameterInfo paramInfo in parametersInfo)
            {
                XmlSchemaElement parameterElement = GenerateParameter(paramInfo.Name, paramInfo.ParameterType);
                if (paramInfo.IsOptional)
                {
                    parameterElement.MinOccursString = "0";
                }
                parentGroup.Items.Add(parameterElement);
            }
        }

        /// <summary>
        /// Generates the element for a parameter.
        /// </summary>
        /// <param name="name">the name of the parameter</param>
        /// <param name="type">the type of the parameter</param>
        /// <returns>the element</returns>
        private XmlSchemaElement GenerateParameter(string name, Type type)
        {
            XmlSchemaElement parameterElement = new XmlSchemaElement
            {
                Name = name,
                MaxOccursString = "1",
                MinOccursString = "1"
            };
            if (type == typeof(string))
            {
                parameterElement.SchemaTypeName = new XmlQualifiedName("string", "http://www.w3.org/2001/XMLSchema");
            }
            else if (type == typeof(int))
            {
                parameterElement.SchemaTypeName = new XmlQualifiedName("integer", "http://www.w3.org/2001/XMLSchema");
            }
            else if (type == typeof(double))
            {
                parameterElement.SchemaTypeName = new XmlQualifiedName("decimal", "http://www.w3.org/2001/XMLSchema");
            }
            else if (type == typeof(bool))
            {
                parameterElement.SchemaTypeName = new XmlQualifiedName("boolean", "http://www.w3.org/2001/XMLSchema");
            }
            else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ICollection<>))
            {
                Type subType = type.GetGenericArguments()[0];
                parameterElement = GenerateParameter(name, subType);
                parameterElement.MaxOccursString = "unbounded";
                parameterElement.MinOccursString = "0";
            }
            else
            {
                //Genrating parameters from the complex type constructor 
                XmlSchemaSequence seq = new XmlSchemaSequence();
                GenerateParameters(seq, ReflectionUtils.GetConstructor(type));
                XmlSchemaComplexType elemType = new XmlSchemaComplexType {Particle = seq};
                parameterElement.SchemaType = elemType;
            }
            return parameterElement;
        }

    }
}
