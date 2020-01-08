using TrafficSim.Network;

namespace TrafficSim.PathFinding
{
    internal sealed class MinHeapNode
    {
        public MinHeapNode(Road road, MinHeapNode cameFrom, float costSoFar, float expectedCost)
        {
            this.Road = road;
            this.CameFrom = cameFrom;
            this.CostSoFar = costSoFar;
            this.ExpectedCost = expectedCost;
        }

        public Road Road { get; }
        public MinHeapNode CameFrom { get; set; }
        public float CostSoFar { get; set; }
        public float ExpectedCost { get; set; }
        public MinHeapNode Next { get; set; }
    }
}
