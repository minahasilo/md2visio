using md2visio.figure;
using md2visio.journey;

namespace md2visio.vsdx
{
    internal class VBuilderJo : VBuilder
    {
        VShapeDrawerJo shapeDrawer;

        public VBuilderJo(Figure figure) : base(figure)
        {
            shapeDrawer = new VShapeDrawerJo(visioApp);
        }

        public override void Build(string outputFile)
        {
            Journey jo = figure.DownCast<Journey>();
            shapeDrawer.DrawJounery(jo);
            visioPage.ResizeToFitContents();

            visioDoc.SaveAsEx(outputFile, 0);
            visioDoc.Close();
            visioApp.Quit();
        }
    }
}
