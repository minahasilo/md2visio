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
        double r = 1.02;
        public VShapeDrawerPie(Application visioApp) : base(visioApp)
        {
        }

        public void Draw(Pie pie)
        {
            List<Tuple<int, int, int>> colors = ColorGenerator.Generate(pie.InnerNodes.Count);
            total = pie.TotalNum();
            int ci = 0;
            foreach(INode item in pie.InnerNodes.Values)
            {
                Shape sector = DropSector(((PieDataItem)item), colors[ci++]);
                r = Width(sector);
            }

            Shape title = DropText(pie.Title);
            AlignBottom(title, r + 0.05);
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

            // textPosition = 0.8
            (x, y) = ControlPoint(Width(sector) * 0.8, drawnAngle + rad / 2);
            DropText($"{(percent * 100).ToString("N0")}%", x, y);

            drawnAngle += rad;

            // tag
            if(tagBound.IsEmpty())
            {
                tagBound = new VBoundary();
                tagBound.AlignLeftTop(r + 0.5, r);
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
