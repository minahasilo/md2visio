using md2visio.struc.figure;
using md2visio.struc.pie;
using Microsoft.Office.Interop.Visio;
using System.Drawing;

namespace md2visio.vsdx
{
    internal class VShapeDrawerPie : VShapeDrawer
    {
        double drawnAngle = 0;
        VBoundary tagBound = VBoundary.Empty;
        double total = 0;
        double radius = 1.02;
        public VShapeDrawerPie(Application visioApp) : base(visioApp)
        {
        }

        public void Draw(Pie pie)
        {
            // sector
            List<Tuple<int, int, int>> colors = ColorGenerator.Generate(pie.InnerNodes.Count);
            total = pie.TotalNum();
            int ci = 0;
            foreach(INode item in pie.InnerNodes.Values)
            {
                Shape sector = DropSector(((PieDataItem)item), colors[ci++]);
                radius = Width(sector);
            }

            // title
            Shape title = DropText(pie.Title, new SizeF(2, 2));
            AlignBottom(title, radius + 0.05);

            // outer stroke
            DrawOuterStroke(pie);
        }

        public Shape DropSector(PieDataItem item, Tuple<int,int,int> color)
        {
            double percent = item.Data / total;
            Shape sector = visioPage.Drop(GetMaster("sector"), 0, 0);

            // sector
            double rad = percent * 2 * double.Pi;
            (double x, double y) = ControlPoint(Width(sector), rad);
            SetShapeSheet(sector, "Controls.X", x.ToString());
            SetShapeSheet(sector, "Controls.Y", y.ToString());
            Rotate(sector, drawnAngle, false);
            SetFillForegnd(sector, color);

            // textPosition
            double textPosition = 0.75;
            Pie pie = item.Container.DownCast<Pie>();
            (bool success, double tpos) = pie.Setting.GetDouble("init.pie.textPosition");
            if (success) textPosition = tpos;
            (x, y) = ControlPoint(Width(sector) * textPosition, drawnAngle + rad / 2);
            DropText($"{(percent * 100).ToString("N0")}%", x, y);

            drawnAngle += rad;

            // tag
            if(tagBound.IsEmpty())
            {
                tagBound = new VBoundary();
                tagBound.AlignLeftTop(radius + 0.5, radius);
            }            
            DrawTag(color, item.ID, item.Data);

            return sector;
        }

        void DrawTag(Tuple<int,int,int> color, string title, double data)
        {
            Shape tag = visioPage.Drop(GetMaster("[]"), 0, 0);
            SetFillForegnd(tag, color);
            SetWidth(tag, "3 mm");
            SetHeight(tag, "3 mm");            
            AlignLeftTop(tag, tagBound.Left, tagBound.Top);
            tagBound.Right = tagBound.Left + Width(tag);
            tagBound.Bottom = tagBound.Top - Height(tag);

            Shape text = DropText($"{title} [{data.ToString("N2")}]", 0, PinY(tag));
            visioApp.DoCmd((short)VisUICmds.visCmdTextHAlignLeft);
            AlignLeft(text, new VShapeBoundary(tag).Right + 0.01);

            tagBound.AlignTop(tagBound.Bottom - 0.02);
        }

        void DrawOuterStroke(Pie pie)
        {
            Shape shape = visioPage.Drop(GetMaster("(())"), 0, 0);
            SetShapeSheet(shape, "FillPattern", "0");

            double outerStrokeWidth = 0.2646; // 0.2646 mm = 0.75 pt
            string? value = pie.Setting.GetString("init.themeVariables.pieOuterStrokeWidth");
            (bool success, string uname, double unitVal) = UnitValue(value);
            if (success)
            {
                if (uname.EndsWith("pt")) outerStrokeWidth = Pt2MM() * unitVal;
                else if (uname.EndsWith("px")) outerStrokeWidth = Pix2MM() * unitVal;
                else if (uname.EndsWith("mm")) outerStrokeWidth = unitVal;
            }

            string size = $"{VisioUnit2MM(shape)*radius*2 + outerStrokeWidth} mm";
            SetWidth(shape, size);
            SetHeight(shape, size);
            SetShapeSheet(shape, "LineWeight", $"{outerStrokeWidth} mm");            
        }

        (double x, double y) ControlPoint(double r, double rad)
        {
            const double pi = double.Pi;            
            double x, y;
            if (rad < pi / 2)
            {
                x = r * Math.Cos(rad);
                y = r * Math.Sin(rad);
            }
            else if (rad < pi)
            {
                x = -r * Math.Cos(pi - rad);
                y = r * Math.Sin(pi - rad);
            }
            else if (rad < pi / 2 * 3)
            {
                x = -r * Math.Cos(rad - pi);
                y = -r * Math.Sin(rad - pi);
            }
            else
            {
                x = r * Math.Cos(2 * pi - rad);
                y = -r * Math.Sin(2 * pi - rad);
            }
            return (x, y);
        }
    }
}
