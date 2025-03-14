using md2visio.mermaid.graph;
using md2visio.mermaid.graph.@internal;
using md2visio.mermaid.journey;
using md2visio.mermaid.pie;
using md2visio.struc.graph;
using md2visio.struc.journey;
using md2visio.struc.pie;
using md2visio.vsdx;

namespace md2visio.mermaid._cmn
{
    internal class TypeMap
    {
        public static readonly Dictionary<string, Type> KeywordMap = new Dictionary<string, Type>
        {
            { "graph", typeof(GSttKeyword) }, { "flowchart", typeof(GSttKeyword) },
            { "journey", typeof(JoSttKeyword) },
            { "pie", typeof(PieSttKeyword) },
        };

        public static readonly Dictionary<string, Type> CharMap = new Dictionary<string, Type>
        {
            { "graph", typeof(GSttChar) }, { "flowchart", typeof(GSttChar) },
            { "journey", typeof(JoSttChar) },
            { "pie", typeof(PieSttChar) }
        };

        public static readonly Dictionary<string, Type> BuilderMap = new Dictionary<string, Type>()
        {
            { "graph", typeof(GBuilder) }, { "flowchart", typeof(GBuilder) },
            { "journey", typeof(JoBuilder) },
            { "pie", typeof(PieBuilder) },
        };
    }
}
