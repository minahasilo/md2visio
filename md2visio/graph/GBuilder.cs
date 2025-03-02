using md2visio.figure;
using md2visio.mermaid;

namespace md2visio.graph
{
    internal class GBuilder
    {
        static List<GNode> EmptyList = new List<GNode>();

        SynContext ctx;
        Stack<Graph> stack = new Stack<Graph>();
        SttIterator iter;
        int count = 1;

        List<GNode> fromNodes = EmptyList, toNodes = EmptyList;
        GEdge edge = GEdge.Empty;

        public GBuilder(SynContext ctx) { 
            this.ctx = ctx;
            iter = ctx.SttIterator();
        }

        public void Build(string outputFile)
        {
            while (iter.HasNext())
            {
                SynState cur = iter.Next();
                if (cur is SttMermaidStart) stack.Clear();
                else if (cur is SttMermaidClose) Output(outputFile);
                else if (cur is SttKeyword) BuildKeyword();
                else if (cur is SttText)
                {
                    List<GNode> nodes = GatherNodes();
                    if (fromNodes.Count == 0) fromNodes = nodes;
                    else if (edge.IsEmpty()) fromNodes = nodes;
                    else
                    {
                        toNodes = nodes;
                        ConnectNodes();
                    }
                }
                else if(cur is SttLinkStart) BuildEdge();
                else if(cur is SttNoLabelLink) BuildEdge();
                else if (cur is SttComment) { }
                else if (cur is SttConfig) { }
                else if (cur is SttFinishFlag) { }
            }
        }

        void ConnectNodes()
        {
            if(edge.IsEmpty()) return;

            foreach (GNode from in fromNodes)
            {
                foreach(GNode to in toNodes)
                {
                    edge.Clone().Connect(from, to);
                }
            }

            edge = GEdge.Empty;
            fromNodes = toNodes;
            toNodes = EmptyList;
        }

        List<GNode> GatherNodes(List<GNode>? nodes = null)
        {
            SynState cur = iter.Current;
            if (cur is not SttText) throw new SynException("expect graph node", ctx);

            if (nodes == null) nodes = new List<GNode>();

            Graph graph = SuperContainer();
            GNode node = graph.RetrieveNode<GNode>(cur.Fragment);
            nodes.Add(node);
            graph.AddInnerNode(node);

            // shape
            SynState next = iter.PeekNext();
            if (next is SttExtendShape)
            {
                iter.Next();
                node.NodeShape = GNodeShape.CreateExtend(next.Fragment);
            }
            else if (next is SttPaired)
            {
                iter.Next();
                string start = next.GetPart("start"),
                    mid = next.GetPart("mid"),
                    close = next.GetPart("close");
                node.NodeShape = GNodeShape.CreatePaired(start, mid, close);
            }
            // &
            else if (next is SttAmp)
            {
                iter.Next();
                iter.Next();
                GatherNodes(nodes);
            }

            return nodes;
        }

        GEdge BuildEdge()
        {
            SynState state = iter.Current;
            edge = GEdge.Empty;
            if (state is SttLinkStart)
            {
                edge = new GEdge();
                edge.StartTag = state.Fragment;

                if (iter.Next() is not SttLinkLabel) throw new SynException("expect link label", ctx);
                edge.Text = iter.Current.Fragment;

                if (iter.Next() is not SttLinkEnd) throw new SynException("expect link end", ctx);
                edge.EndTag = iter.Current.Fragment;
            }
            else if (state is SttNoLabelLink)
            {
                edge = new GEdge();
                edge.StartTag = state.Fragment;
                edge.EndTag = state.Fragment;

                if (iter.PeekNext() is SttPipedLinkText)
                {
                    edge.Text = iter.Next().Fragment;
                }
            }

            return edge;
        }

        void BuildKeyword()
        {
            SynState sttNext = iter.PeekNext();            
            string frag = iter.Current.Fragment;

            if (frag == "graph" || frag == "flowchart")
            {
                Graph graph = new Graph();
                graph.SetParam(iter.Next().PartList);
                stack.Push(graph);
            }
            else if (frag == "subgraph")
            {
                Graph graph = SuperContainer();
                GSubgraph subgraph = new GSubgraph(graph);
                subgraph.SetParam(iter.Next().PartList);

                // add to parent graph
                graph.AddSub(subgraph);
                stack.Push(subgraph);
            }
            else if (frag == "end")
            {
                if (stack.Count == 0) throw new SynException("expect 'graph', 'flowchart' or 'subgraph'", ctx);
                stack.Pop();
            }
            else if (frag == "direction") {
                if (sttNext is not SttKeywordParam) throw new SynException("expect keyword param", ctx);

                stack.First().Direction = iter.Next().Fragment;
            }
            // TODO
            else if (frag == "click") { }   
            else if (frag == "style") { }
            else if (frag == "linkStyle") { }
            else if (frag == "class") { }
            else if (frag == "classDef") { }
        }

        Graph SuperContainer()
        {
            if (stack.Count == 0) throw new SynException("expect a graph or subgraph", ctx);

            return stack.First();
        }

        void Output(string outputFile)
        {
            if (stack.Count == 0) return;

            string name = Path.GetFileNameWithoutExtension(outputFile);
            string ext = Path.GetExtension(outputFile);
            string? dir = Path.GetDirectoryName(outputFile);

            Container nc = stack.First();
            if (nc is Figure) nc.DownCast<Figure>().ToVisio($"{dir}\\{name}{count++}{ext}");
            stack.Clear();
        }


    }
}
 