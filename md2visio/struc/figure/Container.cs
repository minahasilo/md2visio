using md2visio.struc.graph;
using System.Text.RegularExpressions;

namespace md2visio.struc.figure
{
    internal class Container
    {
        string direction = "LR";
        GGrowthDirection shift;
        Container? parent = null;
        public Container? Parent
        {
            get => parent;
            set
            {
                if (value != null) nodeDict = value.nodeDict;
                parent = value;
            }
        }

        protected Dictionary<string, INode> nodeDict = new Dictionary<string, INode>();
        protected Dictionary<string, INode> innerNodes = new Dictionary<string, INode>();

        public string Direction
        {
            get { return direction; }
            set
            {
                if (IsDirectionFragment(value))
                {
                    direction = value.Trim();
                    shift.Decide(this);
                }
            }
        }
        public Dictionary<string, INode> InnerNodes { get { return innerNodes; } }

        public Dictionary<string, INode> NodeDict { get { return nodeDict; } }

        public Container()
        {
            shift = new GGrowthDirection(this);
        }
        public GGrowthDirection GrowDirect { get { return shift; } }

        public virtual T RetrieveNode<T>(string id) where T : INode, new()
        {
            if (nodeDict.ContainsKey(id)) return (T)nodeDict[id];

            T node = new T();
            node.Container = this;
            node.ID = id;

            AddNodeToDict(node);
            return node;
        }

        public void AddInnerNode(INode node)
        {
            if (!innerNodes.ContainsKey(node.ID))
                innerNodes.Add(node.ID, node);
            node.Container = this;
            AddNodeToDict(node);
        }

        protected void AddNodeToDict(INode node)
        {
            if (!nodeDict.ContainsKey(node.ID))
                nodeDict.Add(node.ID, node);
        }

        public T DownCast<T>() where T : Container
        {
            if (this is T) return (T)this;

            throw new InvalidCastException();
        }

        static public bool IsDirectionFragment(string fragment)
        {
            return Regex.IsMatch(fragment.Trim(), "^(LR|RL|TB|TD|BT)$");
        }
    }
}
