namespace md2visio.mermaid.@internal
{
    internal class SttChar : SynState
    {
        public override SynState NextState()
        {
            string? next = Ctx.Peek();
            if (next == null) return EndOfFile;

            if (next == ";") { return Forward<SttWordFlag>(); }
            if (next == "\n") { Ctx.Line++; return Forward<SttWordFlag>(); }
            if (next == "\t") { return Forward<SttWordFlag>(); }
            if (next == " ") { return Forward<SttWordFlag>(); }
            if (next == "@") { return Forward<SttWordFlag>(); }
            if (next == "~") { return Forward<SttWordFlag>(); }
            if (next == "{") { return Forward<SttWordFlag>(); }
            if (next == "[") { return Forward<SttWordFlag>(); }
            if (next == "(") { return Forward<SttWordFlag>(); }
            if (next == "<") { return Forward<SttWordFlag>(); }
            if (next == "`") { return Forward<SttBackQuote>(); }
            if (next == "\"") { return Forward<SttQuoted>(); }
            if (next == "-") { return Forward<SttMinus>(); }
            if (next == "=") { return Forward<SttEqual>(); }
            if (next == "&") { return Forward<SttAmp>(); }
            if (next == "%") { return Forward<SttPercent>(); }
            if (next == "|") { return Forward<SttPipedLinkText>(); }
            if (next == ")") { throw new SynException("unexpected ')'", Ctx); }
            if (next == "}") { throw new SynException("unexpected '}'", Ctx); }
            if (next == "]") { throw new SynException("unexpected ']'", Ctx); }
            if (next == ">") { throw new SynException("unexpected '>'", Ctx); }

            return Take().Forward<SttChar>();
        }

        public static SynState Run(SynContext ctx)
        {
            SynState state = new SttChar();
            state.Ctx = ctx;
            return state.NextState();
        }
    }
}
