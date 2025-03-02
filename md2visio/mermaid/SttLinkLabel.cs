using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

namespace md2visio.mermaid
{
    internal class SttLinkLabel : SynState
    {
        public override SynState NextState()
        {
            // [<ox]--LINK_LABEL or [<ox]==LINK_LABEL
            if(!IsLinkText(Ctx)) throw new SynException("expect link text", Ctx);

            return Create(Ctx);
        }

        public static SynState Create(SynContext ctx)
        {
            if (!IsLinkText(ctx)) return Empty;

            Group textGroup = ctx.TestGroups["text"];
            string text = textGroup.Value.Trim();

            SynState state = new SttLinkLabel();
            state.Ctx = ctx;
            state.Save(text).Slide(textGroup.Index + textGroup.Length);

            return state.Forward<SttLinkEnd>();
        }

        public static bool IsLinkText(SynContext ctx) 
        { 
            if(ctx.LastState() is not SttLinkStart) return false;

            string tag = ctx.LastState().Fragment.Substring(1, 1);
            if (!ctx.Test(@$"(?s)^(?<text>.+?)(?=[<xo]?{tag}{{2,}}[{tag}ox>])"))
                return false;

            Group textGroup = ctx.TestGroups["text"];
            string text = textGroup.Value.Trim();
            if (text.Length == 0) return false; 

            return true;
        }
    }
}
