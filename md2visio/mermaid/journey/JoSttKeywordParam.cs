using md2visio.mermaid._cmn;

namespace md2visio.mermaid.journey
{
    internal class JoSttKeywordParam : SttKeywordParam
    {
        public override SynState NextState()
        {
            return Save(ExpectedGroups["param"].Value).Forward<JoSttChar>();
        }
    }
}
