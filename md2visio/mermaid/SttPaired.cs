using md2visio.mermaid.@internal;
using System.Text;
using System.Text.RegularExpressions;

namespace md2visio.mermaid
{
    internal class SttPaired : SynState
    {
        public override SynState NextState()
        {
            if(!IsPaired(Ctx)) throw new SynException("expect pair start", Ctx);
            
            string pairStart = Ctx.TestGroups["start"].Value;
            if (!IsShapeStart(pairStart)) throw new SynException("expect shape start", Ctx);

            if(IsPaired(pairStart, Ctx))
            {
                string mid = Ctx.TestGroups["mid"].Value;
                string close = Ctx.TestGroups["close"].Value;
                if (mid.Length == 0) throw new SynException("expect pair label", Ctx);

                string pair = Ctx.TestGroups[0].Value;
                AddPart("start", pairStart);
                AddPart("mid", mid);
                AddPart("close", close);
                return Save(pair).Slide(Ctx.TestGroups[0].Length).Forward<SttChar>();
            }

            throw new SynException("expect pair close", Ctx);
        }

        public static bool IsPaired(SynContext ctx)
        {
            if (!ctx.Test(@"(?<start>[\[\{\(>]+)")) return false;
            return IsPaired(ctx.TestGroups["start"].Value, ctx);
        }

        public static bool IsPaired(string pairStart, SynContext ctx)
        {
            if (MmdPaired.IsPaired(pairStart, ctx.TextBuilder))
            {
                ctx.TestGroups = MmdPaired.Groups;
                return true;
            }
            return false;
        }

        bool IsShapeStart(string fragment)
        {
            return Regex.IsMatch(fragment, @"^(>|\[{1,2}|\{{1,2}|\({1,3}|\[\(|\[\\|\[/|\(\[)$");
        }

        bool IsShapeClose(string fragment)
        {
            return Regex.IsMatch(fragment, @"^(\]{1,2}|\}{1,2}|\){1,3}|\)\]|\\\]|/\]|\]\))$");
        }


    }
}
