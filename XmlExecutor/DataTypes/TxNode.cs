namespace XmlExecutor.DataTypes
{
    public class TxNode<T>
    {
        private TxNode<T> _parent;
        public T Content{ get; private set;}
        public int Depth { get; private set; }

        public OrderedSet<TxNode<T>> Children { get; private set; }

        /// <summary>
        /// The constructor if the parent should be null if this is the root node.
        /// If the parent is not null it will be modified since this node will be
        /// added as one of its children.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="parent">the parent of the node</param>
        public TxNode(T content, TxNode<T> parent = null)
        {
            Content = content;
            Children = new OrderedSet<TxNode<T>>();
            this._parent = parent;
            if (parent != null)
            {
                parent.AddChild(this);
                Depth = parent.Depth + 1;
            }
            else
            {
                Depth = 0;
            }
        }

        /// <summary>
        /// Adds a child to this node.
        /// </summary>
        /// <param name="child">the child to add</param>
        /// <returns>true if the child was successfully added</returns>
        private bool AddChild(TxNode<T> child)
        {
            return Children.Add(child);
        }
    }
}
