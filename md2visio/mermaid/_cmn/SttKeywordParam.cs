using md2visio.mermaid._cmn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md2visio.mermaid._cmn
{
    internal abstract class SttKeywordParam: SynState
    {
        public static bool HasParam(SynContext ctx)
        {
            return ctx.Expect(@"[^\S\n]*(?<param>\S.+?)\s*(?=\n)");
        }
    }
}
