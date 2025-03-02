using md2visio.mermaid.@internal;

namespace md2visio.mermaid
{
    internal class SttComment : SynState
    {
        public override SynState NextState()
        {
            if(Expect(@"(?s)(?<cmnt>%%[^\n]+?(%%)?(?=\n))")) return Save(Sequence("cmnt")).Forward<SttChar>();

            return Take(2).Forward<SttText>();
        }
    }
}
