using md2visio.mermaid.cmn;
using md2visio.mermaid.graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md2visio.mermaid.graph.@internal
{
    internal class GSttTilde : SynState
    {
        public override SynState NextState()
        {
            if (GSttNoLabelLink.IsEmptyLink(Ctx)) return Forward<GSttNoLabelLink>();

            throw new SynException("unexpected '~'", Ctx);
        }
    }
}
