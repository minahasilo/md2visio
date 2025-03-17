using md2visio.struc.figure;
using md2visio.struc.packet;

namespace md2visio.vsdx
{
    internal class VBuilderPac : VBuilder
    {
        public VBuilderPac(Figure figure) : base(figure)
        {
        }

        public override void Build(string outputFile)
        {
            new VShapeDrawerPac(VisioApp)
                .DrawPacket(figure.DownCast<Packet>(), OrderInnerNodes());

            SaveAndClose(outputFile);
        }

        List<INode> OrderInnerNodes()
        {
            List<INode> nodes = figure.InnerNodes.Values.ToList<INode>();
            IComparer<INode> comparer = new PacBitsComparer();
            nodes.Sort(comparer);

            return nodes;
        }
    }

    class PacBitsComparer : IComparer<INode>
    {
        public int Compare(INode? x, INode? y)
        {
            if(x == null || y == null) return 0;
            PacBlock bitsx = (PacBlock) x, 
                bitsy = (PacBlock) y;

            return bitsx.BitStart - bitsy.BitStart;
        }
    }
}
