using md2visio.struc.figure;
using md2visio.struc.journey;

namespace md2visio.vsdx
{
    internal class VBuilderJo : VBuilder
    {
        VShapeDrawerJo shapeDrawer;

        public VBuilderJo(Figure figure) : base(figure)
        {
            shapeDrawer = new VShapeDrawerJo(VisioApp);
        }

        public override void Build(string outputFile)
        {
            Journey jo = figure.DownCast<Journey>();
            shapeDrawer.DrawJounery(jo);
            
            SaveAndClose(outputFile);
        }
    }
}
