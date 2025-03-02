using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md2visio.mermaid.@internal
{
    internal class SttTilde : SynState
    {
        public override SynState NextState()
        {
            if (SttNoLabelLink.IsEmptyLink(Ctx)) return Forward<SttNoLabelLink>();

            throw new SynException("unexpected '~'", Ctx);
        }
    }
}
