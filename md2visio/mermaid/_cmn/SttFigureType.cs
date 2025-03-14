using md2visio.mermaid.graph;
using System.Text.RegularExpressions;

namespace md2visio.mermaid._cmn
{
    internal class SttFigureType : SynState
    {
        public static readonly string Supported = 
            "graph|flowchart|sequenceDiagram|classDiagram|stateDiagram|stateDiagram-v2|" +
            "erDiagram|journey|gantt|pie|quadrantChart|requirementDiagram|gitGraph|C4Context|mindmap|" +
            "timeline|zenuml|sankey-beta|xychart-beta|block-beta|packet-beta|kanban|architecture-beta";

        Dictionary<string, Type> typeMap = TypeMap.KeywordMap;

        public override SynState NextState()
        {
            string kw = Buffer.Trim();
            if (!IsFigure(kw) || !typeMap.ContainsKey(kw)) throw new SynException("unknown graph type", Ctx);

            return Forward(typeMap[kw]);
        }

        public static bool IsFigure(string word)
        {
            return Regex.IsMatch(word, $"^({Supported})$");
        }
    }
}
