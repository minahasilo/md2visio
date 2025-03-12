using md2visio.mermaid.graph.@internal;
using md2visio.mermaid.journey;

namespace md2visio.mermaid.cmn
{
    internal class SttCtxChar : SynState
    {
        Dictionary<string, Type> typeMap = new Dictionary<string, Type>
        {
            { "graph", typeof(GSttChar) }, { "flowchart", typeof(GSttChar) },
            { "journey", typeof(JoSttChar) }
        };

        public override SynState NextState()
        {
            (bool success, string graph) = Ctx.FindContainerFrag(SttFigureType.Supported);
            if (success) return Forward(typeMap[graph]);
            else return Forward<SttIntro>();
        }
    }
}
