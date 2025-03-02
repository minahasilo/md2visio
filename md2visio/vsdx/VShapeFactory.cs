using md2visio.graph;
using md2visio.mermaid;
using Microsoft.Office.Interop.Visio;
using System.Drawing;
using System.Text.RegularExpressions;
using Drawing = System.Drawing;

namespace md2visio.vsdx
{
    class BoundaryShapes
    {
        public BoundaryShapes(Shape? front, Shape? tail) { Front = front; Tail = tail; }
        public Shape? Front { get; set; }
        public Shape? Tail { get; set; }
    }

    internal class VShapeFactory
    {
        //public static string WFOBJ_M = "WFOBJ_M.VSSX";
        //public static string BASIC_M = "BASIC_M.VSSX";
        //public static string BASFLO_M = "BASFLO_M.VSSX";
        public static string VSSX = "md2visio.vssx";

        Application visioApp;
        Page visioPage;
        public Shape EmptyShape { get; }
        Document stencilDoc;

        public VShapeFactory(Application visioApp) {
            this.visioApp = visioApp;
            visioPage = visioApp.ActivePage;
            stencilDoc = visioApp.Documents.OpenEx(VSSX, (short)VisOpenSaveArgs.visOpenDocked);            
            EmptyShape = visioPage.DrawRectangle(0, 0, 0, 0);
        }

        public GNode CreateSubgraphBorderNode(GSubgraph gSubgraph)
        {
            if (gSubgraph.Parent == null) throw new SynException("expect parent of subgraph");

            GNode node = new GNode(gSubgraph.Parent, gSubgraph.ID);

            VBoundary bnd = SubgraphBoundary(gSubgraph);
            Shape shape = CreateShape(node);
            shape.CellsU["PinX"].FormulaU = bnd.PinX.ToString();
            shape.CellsU["PinY"].FormulaU = bnd.PinY.ToString();
            shape.CellsU["Width"].FormulaU = (bnd.Width + GNode.SPACE*2).ToString();
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
            if (gSubgraph.VisioShape != null) boundary.Expand(new VBoundary(gSubgraph.VisioShape));

            foreach(GSubgraph sub in gSubgraph.Subgraphs)
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
                default:  shape.CellsU["LineWeight"].FormulaU = "=0.25 pt"; break;
            }

            // start tag
            // x/o/-/<
            switch(edge.StartTag)
            {
                case "x": shape.CellsU["BeginArrow"].FormulaU = "=24"; break;
                case "o": shape.CellsU["BeginArrow"].FormulaU = "=10"; break;
                case "-": shape.CellsU["BeginArrow"].FormulaU = "=0"; break;
                case "<": shape.CellsU["BeginArrow"].FormulaU = "=4"; break;
                default:  shape.CellsU["BeginArrow"].FormulaU = "=0"; break;
            }

            // end tag
            // x/o/-/>
            switch (edge.EndTag)
            {
                case "x": shape.CellsU["EndArrow"].FormulaU = "=24"; break;
                case "o": shape.CellsU["EndArrow"].FormulaU = "=10"; break;
                case "-": shape.CellsU["EndArrow"].FormulaU = "=0"; break;
                case ">": shape.CellsU["EndArrow"].FormulaU = "=4"; break;
                default:  shape.CellsU["EndArrow"].FormulaU = "=0"; break;
            }
        }

        public Master? GetMaster(string vssx, string name)
        {
            stencilDoc = visioApp.Documents.OpenEx(vssx, (short)VisOpenSaveArgs.visOpenDocked);            
            return FindMaster(name);
        }

        public Master? GetMaster(string name)
        {
            return FindMaster(name);   
        }

        public Master? FindMaster(string name)
        {
            foreach (Master master in stencilDoc.Masters)
            {
                if (master.Name == name)
                {
                    return master;
                }
            }
            return null;
        }

        public void AdjustSize(Shape shape)
        {
            if (string.IsNullOrEmpty(shape.Text)) return;

            string text = shape.Text;
            // 获取文本格式，包括字体大小
            double fontSize = FontSize(shape, "mm");
            string fontName = FontName(visioApp, shape);

            SizeF sizeF = MeasureTextSize(text, fontName, fontSize);

            // 调整形状大小以适应文本
            //double marginH = ShapeSheet(shape, "LeftMargin", "mm") + ShapeSheet(shape, "RightMargin", "mm");
            //double marginV = ShapeSheet(shape, "TopMargin", "mm") + ShapeSheet(shape, "BottomMargin", "mm");
            double padding = VisioUnit2MM(shape) * GNode.PADDING;
            shape.CellsU["Width"].FormulaU = $"={sizeF.Width} mm";
            shape.CellsU["Height"].FormulaU = $"={sizeF.Height + padding} mm"; 
        }
        
        public static void MoveTo(Shape shape, double pinx, double piny)
        {
            shape.CellsU["PinX"].FormulaU = pinx.ToString();
            shape.CellsU["PinY"].FormulaU = piny.ToString();
        }

        public static void MoveTo(Shape? shape, PointF pos)
        {
            if (shape == null) return;

            shape.CellsU["PinX"].FormulaU = pos.X.ToString();
            shape.CellsU["PinY"].FormulaU = pos.Y.ToString();
        }

        public static double ShapeSheetIU(Shape shape, string propName)
        {
            return shape.CellsU[propName].ResultIU; // Result Internal Unit
        }

        public static double ShapeSheetIU(GNode node, string propName)
        {
            if(node.VisioShape == null) return 0;
            return ShapeSheetIU(node.VisioShape, propName);
        }

        public static double ShapeSheet(Shape shape, string propName, string unit)
        {
            string sval = shape.CellsU[propName].ResultStr[unit];
            return Convert.ToDouble(Regex.Replace(sval, unit + "| ", ""));
        }

        public static double VisioUnit2MM(Shape shape)
        {
            double vunit = FontSize(shape);
            if (vunit == 0) return 0;

            double mm = FontSize(shape, "mm");
            return mm / vunit;
        }

        public static double MM2VisioUnit(Shape shape)
        {
            double v2mm = VisioUnit2MM(shape);
            if (v2mm != 0) return 1 / v2mm;

            return 0;
        }

        public static double FontSize(Shape shape, string unit)
        {
            string sval = shape.CellsU["Char.Size"].ResultStr[unit]; // pt, mm
            return Convert.ToDouble(Regex.Replace(sval, unit + "| ", ""));
        }

        public static double FontSize(Shape shape)
        {
            return shape.CellsU["Char.Size"].ResultIU; // visio内部单位drawing units（Result Internal Unit）
        }
        public static string FontName(Application visioApp, Shape shape)
        {
            double fontId = shape.CellsU["Char.Font"].ResultIU;
            return visioApp.ActiveDocument.Fonts[fontId].Name;
        }

        public static double Width(Shape shape)
        {
            return shape.CellsU["Width"].ResultIU;
        }

        public static double Height(Shape shape)
        {
            return shape.CellsU["Height"].ResultIU;
        }

        public static double LeftMargin(Shape shape)
        {
            return shape.CellsU["LeftMargin"].ResultIU;
        }

        public static double RightMargin(Shape shape)
        {
            return shape.CellsU["RightMargin"].ResultIU;
        }

        public static double TopMargin(Shape shape)
        {
            return shape.CellsU["TopMargin"].ResultIU;
        }

        public static double BottomMargin(Shape shape)
        {
            return shape.CellsU["BottomMargin"].ResultIU;
        }

        public static double PinX(Shape shape)
        {
            return shape.CellsU["PinX"].ResultIU;
        }

        public static double PinY(Shape shape)
        {
            return shape.CellsU["PinY"].ResultIU;
        }

        public static SizeF MeasureTextSize(string text, string fontName, double fontSizeMM)
        {
            // 获取当前屏幕的DPI
#pragma warning disable CA1416
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                float dpiX = graphics.DpiX;
                float dpiY = graphics.DpiY;

                // 将字体大小从mm转换为像素
                float fontSizePixel = (float)(fontSizeMM * dpiX / 25.4);

                // 创建Font对象
                using (Drawing.Font font = new Drawing.Font(fontName, fontSizePixel))
                {
                    // 测量字符串尺寸
                    SizeF textSizePixels = graphics.MeasureString(text, font);

                    // 将尺寸从像素转换回mm
                    float widthMM = textSizePixels.Width * 25.4f / dpiX;
                    float heightMM = textSizePixels.Height * 25.4f / dpiY;

                    return new SizeF(widthMM, heightMM);
                }
            }
        }

    }   
    
}
