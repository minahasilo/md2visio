using md2visio.mermaid._cmn;
using md2visio.mermaid.packet;
using md2visio.struc.figure;

namespace md2visio.struc.packet
{
    internal class PacBuilder : FigureBuilder
    {
        Packet packet = new Packet();

        public PacBuilder(SttIterator iter) : base(iter)
        {
        }

        public override void Build(string outputFile)
        {
            while (iter.HasNext())
            {
                SynState cur = iter.Next();
                if (cur is SttMermaidStart) { }
                if (cur is SttMermaidClose) { packet.ToVisio(outputFile); break; }
                if (cur is PacSttTuple) { BuildBits((PacSttTuple) cur); }
                if (cur is SttComment) { packet.InitDirectiveFromComment(cur.Fragment); }
                if (cur is SttFrontMatter) { packet.InitFrontMatter(cur.Fragment); } 
            }
        }

        void BuildBits(PacSttTuple tuple)
        {
            PacBlock bits = new PacBlock(tuple.GetPart("bits"), tuple.GetPart("name"));
            packet.AddInnerNode(bits);
        }
    }
}
