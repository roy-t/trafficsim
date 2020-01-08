using System.Collections.Generic;
using TrafficSim.Network;

namespace TrafficSim.PathFinding
{
    public class PathFinderResult
    {
        public PathFinderResult(IReadOnlyList<Road> path, float cost)
        {
            this.FoundPath = true;
            this.Path = path;
            this.Cost = cost;
        }

        public PathFinderResult()
        {
            this.FoundPath = false;
            this.Path = null;
        }

        public bool FoundPath { get; }

        public float Cost { get; }

        public IReadOnlyList<Road> Path { get; }
    }
}
