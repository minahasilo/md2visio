namespace md2visio.mermaid._cmn
{
    internal abstract class SttQuoted : SynState
    {
        public static bool TestQuoted(SynContext ctx)
        {
            return ctx.Test(@"^(?s)""(?<quote>[^""]*)""");
        }
    }
}
