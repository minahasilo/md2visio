using md2visio.figure;
using md2visio.mermaid;
using Microsoft.Office.Interop.Visio;
using System.Text;

namespace md2visio.graph
{
    internal class GSubgraph: Graph, INode
    {
        public static double NODE_SPACE = 0.1;

        string id = string.Empty;
        GLabel label = new GLabel(string.Empty);

        public string ID { 
            get { return id; } 
            set { 
                id = value;
                if(label.Content.Length == 0) Label = id;
            } 
        }
        public string Label {
            get { return label.ToString(); }
            set { label.Content = value; }
        }
        public Container Container { get { return Parent ?? Empty; } set { Parent = value; } }

        public GSubgraph(Graph parent, string id)
        {
            Parent = parent;
            ID = id;
        }
        public GSubgraph(Graph parent) : this(parent, string.Empty)
        {

        }
        public Shape? VisioShape { get; set; }

        public List<GEdge> InputEdges { get; set; } = new List<GEdge>();

        public List<GEdge> OutputEdges { get; set; } = new List<GEdge>();

        public override void SetParam(PartValueList valueList)
        {
            StringBuilder t = new StringBuilder();
            foreach (var item in valueList.Values())
            {
                if(item.IsPaired()) Label = item.Value;
                else if(item.IsText()) t.Append(item.Value);
                else if(item.IsQuoted()) t.Append(item.Value);
            }
            if (t.Length == 0) throw new SynException("expect subgraph ID");
            ID = t.ToString();
        }
    }
}
