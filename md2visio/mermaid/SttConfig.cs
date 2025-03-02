using md2visio.mermaid.@internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md2visio.mermaid
{
    internal class SttConfig : SynState
    {
        public override SynState NextState()
        {
            if (!Until(@"(?s)---\s*(?<cfg>.*?)\n\s*---\s*(?=\n)")) throw new SynException("expect '---'", Ctx);

            Save(Sequence("cfg"));
            return Clear().Forward<SttChar>();
        }

        public static bool IsConfig(SynContext ctx)
        {
            return ctx.Test(@"^(?<cstart>---\s*\n)");
        }
    }
}
