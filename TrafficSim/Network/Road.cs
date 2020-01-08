using Microsoft.Xna.Framework;

namespace TrafficSim.Network
{
    public sealed class Road
    {
        public const float MaxSpeed = 120;
        public const float MinSpeed = 30;


        public Road(Vector2 start, Vector2 end, RoadType roadType = RoadType.TwoWay)
        {
            this.StartJunction = new Junction(start);
            this.EndJunction = new Junction(end);
            this.RoadType = roadType;
        }

        public Vector2 Start => this.StartJunction.Position;
        public Vector2 End => this.EndJunction.Position;

        public Junction StartJunction { get; }
        public Junction EndJunction { get; }

        /// <summary>
        /// In KM/h
        /// </summary>
        public float SpeedLimit => 50;

        public RoadType RoadType { get; }

        public Road Add(Vector2 end)
        {
            var road = new Road(this.End, end);
            this.EndJunction.AddTwoWayConnection(road);
            road.StartJunction.AddTwoWayConnection(this);

            return road;
        }
    }
}
