using md2visio.mermaid.cmn;
using System.Text.RegularExpressions;

namespace md2visio.mermaid.graph.@internal
{
    internal class GSttMinus : SynState
    {
        public override SynState NextState()
        {
            // left
            if (Buffer.Length > 1) return Forward<GSttWord>();
            else if (Buffer.Length == 1)
            {
                if (Regex.IsMatch(Buffer, "^[xo]$"))
                {
                    if (GSttLinkStart.IsLinkStart(Ctx))
                        return Clear().Restore(1).Forward<GSttLinkStart>();
                    else if (GSttNoLabelLink.IsNoLabelLink(Ctx))
                        return Clear().Restore(1).Forward<GSttNoLabelLink>();
                    return Take().Forward<GSttChar>();
                }
                else if (GSttLinkStart.IsLinkStart(Ctx) ||
                        GSttNoLabelLink.IsNoLabelLink(Ctx))
                {
                    return Forward<GSttWord>();
                }
                else
                {
                    return Take().Forward<GSttChar>();
                }
            }

            // right
            if (SttConfig.IsConfig(Ctx)) return Forward<SttConfig>();
            if (GSttLinkStart.IsLinkStart(Ctx)) return Forward<GSttLinkStart>();
            else if (GSttNoLabelLink.IsNoLabelLink(Ctx)) return Forward<GSttNoLabelLink>();

            return Take().Forward<GSttChar>();
        }

    }
}
