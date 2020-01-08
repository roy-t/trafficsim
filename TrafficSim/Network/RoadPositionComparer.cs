using System;
using System.Collections.Generic;

namespace TrafficSim.Network
{
    internal sealed class RoadPositionComparer : IEqualityComparer<Road>
    {
        private static readonly Lazy<RoadPositionComparer> LazyInstance = new Lazy<RoadPositionComparer>(() => new RoadPositionComparer());
        public static RoadPositionComparer Instance => LazyInstance.Value;

        public bool Equals(Road x, Road y) => x.Start == y.Start && x.End == y.End;
        public int GetHashCode(Road road)
        {
            unchecked // Overflow is fine, just wrap
            {
                var hash = 17;
                hash = (hash * 23) + road.Start.GetHashCode();
                hash = (hash * 23) + road.End.GetHashCode();
                return hash;
            }
        }
    }
}
