using md2visio.mermaid.cmn;
using md2visio.mermaid.graph.@internal;

namespace md2visio.mermaid.graph
{
    internal class SttQuoted : SynState
    {

        public override SynState NextState()
        {
            // e.g. "..."
            string? pre = Peek(-1), next = Peek(1);
            if (Buffer.Length > 0 && !string.IsNullOrWhiteSpace(Buffer.Last().ToString())) // e.g. D"
                return Take().Forward<GSttChar>();

            if (Expect(@"(?s)""(?<quote>[^""]*)"""))
                return Clear().Save(Sequence("quote")).Forward<GSttChar>();

            throw new SynException("syntax error", Ctx);
        }

        public static bool IsQuotedSequence(SynContext ctx)
        {
            return ctx.Test(@"^(?s)""(?<quote>[^""]*)""");
        }

    }
}
