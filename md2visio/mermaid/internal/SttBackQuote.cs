namespace md2visio.mermaid.@internal
{
    internal class SttBackQuote : SynState
    {
        public override SynState NextState()
        {
            if (SttMermaidClose.IsMermaidClose(Ctx)) return Forward<SttMermaidClose>();

            return Take().Forward<SttChar>();
        }
    }
}
