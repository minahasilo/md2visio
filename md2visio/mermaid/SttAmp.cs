using md2visio.mermaid.@internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md2visio.mermaid
{
    internal class SttAmp : SynState
    {
        public override SynState NextState()
        {
            string? pre = Peek(-1), next2 = Peek(2);
            if (!string.IsNullOrWhiteSpace(pre)) return Take().Forward<SttChar>();
            if ("&" != next2?.Trim()) return Take().Forward<SttChar>();

            if (Ctx.LastNonFinishState() is not SttText) throw new SynException("syntax error", Ctx);

            return Save("&").Slide(2).Forward<SttChar>();
        }
    }
}
