using System.Text.RegularExpressions;

namespace md2visio.mermaid.@internal
{
    internal class SttEqual : SynState
    {
        public override SynState NextState()
        {
            // left
            if (Cache.Length > 0)
            {
                if (SttLinkStart.IsLinkStart(Ctx)) 
                    return Restore(Cache.Length).Clear().Forward<SttLinkStart>();
                if (SttNoLabelLink.IsNoLabelLink(Ctx)) 
                    return Restore(Cache.Length).Clear().Forward<SttNoLabelLink>();

                throw new SynException("unexpected '='", Ctx);
            }

            // right
            if (SttLinkStart.IsLinkStart(Ctx)) return Forward<SttLinkStart>();
            else if (SttNoLabelLink.IsNoLabelLink(Ctx)) return Forward<SttNoLabelLink>();

            throw new SynException("unexpected '='", Ctx);
        }
    }
}
