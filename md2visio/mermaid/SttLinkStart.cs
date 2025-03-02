using System.Text.RegularExpressions;

namespace md2visio.mermaid
{
    internal class SttLinkStart : SynState
    {
        public override SynState NextState()
        {
            SynState state = Create(Ctx);
            if (state.IsEmpty()) throw new SynException("syntax error", Ctx);

            return state;
        }

        public static SynState Create(SynContext ctx) 
        {
            if (!IsLinkStart(ctx)) return Empty;

            SynState state = new SttLinkStart();
            state.Ctx = ctx;
            state.Save(ctx.TestGroups["estart"].Value);

            return state.Slide(state.Fragment.Length).Forward<SttLinkLabel>();
        }

        public static bool IsLinkStart(SynContext ctx)
        {
            return ctx.Test("^(?<estart>[xo<]?(--(?![-ox>])|==(?![=ox>])))"); // -- or ==
        }
    }
}
