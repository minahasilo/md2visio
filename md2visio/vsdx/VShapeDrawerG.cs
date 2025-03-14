using md2visio.mermaid._cmn;
using md2visio.struc.graph;
using Microsoft.Office.Interop.Visio;

namespace md2visio.vsdx
{
    internal class VShapeDrawerG : VShapeDrawer
    {
        public VShapeDrawerG(Application visioApp) : base(visioApp)
        {
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
