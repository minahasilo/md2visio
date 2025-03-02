using md2visio.mermaid.@internal;
using System.Text.RegularExpressions;

namespace md2visio.mermaid
{
    internal class SttKeyword : SynState
    {
        public override SynState NextState()
        {
            if(!IsKeyword(Cache)) throw new SynException("syntax error", Ctx);
            if(Cache == "direction")
            {
                if (!SttKeywordParam.HasParam(Ctx)) return Forward<SttText>();
            }

            Save(Cache).Clear().SlideSpaces();
            if (SttKeywordParam.HasParam(Ctx)) return Forward<SttKeywordParam>();
            
            return Forward<SttChar>();
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
