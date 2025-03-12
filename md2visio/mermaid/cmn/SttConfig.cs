namespace md2visio.mermaid.cmn
{
    internal class SttConfig : SynState
    {
        public override SynState NextState()
        {
            if (!Until(@"(?s)---\s*(?<cfg>.*?)\n\s*---\s*(?=\n)")) throw new SynException("expected '---'", Ctx);

            Save(Sequence("cfg"));
            return Clear().Forward<SttIntro>();
        }

        public static bool IsConfig(SynContext ctx)
        {
            return ctx.Test(@"^(?<cstart>---\s*\n)");
        }
    }
}
