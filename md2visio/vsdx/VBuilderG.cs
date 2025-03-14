using md2visio.struc.figure;
using md2visio.struc.graph;
using Microsoft.Office.Interop.Visio;
using System.Drawing;

namespace md2visio.vsdx
{
    internal enum RelativePos
    {
        FRONT, TAIL, BESIDE
    }

    internal class VBuilderG : VBuilder
    {
        Graph graph;
        LinkedList<GNode> alignList = new LinkedList<GNode>();        
        VShapeDrawerG shapeDrawer;

        public VBuilderG(Graph figure) : base(figure)
        {
            graph = figure;            
            shapeDrawer = new VShapeDrawerG(VisioApp);
        }

        override public void Build(string outputFile) 
        {
            DrawNodes(graph);
            DrawEdges(graph);            
            SaveAndClose(outputFile);
        }

        void DrawEdges(Graph graph)
        {
            List<GEdge> drawnEdges = new List<GEdge>();
            foreach(INode node in graph.NodeDict.Values)
            {
                if (node.VisioShape == null) continue;
                foreach(GEdge edge in node.OutputEdges)
                {
                    if(drawnEdges.Contains(edge) || edge.To.VisioShape == null) continue;

                    Shape shape = shapeDrawer.CreateEdge(edge);
                    node.VisioShape.AutoConnect(edge.To.VisioShape, VisAutoConnectDir.visAutoConnectDirNone, shape);
                    shape.Delete();
                    drawnEdges.Add(edge);
                }
            }
        }

        void DrawNodes(Graph graph)
        {
            foreach (GSubgraph subGraph in graph.Subgraphs)
            {
                DrawNodes(subGraph);
                DrawSubgraphBorder(subGraph);
            }

            LinkedList<GNode> drawnNodes = new LinkedList<GNode>();
            (GNode? linkedNode, RelativePos rpos) = graph.NodeLinkedToSubgraph(drawnNodes);
            while (linkedNode != null)
            {
                DrawNode(linkedNode, rpos);
                (linkedNode, rpos) = graph.NodeLinkedToSubgraph(drawnNodes);
            }

            foreach (GNode node in graph.AlignInnerNodes())
            {
                DrawNode(node, RelativePos.TAIL);    
            }

        }

        void DrawSubgraphBorder(GSubgraph subGraph)
        {
            GNode borderNode = shapeDrawer.CreateSubgraphBorderNode(subGraph);
            alignList.AddLast(borderNode);
            alignList.AddFirst(borderNode);
        }

        void DrawNode(GNode node, RelativePos rpos)
        {
            if (alignList.Contains(node)) return;

            Shape vshape = shapeDrawer.CreateShape(node);
            PointF pos = NextDrawPos(node, rpos);
            VShapeDrawer.MoveTo(vshape, pos);

            if(rpos == RelativePos.TAIL) alignList.AddLast(node);
            else alignList.AddFirst(node);
        }

        PointF NextDrawPos(GNode node, RelativePos rpos)
        {
            PointF pos = PointF.Empty;
            if (alignList.Count == 0) return pos;

            Shape? shape = node.VisioShape;
            if(shape == null) return pos;

            VBoundary boundary = VBoundary.Create(alignList);
            double w = boundary.Width, h = boundary.Height,
                x = boundary.PinX, y = boundary.PinY;
            int reverse = (rpos == RelativePos.FRONT ? -1 : 1);
            {
                double sw = VShapeDrawer.ShapeSheetIU(shape, "Width");
                double sh = VShapeDrawer.ShapeSheetIU(shape, "Height");
                GGrowthDirection grow = node.Container.GrowDirect;
                if (grow.H == 0)
                {
                    pos.X = (float)x; // 水平居中对齐
                    pos.Y = (float)(y + grow.V * reverse * (h / 2 + sh / 2 + GNode.SPACE));
                }
                else if (grow.V == 0)
                {
                    pos.Y = (float)y; // 垂直居中对齐
                    pos.X = (float)(x + grow.H * reverse * (w / 2 + sw / 2 + GNode.SPACE));
                }
            }

            return pos;
        }


    }
}
