using System.Collections.Generic;

namespace TrafficSim
{
    public sealed class PathFinder
    {
        public IReadOnlyList<Road> FindPath(Road from, Road to)
        {
            return new List<Road>() { from, to };
        }
    }
}
