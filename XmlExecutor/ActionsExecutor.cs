using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;
using XmlExecutor.Factories;

namespace XmlExecutor
{
    /// <summary>
    /// This class represents an executor for a tree of chained functions calls.
    /// </summary>
    public class ActionsExecutor 
    {
        public delegate void ExecHandler(object instance, TxNode<MethodCall> callingTree, MethodSignature signature);
        public delegate void ErrorHandler(object instance, TxNode<MethodCall> callingTree, MethodSignature signature, Exception e);

        public event ExecHandler BeforeExec;
        public event ExecHandler AfterExec;
        public event ErrorHandler ExecError;

        public string Name { get; private set; }
        public string FailedAction { get; private set; }
        public IDictionary<string, IList> ExtractedData { get; private set; }

        private readonly TxNode<MethodCall> _callingTree;
        private readonly AttributeUtils _attributeUtils;
        private static IDictionary<string, object> _valuesToReplace;

        /// <summary>
        /// Creates the executor from a tree of chained functions calls.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="callingTree">the function calls tree</param>
        /// <param name="attributeUtils"></param>
        public ActionsExecutor(AttributeUtils attributeUtils, string name, TxNode<MethodCall> callingTree)
        {
            _callingTree = callingTree;
            Name = name;
            FailedAction = "";
            _attributeUtils = attributeUtils;
            _valuesToReplace = new Dictionary<string, object>();
            ExtractedData = new Dictionary<string, IList>();
        }

        /// <summary>
        /// Executes the call tree on a specific instance.
        /// </summary>
        /// <param name="rootInstance">the instance used as the root for the function calls</param>
        /// <returns>false if an exception is raised during the calls or if a returned value does not have the expected value</returns>
        public bool Execute(object rootInstance)
        {
            bool passed = true;
            Type rootType = rootInstance.GetType();
            SignatureStorage storage = _attributeUtils.GetSignatureStorage();
            foreach (TxNode<MethodCall> childCall in _callingTree.Children)
            {
                string childName = childCall.Content.Name;
                MethodSignature nextSignature = storage.GetSignatures(rootType).First(s => s.Name == childName);
                try
                {
                    OnBeforeExec(rootInstance, _callingTree, nextSignature);
                    passed = ExecuteCall(rootInstance, childCall, nextSignature);
                    OnAfterExec(rootInstance, _callingTree, nextSignature);
                }
                catch (Exception e)
                {
                    passed = false;
                    OnExecError(rootInstance, _callingTree, nextSignature, e);
                }
                if (!passed) break;
            }
            return passed;
        }

        /// <summary>
        /// Executes a single call. If the call has children, this function 
        /// will call itself for each one of them with the corresponding parameters.
        /// </summary>
        /// <param name="instance">the object instance from which the call should be invoked</param>
        /// <param name="callingTree">the node of the current call from the tree</param>
        /// <param name="signature">the signature object of the method to call</param>
        /// <returns>false if an exception is raised during the calls or if a returned value does not have the expected value</returns>
        private bool ExecuteCall(object instance, TxNode<MethodCall> callingTree, MethodSignature signature)
        {
            MethodCall call = callingTree.Content;
            
            Type objectType = instance.GetType();
            bool passed = true;
            MethodInfo methodInfo = objectType.GetMethod(signature.Info.Name);
            if (methodInfo.IsGenericMethodDefinition)
            {
                methodInfo = methodInfo.MakeGenericMethod(call.CallValues.GenTypes.ToArray());
            }
            try
            {
                object[] parameters = ReflectionUtils.MapParameters(methodInfo, call.CallValues.Values, _valuesToReplace);
                object returnedObject = methodInfo.Invoke(instance, parameters);
                returnedObject = OverloadUtils.OverloadInstance(returnedObject);
                Type returnType = methodInfo.ReturnType;
                returnType = OverloadUtils.OverLoadType(returnType);
                SignatureStorage storage = _attributeUtils.GetSignatureStorage();

                if (call.CallValues.Hashtag != null)
                {                    
                    _valuesToReplace[call.CallValues.Hashtag] = returnedObject;
                    Console.WriteLine("tag : " + call.CallValues.Hashtag.ToString() + " value : " + returnedObject.ToString());
                }
                if (call.CallValues.ExpectedReturn != null)
                {
                    object replacingObject;
                    object expectedReturn = _valuesToReplace.TryGetValue(call.CallValues.ExpectedReturn.ToString(), out replacingObject) ? replacingObject : call.CallValues.ExpectedReturn;

                    if (expectedReturn is string)
                    {
                        expectedReturn = call.CallValues.ExpectedReturn.ToString();
                        foreach (var item in _valuesToReplace)
                            if (call.CallValues.ExpectedReturn.ToString().Contains(item.Key.ToString()))
                                expectedReturn = expectedReturn.ToString().Replace(item.Key.ToString(), item.Value.ToString());
                    }

                    passed = CheckReturnValue(returnedObject, expectedReturn);
                    if (!passed)
                     {
                        Console.WriteLine("return object : " + returnedObject.ToString());

                        Console.WriteLine("expected return : " + expectedReturn.ToString());

                        FailedAction = call.Name;
                        //to print failed method details
                        Console.WriteLine("***********Start of Method Details**********");
                        Console.WriteLine("Failed at : " + call.Name);
                        try
                        {
                            if (call.CallValues.Values.Count > 0)
                            {
                                Console.WriteLine("Parameters :");
                                foreach (var value in call.CallValues.Values)
                                    Console.WriteLine(value.Key + "=" + value.Value);
                            }
                        }
                        catch (Exception exp)
                        {
                            Console.WriteLine(FailedAction + " Method does not have any parameter.");
                            Console.WriteLine("Execption : " + exp.ToString());
                        }
                        Console.WriteLine("***********End of Method Details**********");

                        throw new Exception("Expected value not found\n\t\tReturned Object-->" + returnedObject.ToString());
                    }
                }
                else if (returnType != typeof(void) && _attributeUtils.IsActionType(returnType))
                {
                    IEnumerable<MethodSignature> signatures = storage.GetSignatures(returnType);
                    foreach (TxNode<MethodCall> childCall in callingTree.Children)
                    {
                        string childName = childCall.Content.Name;
                        MethodSignature nextSignature = signatures.First(s => s.Name == childName);
                        OnBeforeExec(returnedObject, childCall, nextSignature);
                        passed = ExecuteCall(returnedObject, childCall, nextSignature);
                        OnAfterExec(returnedObject, childCall, nextSignature);
                        if (!passed)
                        {
                            break;
                        }
                    }
                }

                TxExtractOutputAttribute extractAttr = (TxExtractOutputAttribute) methodInfo.GetCustomAttribute(typeof(TxExtractOutputAttribute));
                if (extractAttr != null)
                {
                    var fullName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;
                    IList dataList;
                    if ( ! ExtractedData.TryGetValue(fullName, out dataList))
                    {
                        dataList = new ArrayList();
                        ExtractedData[fullName] = dataList;
                    }
                    dataList.Add(returnedObject);
                }

            }
            catch (Exception e)
            {
                passed = false;
                FailedAction = call.Name;
                
                OnExecError(instance, callingTree, signature, e);
            }
            return passed;
        }

        /// <summary>
        /// Checks if a returned object is equal, value wise, to an expected object.
        /// </summary>
        /// <param name="returnedObject">the returned object</param>
        /// <param name="expectedReturn">the expected object</param>
        /// <returns></returns>
        private bool CheckReturnValue(object returnedObject, object expectedReturn)
        {
            bool passed = true;
            if (returnedObject is ICollection)
            {
                ICollection returnedCollection = returnedObject as ICollection;
                ICollection expectedCollection = expectedReturn as ICollection;
                if (returnedCollection.Count != expectedCollection.Count)
                {
                    passed = false;
                }
                else
                {
                    IEnumerator returnedEnum = returnedCollection.GetEnumerator();
                    IEnumerator expectedEnum = expectedCollection.GetEnumerator();
                    while (returnedEnum.MoveNext() && expectedEnum.MoveNext())
                    {
                        passed = passed && expectedEnum.Current.Equals(returnedEnum.Current);
                    }
                }
            }
            else
            {
                if (expectedReturn.ToString().Contains('|'))
                {
                    String[] expResult;
                    expResult = expectedReturn.ToString().Split('|');
                    foreach(object value in expResult)
                    {
                        passed = passed | CheckReturnValueForRandomIssue(returnedObject, value);
                    }
                }
                else
                passed = expectedReturn.Equals(returnedObject);
            }
            return passed;
        }

        private bool CheckReturnValueForRandomIssue(object returnedObject, object expectedReturn)
        {
            bool passed = true;
               passed = expectedReturn.Equals(returnedObject);
            return passed;
        }

        private void OnBeforeExec(object instance, TxNode<MethodCall> callingTree, MethodSignature signature)
        {
            BeforeExec?.Invoke(instance, callingTree, signature);
        }

        private void OnAfterExec(object instance, TxNode<MethodCall> callingTree, MethodSignature signature)
        {
            AfterExec?.Invoke(instance, callingTree, signature);
        }

        private void OnExecError(object instance, TxNode<MethodCall> callingTree, MethodSignature signature, Exception e)
        {
            ExecError?.Invoke(instance, callingTree, signature, e);
        }
    }
}
