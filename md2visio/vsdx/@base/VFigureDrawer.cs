using md2visio.struc.figure;
using md2visio.vsdx._tool;
using md2visio.vsdx.@base;
using md2visio.vsdx.tool;
using Microsoft.Office.Interop.Visio;
using System.Drawing;

namespace md2visio.vsdx.@base
{
    internal abstract class VFigureDrawer<T> :
        VShapeDrawer where T : Figure
    {
        protected T figure = Empty.Get<T>();
        protected Config config;

        public VFigureDrawer(T figure, Application visioApp) : base(visioApp)
        {
            this.figure = figure;
            config = figure.Config;
        }

        public abstract void Draw();

        public void SetFillForegnd(Shape shape, string configPath)
        {
            if (config.GetString(configPath, out string color))
            {
                SetShapeSheet(shape, "FillPattern", "1");
                SetShapeSheet(shape, "FillForegnd", $"THEMEGUARD({VColor.Create(color).RGB()})");
            }
        }

        public void SetLineColor(Shape shape, string configPath)
        {
            if(config.GetString(configPath, out string color)) {
                SetShapeSheet(shape, "LineColor", $"THEMEGUARD({VColor.Create(color).RGB()})");
            }
        }

        public void SetTextColor(Shape shape, string configPath)
        {
            if (config.GetString(configPath, out string color)) {
                shape.CellsU["Char.Color"].FormulaU = $"THEMEGUARD({VColor.Create(color).RGB()})";
            }
        }
    }
}
