using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TrafficSim.Network
{
    public sealed class Junction
    {
        public Junction(Vector2 position)
        {
            this.Position = position;
            this.Incoming = new List<Road>(0);
            this.Outgoing = new List<Road>(0);
        }

        public Vector2 Position { get; }

        public List<Road> Incoming { get; }
        public List<Road> Outgoing { get; }

        public void AddTwoWayConnection(Road road)
        {
            this.Incoming.Add(road);
            this.Outgoing.Add(road);
        }
    }
}
