using md2visio.mermaid.cmn;
using md2visio.mermaid.graph.@internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace md2visio.mermaid.graph
{
    internal class GSttLinkEnd : SynState
    {
        public override SynState NextState()
        {
            if (!IsLinkEnd(Ctx)) throw new SynException("syntax error", Ctx);

            return Create(Ctx);
        }

        public static SynState Create(SynContext ctx)
        {
            if (!IsLinkEnd(ctx)) return Empty;

            SynState state = new GSttLinkEnd();
            state.Ctx = ctx;
            state.Save(ctx.TestGroups["edgeEnd"].Value);

            return state.Slide(state.Fragment.Length).Forward<GSttChar>();
        }

        public static bool IsLinkEnd(SynContext ctx)
        {
            return ctx.Test("^(?<edgeEnd>[<xo]?-{2,}[-ox>])") ||
                ctx.Test("^(?<edgeEnd>[<xo]?={2,}[=ox>])");
        }
    }
}
