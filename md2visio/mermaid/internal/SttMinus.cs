using System.Text.RegularExpressions;

namespace md2visio.mermaid.@internal
{
    internal class SttMinus : SynState
    {
        public override SynState NextState()
        {
            // left
            if (Cache.Length > 1) return Forward<SttWord>();
            else if (Cache.Length == 1)
            {
                if (Regex.IsMatch(Cache, "^[xo]$"))
                {
                    if (SttLinkStart.IsLinkStart(Ctx)) 
                        return Clear().Restore(1).Forward<SttLinkStart>();
                    else if (SttNoLabelLink.IsNoLabelLink(Ctx)) 
                        return Clear().Restore(1).Forward<SttNoLabelLink>();
                    return Take().Forward<SttChar>();
                }
                else if (SttLinkStart.IsLinkStart(Ctx) ||
                        SttNoLabelLink.IsNoLabelLink(Ctx))
                {
                    return Forward<SttWord>();
                }
                else
                {
                    return Take().Forward<SttChar>();
                }
            }

            // right
            if (SttConfig.IsConfig(Ctx)) return Forward<SttConfig>();
            if (SttLinkStart.IsLinkStart(Ctx)) return Forward<SttLinkStart>();
            else if (SttNoLabelLink.IsNoLabelLink(Ctx)) return Forward<SttNoLabelLink>();

            return Take().Forward<SttChar>();
        }

    }
}
