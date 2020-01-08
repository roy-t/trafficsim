using TrafficSim.Network;

namespace TrafficSim.PathFinding
{
    internal sealed class MinHeapNode
    {
        public MinHeapNode(Junction junction, MinHeapNode cameFrom, Road cameVia, float costSoFar, float expectedCost)
        {
            this.Junction = junction;
            this.CameFrom = cameFrom;
            this.CameVia = cameVia;
            this.CostSoFar = costSoFar;
            this.ExpectedCost = expectedCost;
        }

        public Junction Junction { get; }
        public MinHeapNode CameFrom { get; }
        public Road CameVia { get; }
        public float CostSoFar { get; }
        public float ExpectedCost { get; }
        public MinHeapNode Next { get; set; }

        public override string ToString() => $"📍{this.Junction.Position}, ⏱{this.ExpectedCost}";
    }
}
