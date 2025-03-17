using md2visio.mermaid.graph;
using md2visio.mermaid.graph.@internal;
using md2visio.mermaid.journey;
using md2visio.mermaid.packet;
using md2visio.mermaid.pie;
using md2visio.struc.graph;
using md2visio.struc.journey;
using md2visio.struc.packet;
using md2visio.struc.pie;

namespace md2visio.mermaid._cmn
{
    internal class TypeMap
    {
        public static readonly Dictionary<string, Type> KeywordMap = new Dictionary<string, Type>
        {
            { "graph", typeof(GSttKeyword) }, { "flowchart", typeof(GSttKeyword) },
            { "journey", typeof(JoSttKeyword) },
            { "pie", typeof(PieSttKeyword) },
            { "packet-beta", typeof(PacSttKeyword) }, { "packet", typeof(PacSttKeyword) },
        };

        public static readonly Dictionary<string, Type> CharMap = new Dictionary<string, Type>
        {
            { "graph", typeof(GSttChar) }, { "flowchart", typeof(GSttChar) },
            { "journey", typeof(JoSttChar) },
            { "pie", typeof(PieSttChar) },
            { "packet-beta", typeof(PaSttChar) }, { "packet", typeof(PaSttChar) },
        };

        public static readonly Dictionary<string, Type> BuilderMap = new Dictionary<string, Type>()
        {
            { "graph", typeof(GBuilder) }, { "flowchart", typeof(GBuilder) },
            { "journey", typeof(JoBuilder) },
            { "pie", typeof(PieBuilder) },
            { "packet-beta", typeof(PacBuilder) }, { "packet", typeof(PacBuilder) },
        };
    }
}
