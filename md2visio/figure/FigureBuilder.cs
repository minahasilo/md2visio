using md2visio.mermaid.cmn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md2visio.figure
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
