using System.Collections;
using System.Collections.Generic;
using System.Text;
using XmlExecutor.DataTypes;

namespace TxWebTests.Spreadsheet
{
    public class ScenarioAction
    {
        public ICollection<Action> Actions;

        public ScenarioAction()
        {
            Actions = new List<Action>();
        }

        public void AddAction(TxNode<MethodCall> node, MethodSignature signature)
        {
            Actions.Add(new Action(signature.Name, node));
        }

        public override string ToString()
        {
            StringBuilder scenario = new StringBuilder();
            foreach (Action action in Actions)
            {
                scenario.AppendLine(action.Depth + action.ToString());
            }
            return scenario.ToString().TrimEnd();
        }
    }


    public class Action
    {
        private readonly string _actionName;
        public string Depth;
        private readonly IDictionary<string, object> _parameters;
        private readonly object _expectedResult;
        private readonly object _hashtag;

        public Action(string actionName, TxNode<MethodCall> node)
        {
            _actionName = actionName;
            Depth = string.Empty.PadLeft(node.Depth, '\t');

            if (node.Content.CallValues?.Values != null)
                _parameters = node.Content.CallValues.Values;

            if (node.Content.CallValues?.ExpectedReturn != null)
                _expectedResult = node.Content.CallValues.ExpectedReturn;

            if (node.Content.CallValues?.Hashtag != null)
                _hashtag = node.Content.CallValues.Hashtag;
        }

        public override string ToString()
        {
            StringBuilder action = new StringBuilder();
            action.Append(_actionName);

            if (_parameters != null)
            {
                action.AppendLine();
                foreach (var param in _parameters)
                {
                    action.AppendLine(Depth + "\t" + param.Key.ToString() + " : " + ToString(param.Value, Depth + "\t"));
                }
            }

            if (_expectedResult != null)
                action.AppendLine(Depth + "\tExpectedResult : " + ToString(_expectedResult, Depth + "\t"));

            if (_hashtag != null)
                action.AppendLine(Depth + "\tHashtag : " + ToString(_hashtag, Depth + "\t"));

            return action.ToString().TrimEnd();
        }

        //TODO recusive function
        /// <summary>
        /// To string for param and expected result
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="indentation"></param>
        /// <returns></returns>
        private string ToString(object obj, string indentation)
        {
            StringBuilder value = new StringBuilder();
           
             if (obj.GetType().Name.Contains("ActionColl"))
            {
                value.AppendLine();
                foreach (var item in (obj as IEnumerable))
                {
                    value.AppendLine(indentation + "\t" + "collection : " + item.ToString());
                }
                return value.ToString().TrimEnd();
            }
            else
                return obj.ToString();
        }
    }
}
