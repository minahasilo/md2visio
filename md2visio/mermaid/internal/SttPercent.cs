using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using md2visio.mermaid.@internal;

namespace md2visio.mermaid.@internal
{
    internal class SttPercent : SynState
    {
        public override SynState NextState()
        {
            if (string.IsNullOrWhiteSpace(Cache))
            {
                if (Expect("%%")) return Restore(2).Forward<SttComment>();
            }

            return Take().Forward<SttChar>();
        }
    }
}
