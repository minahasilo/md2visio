using md2visio.mermaid.@internal;
using System.Text.RegularExpressions;

namespace md2visio.mermaid
{
    internal class SttPipedLinkText : SynState
    {
        public override SynState NextState()
        {
            if (!IsPipedLinkText(Ctx)) throw new SynException("expect piped link text", Ctx);

            Group textGroup = Ctx.TestGroups["text"];
            string text = textGroup.Value.Trim();
            return Save(text).Slide(Ctx.TestGroups[0].Length).Forward<SttChar>();
        }

        public static bool IsPipedLinkText(SynContext ctx)
        {
            if (!ctx.Test(@"(?s)^\s*\|")) return false;
            if (!ctx.Test(@"(?s)^\s*\|(?<text>.+?)\|")) return false;
            return true;
        }
    }
}
