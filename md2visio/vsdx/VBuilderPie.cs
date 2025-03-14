using md2visio.struc.figure;
using md2visio.struc.pie;

namespace md2visio.vsdx
{
    internal class VBuilderPie : VBuilder
    {
        VShapeDrawerPie drawer;

        public VBuilderPie(Figure figure) : base(figure)
        {
            drawer = new VShapeDrawerPie(VisioApp);
        }

        public override void Build(string outputFile)
        {
            drawer.Draw(figure.DownCast<Pie>());
            SaveAndClose(outputFile);
        }
    }
}
