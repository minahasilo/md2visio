using md2visio.figure;
using md2visio.graph;
using System.Text.RegularExpressions;

namespace md2visio.figure
{
    internal abstract class Figure: Container, IEmpty
    {
        public static readonly Figure Empty = new EmptyFigure();

        public Figure()
        {
        }

        public bool IsEmpty()
        {
            return this == Empty;
        }

        public abstract void ToVisio(string path);
    }

    class EmptyFigure : Figure
    {
        public EmptyFigure() { }

        public override void ToVisio(string path)
        {
            throw new NotImplementedException();
        }
    }
}
