using md2visio.mermaid.@internal;

namespace md2visio.mermaid
{
    internal class SttQuoted : SynState
    {

        public override SynState NextState()
        {
            // e.g. "..."
            string? pre = Peek(-1), next = Peek(1);
            if (Cache.Length > 0 && !string.IsNullOrWhiteSpace(Cache.Last().ToString())) // e.g. D"
                return Take().Forward<SttChar>();

            if (Expect(@"(?s)""(?<quote>[^""]*)""")) 
                return Clear().Save(Sequence("quote")).Forward<SttChar>();

            throw new SynException("syntax error", Ctx);
        }

        public static bool QuotedSequence(SynContext ctx)
        {
            return ctx.Test(@"^(?s)""(?<quote>[^""]*)""");
        }
        
    }
}
