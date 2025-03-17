using md2visio.mermaid._cmn;
using md2visio.mermaid.graph.@internal;
using System.Text.RegularExpressions;

namespace md2visio.mermaid.graph
{
    internal class GSttKeyword : SynState
    {
        public override SynState NextState()
        {
            if (!IsKeyword(Buffer)) throw new SynException("syntax error", Ctx);
            if (Buffer == "direction")
            {
                if (!GSttKeywordParam.HasParam(Ctx)) return Forward<GSttText>();
            }

            Save(Buffer).ClearBuffer().SlideSpaces();
            if (GSttKeywordParam.HasParam(Ctx)) return Forward<GSttKeywordParam>();

            return Forward<GSttChar>();
        }

        public static bool IsKeyword(string word)
        {
            return Regex.IsMatch(word, "^(graph|flowchart|subgraph|end|style|linkStyle|class|classDef|direction|click)$");
        }

        public static bool IsDirection(string word)
        {
            return Regex.IsMatch(word, "^(LR|RL|TD|TB|BT)$");
        }
    }
}
