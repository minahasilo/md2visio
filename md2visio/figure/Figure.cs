using md2visio.graph;
using System.Text.RegularExpressions;

namespace md2visio.figure
{
    internal class Figure: Container
    {
        public static Figure Empty = new Figure();

        public Figure()
        {
        }

        public virtual void ToVisio(string path)
        {
        }
    }
}
