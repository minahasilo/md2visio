using md2visio.mermaid.cmn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md2visio.mermaid.journey
{
    internal class JoSttTriple : SynState
    {
        public override SynState NextState()
        {
            string triple = MatchedGroups[0].Value.Trim();
            foreach (string s in triple.Split(":"))
            {
                if (string.IsNullOrEmpty(s)) throw new SynException("task item can't be empty", Ctx);
                AddCompo(s);
            }

            return Save(triple).Forward<JoSttChar>();
        }

        public static bool IsTripleLine(SynContext ctx)
        {
            return ctx.Expect(@"([^:\n]+:){2}[^:\n]+(?=\n)");
        }
    }
}
