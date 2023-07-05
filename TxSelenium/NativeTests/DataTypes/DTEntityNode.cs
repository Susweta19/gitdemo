using System.Collections.Generic;
using System.Linq;
using XmlExecutor.Attributes;
using XmlExecutor.DataTypes;

namespace TxSelenium.NativeTests.DataTypes
{
    /// <summary>
    /// Represents a node in a tree.
    /// </summary>
    public class DTEntityNode : IWritableData
    {

        public static DTEntityNode CreateNodeFromPath(string[] path)
        {
            DTEntityNode lastCreatedNode = null;
            for (int i = 0; i < path.Length; i++)
            {
                lastCreatedNode = new DTEntityNode(path[i], lastCreatedNode);
            }
            return lastCreatedNode;
        }

        public DTEntityNode parent;
        public OrderedSet<DTEntityNode> Children { get; private set; }
        private string name;

        /// <summary>
        /// The constructor if the parent should be null if this is the root node.
        /// If the parent is not null it will be modified since this node will be
        /// added as one of its children.
        /// </summary>
        /// <param name="name">the name of the node</param>
        /// <param name="parent">the parent of the node</param>
        public DTEntityNode(string name, DTEntityNode parent = null)
        {
            this.name = name;
            this.parent = parent;
            if (parent != null)
            {   //TODO this makes the class mutable and a way to avoid this should be found, 
                // if necessary changing the data structure
                parent.AddChild(this);
            }
        }

        [TxConst]
        public DTEntityNode(ICollection<string> path)
        {
            DTEntityNode lastCreatedNode = null;
            int count = path.Count();
            for (int i = 0; i < count - 1; i++)
            {
                lastCreatedNode = new DTEntityNode(path.ElementAt(i), lastCreatedNode);
            }
            this.name = path.ElementAt(count - 1);
            this.parent = lastCreatedNode;
        }

        /// <summary>
        /// Adds a child to this node.
        /// </summary>
        /// <param name="child">the child to add</param>
        /// <returns>true if the child was successfully added</returns>
        private bool AddChild(DTEntityNode child)
        {
            if (Children == null)
                Children = new OrderedSet<DTEntityNode>();
            return Children.Add(child);
        }

        /// <summary>
        /// Gets the path from the root node including this node.
        /// </summary>
        /// <returns></returns>
        public ICollection<string> GetPath()
        {
            ICollection<string> path;
            if (parent != null)
                path = parent.GetPath();
            else
                path = new List<string>();
            path.Add(name);
            return path;
        }

        public override string ToString()
        {
            string strToReturned = "";
            foreach (string str in this.GetPath())
                strToReturned = strToReturned + str + ";";

            return strToReturned.TrimEnd(new char[] { ';' });
        }
    }
}
