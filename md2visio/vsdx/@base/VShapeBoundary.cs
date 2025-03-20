using md2visio.struc.figure;
using md2visio.vsdx.@base;
using Microsoft.Office.Interop.Visio;

namespace md2visio.vsdx.@base
{
    internal class VShapeBoundary : VBoundary
    {
        Shape shape = Empty.Get<EmptyShape>();

        public Shape Shape { get => shape; }

        public VShapeBoundary() { }

        public VShapeBoundary(Shape shape) : base(VShapeDrawer.PinX(shape),
            VShapeDrawer.PinY(shape),
            VShapeDrawer.Width(shape),
            VShapeDrawer.Height(shape))
        {
            this.shape = shape;
        }
    }
}
