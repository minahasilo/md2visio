using md2visio.mermaid.@internal;
using System.Text.RegularExpressions;

namespace md2visio.mermaid
{
    internal class SttNoLabelLink : SynState
    {
        public override SynState NextState()
        {
            SynState edge = Create(Ctx);
            if (edge.IsEmpty()) throw new SynException("syntax error", Ctx);                       
            
            return edge;
        }

        public static SynState Create(SynContext ctx)
        {
            if (!IsNoLabelLink(ctx)) return Empty;

            SynState state = new SttNoLabelLink();
            state.Ctx = ctx;
            state.Save(ctx.TestGroups["edge"].Value);

            return state.Slide(state.Fragment.Length).Forward<SttChar>();
        }

        public static bool IsNoLabelLink(SynContext context) 
        {
            return IsSolidSingleLine(context) 
                || IsDashedSingleLine(context) 
                || IsEmptyLink(context);
        }

        public static bool IsDashedSingleLine(SynContext ctx)
        {
            return ctx.Test(@"^(?<edge>[<xo]?-+[\.]+-+[xo>]?)");
        }

        public static bool IsSolidSingleLine(SynContext ctx)
        {
            return ctx.Test(@"^(?<edge>[<xo]?-{2,}[-xo>])") ||
                ctx.Test(@"^(?<edge>[<xo]?={2,}[=xo>])");
        }

        public static bool IsEmptyLink(SynContext ctx)
        {
            return ctx.Test("^(?<edge>~{3,})");
        }

    }
}
