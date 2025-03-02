using md2visio.mermaid.@internal;
using System.Text.RegularExpressions;

namespace md2visio.mermaid
{
    internal class SttText : SynState
    {
        public override SynState NextState()
        {
            if (!Ctx.FindContainerFrag("graph|flowchart|subgraph").Success) 
                throw new SynException("syntax error", Ctx);

            string text = Cache;
            Clear().SlideSpaces();
            if (string.IsNullOrWhiteSpace(text)) return Forward<SttChar>();

            if (Regex.IsMatch(text, @"^[\^=\|><]|~{1,2}$")) 
                throw new SynException("unexpected node string", Ctx);

            return Save(text).Forward<SttChar>();
        }
    }
}
