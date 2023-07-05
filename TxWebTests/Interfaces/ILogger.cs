using System;
using XmlExecutor.DataTypes;

namespace TxWebTests.Interfaces
{
    public interface IWebTestsLogger
    {
        /// <summary>
        /// Called before each action execution.
        /// </summary>
        /// <param name="instance">The instance the action is called from</param>
        /// <param name="callingTree">The calling tree for the current script</param>
        /// <param name="signature">The method signature for the called action</param>                 
        void LogBefore(object instance, TxNode<MethodCall> callingTree, MethodSignature signature);


        /// <summary>
        /// Called after each action execution.
        /// </summary>
        /// <param name="instance">The instance the action is called from</param>
        /// <param name="callingTree">The calling tree for the current script</param>
        /// <param name="signature">The method signature for the called action</param> 
        void LogAfter(object instance, TxNode<MethodCall> callingTree, MethodSignature signature);


        /// <summary>
        /// Called if an action execution encountered an error.
        /// </summary>
        /// <param name="instance">The instance the action is called from</param>
        /// <param name="callingTree">The calling tree for the current script</param>
        /// <param name="signature">The method signature for the called action</param> 
        /// <param name="e">The thrown exception</param> 
        void LogError(object instance, TxNode<MethodCall> callingTree, MethodSignature signature, Exception e);
    }
}
