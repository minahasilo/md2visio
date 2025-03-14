using md2visio.mermaid._cmn;

namespace md2visio.struc.figure
{
    internal abstract class FigureBuilder
    {
        protected SttIterator iter;

        public FigureBuilder(SttIterator iter)
        {
            this.iter = iter;
        }

        abstract public void Build(string outputFile);
    }
}
