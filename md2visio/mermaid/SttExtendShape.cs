using md2visio.mermaid.@internal;
using System.Text.RegularExpressions;

namespace md2visio.mermaid
{
    internal class SttExtendShape : SynState
    {
        public override SynState NextState()
        {
            string? pre = Peek(-1);
            if (string.IsNullOrWhiteSpace(pre)) throw new SynException("syntax error", Ctx);
            if (!Expect(@"@\{")) throw new SynException("expect @{...}", Ctx);

            Restore();
            if (!SttPaired.IsPaired("{", Ctx)) throw new SynException("expect @{...}", Ctx);

            Group pair = Ctx.TestGroups[0];
            return Save(pair.Value).Slide(pair.Length).Forward<SttChar>();
        }
    }
}
