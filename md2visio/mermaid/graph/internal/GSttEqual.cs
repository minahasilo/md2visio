using md2visio.mermaid.cmn;
using md2visio.mermaid.graph;
using System.Text.RegularExpressions;

namespace md2visio.mermaid.graph.@internal
{
    internal class GSttEqual : SynState
    {
        public override SynState NextState()
        {
            // left
            if (Buffer.Length > 0)
            {
                if (GSttLinkStart.IsLinkStart(Ctx))
                    return Restore(Buffer.Length).Clear().Forward<GSttLinkStart>();
                if (GSttNoLabelLink.IsNoLabelLink(Ctx))
                    return Restore(Buffer.Length).Clear().Forward<GSttNoLabelLink>();

                throw new SynException("unexpected '='", Ctx);
            }

            // right
            if (GSttLinkStart.IsLinkStart(Ctx)) return Forward<GSttLinkStart>();
            else if (GSttNoLabelLink.IsNoLabelLink(Ctx)) return Forward<GSttNoLabelLink>();

            throw new SynException("unexpected '='", Ctx);
        }
    }
}
