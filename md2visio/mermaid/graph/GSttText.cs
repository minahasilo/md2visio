using md2visio.mermaid._cmn;
using md2visio.mermaid.graph.@internal;
using System.Text.RegularExpressions;

namespace md2visio.mermaid.graph
{
    internal class GSttText : SynState
    {
        public override SynState NextState()
        {
            if (!Ctx.FindContainerFrag("graph|flowchart|subgraph").Success)
                throw new SynException("syntax error", Ctx);

            string text = Buffer;
            ClearBufer().SlideSpaces();
            if (string.IsNullOrWhiteSpace(text)) return Forward<GSttChar>();

            if (Regex.IsMatch(text, @"^[\^=\|><]|~{1,2}$"))
                throw new SynException("unexpected node string", Ctx);

            return Save(text).Forward<GSttChar>();
        }
    }
}
