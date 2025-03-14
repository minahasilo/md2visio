using md2visio.struc.graph;
using Microsoft.Office.Interop.Visio;
using System.Drawing;
using System.Text.RegularExpressions;
using Drawing = System.Drawing;

namespace md2visio.vsdx
{
    internal class VShapeDrawer
    {
        public static string VSSX = "md2visio.vssx";
        public Shape EmptyShape { get; }
        protected Application visioApp;
        protected Page visioPage;        
        protected Document stencilDoc;

        public VShapeDrawer(Application visioApp) {
            this.visioApp = visioApp;
            visioPage = visioApp.ActivePage;
            stencilDoc = visioApp.Documents.OpenEx(VSSX, (short)VisOpenSaveArgs.visOpenDocked);            
            EmptyShape = visioPage.DrawRectangle(0, 0, 0, 0);
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

        public Shape DropText(string text, double pinx=0, double piny=0)
        {
            Shape shape = visioPage.Drop(GetMaster("[]"), pinx, piny);
            shape.CellsU["LinePattern"].FormulaU = "=0";
            shape.CellsU["FillPattern"].FormulaU = "=0";
            shape.Text = text;
            AdjustSize(shape);

            return shape;
        }

        public void Rotate(Shape shape, double rad, bool clockwise = true)
        {
            rad = rad % (2 * double.Pi);
            if (rad < double.Pi) rad = (clockwise ? -1 : 1) * rad;
            else if (rad > double.Pi) rad = (clockwise ? 1 : -1) * (2 * double.Pi - rad);

            SetShapeSheet(shape, "Angle", $"{rad} rad");
        }

        public void AdjustSize(Shape shape)
        {
            if (string.IsNullOrEmpty(shape.Text)) return;

            string text = shape.Text;
            double fontSize = FontSize(shape, "mm");
            string fontName = FontName(visioApp, shape);

            SizeF sizeF = MeasureTextSizeMM(text, fontName, fontSize);

            // 调整形状大小以适应文本
            //double marginH = ShapeSheet(shape, "LeftMargin", "mm") + ShapeSheet(shape, "RightMargin", "mm");
            //double marginV = ShapeSheet(shape, "TopMargin", "mm") + ShapeSheet(shape, "BottomMargin", "mm");
            double padding = VisioUnit2MM(shape) * GNode.PADDING;
            shape.CellsU["Width"].FormulaU = $"={sizeF.Width} mm";
            shape.CellsU["Height"].FormulaU = $"={sizeF.Height + padding} mm"; 
        }
        
        public static void MoveTo(Shape shape, double pinx, double piny)
        {
            MoveTo(shape, pinx.ToString(), piny.ToString());
        }

        public static void MoveTo(Shape shape, string pinx, string piny)
        {
            shape.CellsU["PinX"].FormulaU = pinx;
            shape.CellsU["PinY"].FormulaU = piny;
        }

        public static void MoveTo(Shape? shape, PointF pos)
        {
            if (shape == null) return;

            shape.CellsU["PinX"].FormulaU = pos.X.ToString();
            shape.CellsU["PinY"].FormulaU = pos.Y.ToString();
        }

        public static void SetShapeSheet(Shape shape, string propName, string FormulaU)
        {
            shape.CellsU[propName].FormulaU = FormulaU;
        }

        public static double ShapeSheetIU(Shape shape, string propName)
        {
            return shape.CellsU[propName].ResultIU; // Result Internal Unit
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

        public double MM2Pix(Shape shape)
        {
            double fontSizeMM = FontSize(shape, "mm");
            SizeF size = MeasureTextSizePix(shape.Text, FontName(visioApp, shape), fontSizeMM);
            if (fontSizeMM != 0) return size.Height / fontSizeMM;

            return 0;
        }

        public double Pix2MM(Shape shape)
        {
            double mm2pix = MM2Pix(shape);
            if (mm2pix != 0) return 1 / mm2pix;

            return 0;
        }

        public double Pt2MM(Shape shape)
        {
            double pt = FontSize(shape, "pt");
            double mm = FontSize(shape, "mm");            
            if(pt != 0) return mm / pt;

            return 0;
        }

        public double MM2Pt(Shape shape)
        {
            double pt2mm = Pt2MM(shape);
            if( pt2mm != 0) return 1/ pt2mm;

            return 0;
        }

        public (bool success, string unit, double unitVal) UnitValue(string? unitStr)
        {
            bool success = false;
            string uname = string.Empty;
            double result = 0;

            if (unitStr != null)
            {
                unitStr = unitStr.Trim().ToLower();
                if (unitStr.EndsWith("pt")) uname = "pt";
                else if (unitStr.EndsWith("px")) uname = "px";
                else if (unitStr.EndsWith("mm")) uname = "mm";

                if (uname.Length > 0) unitStr = unitStr.Substring(0, unitStr.Length-uname.Length);
                success = double.TryParse(unitStr, out result);
            }

            return (success, uname, result);
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

        public static void SetWidth(Shape shape, string width)
        {
            SetShapeSheet(shape, "Width", width);
        }

        public static double Height(Shape shape)
        {
            return shape.CellsU["Height"].ResultIU;
        }

        public static void SetHeight(Shape shape, string height)
        {
            SetShapeSheet(shape, "Height", height);
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

        public static void AlignRight(Shape shape, double right)
        {
            double pinx = right - ShapeSheetIU(shape, "Width") / 2;
            MoveTo(shape, pinx, ShapeSheetIU(shape, "PinY"));
        }

        public static void AlignBottom(Shape shape, double bottom)
        {
            double piny = bottom + ShapeSheetIU(shape, "Height") / 2;
            MoveTo(shape, ShapeSheetIU(shape, "PinX"), piny);
        }

        public static void AlignRightBottom(Shape shape, double right, double bottom)
        {
            AlignRight(shape, right);
            AlignBottom(shape, bottom);
        }

        public static void AlignLeft(Shape shape, double left)
        {
            double pinx = left + ShapeSheetIU(shape, "Width") / 2;
            MoveTo(shape, pinx, ShapeSheetIU(shape, "PinY"));
        }

        public static void AlignTop(Shape shape, double top)
        {
            double piny = top - ShapeSheetIU(shape, "Height") / 2;
            MoveTo(shape, ShapeSheetIU(shape, "PinX"), piny);
        }

        public static void AlignLeftTop(Shape shape, double left, double top)
        {
            AlignLeft(shape, left);
            AlignTop(shape, top);
        }

        public static void SetFillForegnd(Shape shape, Tuple<int,int,int> color)
        {
            SetShapeSheet(shape, "FillPattern", "1");
            SetShapeSheet(shape, "FillForegnd", $"THEMEGUARD(RGB({color.Item1},{color.Item2},{color.Item3}))");
        }

        public static void SetRounding(Shape shape, string rounding)
        {
            SetShapeSheet(shape, "Rounding", rounding);
        }

        public static SizeF MeasureTextSizeMM(string text, string fontName, double fontSizeMM)
        {
#pragma warning disable CA1416
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                SizeF textSizePixels = MeasureTextSizePix(text, fontName, fontSizeMM);
                float widthMM = textSizePixels.Width * 25.4f / graphics.DpiX;
                float heightMM = textSizePixels.Height * 25.4f / graphics.DpiY;

                return new SizeF(widthMM, heightMM);
            }
        }

        public static SizeF MeasureTextSizePix(string text, string fontName, double fontSizeMM)
        {
            // 获取当前屏幕的DPI
#pragma warning disable CA1416
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                // 将字体大小从mm转换为像素
                float fontSizePixel = (float)(fontSizeMM * graphics.DpiX / 25.4);

                // 创建Font对象
                using (Drawing.Font font = new Drawing.Font(fontName, fontSizePixel))
                {
                    // 测量字符串尺寸
                    SizeF textSizePixels = graphics.MeasureString(string.IsNullOrEmpty(text)?" ":text, font);
                    return textSizePixels;
                }
            }
        }

    }   


    
}
