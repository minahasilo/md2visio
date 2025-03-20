using md2visio.struc.figure;
using md2visio.vsdx.@base;
using Microsoft.Office.Interop.Visio;

namespace md2visio.vsdx.@base
{
    internal abstract class VFigureDrawer<T> :
        VShapeDrawer where T : Figure
    {
        protected T figure = Empty.Get<T>();

        public VFigureDrawer(T figure, Application visioApp) : base(visioApp)
        {
            this.figure = figure;
        }

        public abstract void Draw();
    }
}
