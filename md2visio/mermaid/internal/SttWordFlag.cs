using System.Text.RegularExpressions;

namespace md2visio.mermaid.@internal
{
    internal class SttWordFlag : SynState
    {
        public override SynState NextState()
        {
            // a word flag may make a node string
            // left
            if (!string.IsNullOrWhiteSpace(Cache)) { return Forward<SttWord>(); }

            // right            
            string? next = Peek();
            if (next == "\n") { return Forward<SttFinishFlag>(); }

            next = SlideSpaces().Peek();
            if (next == ";") { return Forward<SttFinishFlag>(); }
            if (next == "`") { return Forward<SttBackQuote>(); }
            if (next == "=") { return Forward<SttNoLabelLink>(); }
            if (next == ">") { return Forward<SttNoLabelLink>(); }
            if (next == "<") { return Forward<SttLinkStart>(); }
            if (next == "{") { return Forward<SttPaired>(); }
            if (next == "[") { return Forward<SttPaired>(); }
            if (next == "(") { return Forward<SttPaired>(); }
            if (next == "@") { return Forward<SttExtendShape>(); }
            if (next == "%") { return Forward<SttPercent>(); }
            if (next == "~") { return Forward<SttTilde>(); }

            return Forward<SttChar>();
        }
    }
}
