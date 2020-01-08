using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TrafficSim.Network
{
    public sealed class Junction
    {
        public Junction(Vector2 position)
        {
            this.Position = position;
            this.Incoming = new HashSet<Road>(0, RoadPositionComparer.Instance);
            this.Outgoing = new HashSet<Road>(0, RoadPositionComparer.Instance);
        }

        public Vector2 Position { get; }

        public HashSet<Road> Incoming { get; }
        public HashSet<Road> Outgoing { get; }

        public Road ConnectWith(Junction other, float speedLimit = Road.DefaultSpeedLimit, RoadType type = RoadType.TwoWay)
        {
            var road = new Road(this, other, speedLimit, type);
            this.Outgoing.Add(road);
            other.Incoming.Add(road);

            if (type == RoadType.TwoWay)
            {
                this.Incoming.Add(road);
                other.Outgoing.Add(road);
            }

            return road;
        }
    }
}
