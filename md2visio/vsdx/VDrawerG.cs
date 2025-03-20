using md2visio.mermaid.cmn;
using md2visio.struc.figure;
using md2visio.struc.graph;
using md2visio.vsdx.@base;
using Microsoft.Office.Interop.Visio;
using System.Drawing;

namespace md2visio.vsdx
{
    internal enum RelativePos
    {
        FRONT, TAIL, BESIDE
    }

    internal class VDrawerG(Graph graph, Application visioApp) : 
        VFigureDrawer<Graph>(graph, visioApp)
    {
        LinkedList<GNode> alignList = new LinkedList<GNode>();

        public override void Draw()
        {
            DrawNodes(figure);
            DrawEdges(figure);
        }

        void DrawEdges(Graph figure)
        {
            List<GEdge> drawnEdges = new List<GEdge>();
            foreach (INode node in figure.NodeDict.Values)
            {
                if (node.VisioShape == null) continue;
                foreach (GEdge edge in node.OutputEdges)
                {
                    if (drawnEdges.Contains(edge) || edge.To.VisioShape == null) continue;

                    Shape shape = CreateEdge(edge);
                    node.VisioShape.AutoConnect(edge.To.VisioShape, VisAutoConnectDir.visAutoConnectDirNone, shape);
                    shape.Delete();
                    drawnEdges.Add(edge);
                }
            }
        }

        void DrawNodes(Graph figure)
        {
            foreach (GSubgraph subGraph in figure.Subgraphs)
            {
                DrawNodes(subGraph);
                DrawSubgraphBorder(subGraph);
            }

            LinkedList<GNode> drawnNodes = new LinkedList<GNode>();
            (GNode? linkedNode, RelativePos rpos) = figure.NodeLinkedToSubgraph(drawnNodes);
            while (linkedNode != null)
            {
                DrawNode(linkedNode, rpos);
                (linkedNode, rpos) = figure.NodeLinkedToSubgraph(drawnNodes);
            }

            foreach (GNode node in figure.AlignInnerNodes())
            {
                DrawNode(node, RelativePos.TAIL);
            }

        }

        void DrawSubgraphBorder(GSubgraph subGraph)
        {
            GNode borderNode = CreateSubgraphBorderNode(subGraph);
            alignList.AddLast(borderNode);
            alignList.AddFirst(borderNode);
        }

        void DrawNode(GNode node, RelativePos rpos)
        {
            if (alignList.Contains(node)) return;

            Shape vshape = CreateShape(node);
            PointF pos = NextDrawPos(node, rpos);
            MoveTo(vshape, pos);

            if (rpos == RelativePos.TAIL) alignList.AddLast(node);
            else alignList.AddFirst(node);
        }

        PointF NextDrawPos(GNode node, RelativePos rpos)
        {
            PointF pos = PointF.Empty;
            if (alignList.Count == 0) return pos;

            Shape? shape = node.VisioShape;
            if (shape == null) return pos;

            VBoundary boundary = VBoundary.Create(alignList);
            double w = boundary.Width, h = boundary.Height,
                x = boundary.PinX, y = boundary.PinY;
            int reverse = (rpos == RelativePos.FRONT ? -1 : 1);
            {
                double sw = ShapeSheetIU(shape, "Width");
                double sh = ShapeSheetIU(shape, "Height");
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

        public static double ShapeSheetIU(GNode node, string propName)
        {
            if (node.VisioShape == null) return 0;
            return ShapeSheetIU(node.VisioShape, propName);
        }

        public GNode CreateSubgraphBorderNode(GSubgraph gSubgraph)
        {
            if (gSubgraph.Parent == null) throw new SynException("expected parent of subgraph");

            GNode node = new GNode(gSubgraph.Parent, gSubgraph.ID);

            VBoundary bnd = SubgraphBoundary(gSubgraph);
            Shape shape = CreateShape(node);
            shape.CellsU["PinX"].FormulaU = bnd.PinX.ToString();
            shape.CellsU["PinY"].FormulaU = bnd.PinY.ToString();
            shape.CellsU["Width"].FormulaU = (bnd.Width + GNode.SPACE * 2).ToString();
            shape.CellsU["Height"].FormulaU = (bnd.Height + GNode.SPACE * 2).ToString();
            shape.CellsU["FillPattern"].FormulaU = "0";
            shape.CellsU["VerticalAlign"].FormulaU = "0";
            shape.Text = gSubgraph.Label;
            gSubgraph.VisioShape = shape;

            return node;
        }

        VBoundary SubgraphBoundary(GSubgraph gSubgraph)
        {
            VBoundary boundary = VBoundary.Create(gSubgraph.AlignInnerNodes());
            if (gSubgraph.VisioShape != null) boundary.Expand(new VShapeBoundary(gSubgraph.VisioShape));

            foreach (GSubgraph sub in gSubgraph.Subgraphs)
            {
                VBoundary subBoundary = SubgraphBoundary(sub);
                boundary.Expand(subBoundary);
            }

            return boundary;
        }

        public Shape CreateShape(GNode node)
        {
            Shape shape = EmptyShape;
            string start = node.ShapeStart, close = node.ShapeClose;
            Master? master = GetMaster($"{start}{close}");

            if (master != null)
            {
                shape = visioPage.Drop(master, 0, 0);
                shape.Text = node.Label;
                AdjustSize(shape);
                node.VisioShape = shape;
            }

            return shape;
        }

        public Shape CreateEdge(GEdge edge)
        {
            Master? master = GetMaster("-");
            Shape shape = visioPage.Drop(master, 0, 0);
            SetupEdgeShape(edge, shape);

            return shape;
        }

        void SetupEdgeShape(GEdge edge, Shape shape)
        {
            shape.Text = edge.Text;
            // line type
            switch (edge.LineType)
            {
                case "-": shape.CellsU["LineWeight"].FormulaU = "=0.25 pt"; break;
                case "=": shape.CellsU["LineWeight"].FormulaU = "=0.75 pt"; break;
                case ".": shape.CellsU["LinePattern"].FormulaU = "=2"; break;
                case "~": shape.CellsU["LinePattern"].FormulaU = "=0"; break;
                default: shape.CellsU["LineWeight"].FormulaU = "=0.25 pt"; break;
            }

            // start tag
            // x/o/-/<
            switch (edge.StartTag)
            {
                case "x": shape.CellsU["BeginArrow"].FormulaU = "=24"; break;
                case "o": shape.CellsU["BeginArrow"].FormulaU = "=10"; break;
                case "-": shape.CellsU["BeginArrow"].FormulaU = "=0"; break;
                case "<": shape.CellsU["BeginArrow"].FormulaU = "=4"; break;
                default: shape.CellsU["BeginArrow"].FormulaU = "=0"; break;
            }

            // end tag
            // x/o/-/>
            switch (edge.EndTag)
            {
                case "x": shape.CellsU["EndArrow"].FormulaU = "=24"; break;
                case "o": shape.CellsU["EndArrow"].FormulaU = "=10"; break;
                case "-": shape.CellsU["EndArrow"].FormulaU = "=0"; break;
                case ">": shape.CellsU["EndArrow"].FormulaU = "=4"; break;
                default: shape.CellsU["EndArrow"].FormulaU = "=0"; break;
            }
        }


    }
}
