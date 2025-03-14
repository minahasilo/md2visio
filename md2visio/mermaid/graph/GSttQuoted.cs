using md2visio.mermaid._cmn;
using md2visio.mermaid.graph.@internal;

namespace md2visio.mermaid.graph
{
    internal class GSttQuoted : SttQuoted
    {

        public override SynState NextState()
        {
            // e.g. "..."
            string? pre = Peek(-1), next = Peek(1);
            if (Buffer.Length > 0 && !string.IsNullOrWhiteSpace(Buffer.Last().ToString())) // e.g. D"
                return Take().Forward<GSttChar>();

            if (Expect(@"(?s)""(?<quote>[^""]*)"""))
                return ClearBufer().Save(ExpectedGroup("quote")).Forward<GSttChar>();

            throw new SynException("syntax error", Ctx);
        }

    }
}
