using System.Text.RegularExpressions;

namespace md2visio.mermaid.@internal
{
    internal class SttWord : SynState
    {
        public override SynState NextState()
        {
            // keyword or text
            if (string.IsNullOrWhiteSpace(Cache)) throw new SynException("syntax error", Ctx);

            if (SttKeyword.IsKeyword(Cache)) return Forward<SttKeyword>();
            else return Forward<SttText>();
        }
    }
}
