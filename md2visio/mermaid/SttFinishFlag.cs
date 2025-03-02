using md2visio.mermaid.@internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace md2visio.mermaid
{
    internal class SttFinishFlag : SynState
    {
        public override SynState NextState()
        {
            return Save(Take().Cache).Clear().Forward<SttChar>();
        }
    }
}
