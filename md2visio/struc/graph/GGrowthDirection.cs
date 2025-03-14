using md2visio.struc.figure;

namespace md2visio.struc.graph
{
    internal class GGrowthDirection
    {
        public GGrowthDirection(Container figure)
        {
            Decide(figure);
        }

        public void Decide(Container figure)
        {
            H = 0;
            V = 0;
            if (figure.Direction == "LR") H = 1;
            else if (figure.Direction == "RL") H = -1;
            else if (figure.Direction == "TD" || figure.Direction == "TB") V = -1;
            else V = 1; // BT
        }

        public int H { get; set; } = 0;
        public int V { get; set; } = 0;
    };
}
