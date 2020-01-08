using Microsoft.Xna.Framework;

namespace TrafficSim.Network
{
    public sealed class Road
    {
        public const float MaxSpeedLimit = 120;
        public const float DefaultSpeedLimit = 50;
        public const float MinSpeedLimit = 30;


        public Road(Junction start, Junction end, float speedLimit = DefaultSpeedLimit, RoadType roadType = RoadType.TwoWay)
        {
            this.StartJunction = start;
            this.EndJunction = end;
            this.SpeedLimit = speedLimit;
            this.RoadType = roadType;
        }

        public Vector2 Start => this.StartJunction.Position;
        public Vector2 End => this.EndJunction.Position;

        public Junction StartJunction { get; }
        public Junction EndJunction { get; }

        /// <summary>
        /// In KM/h
        /// </summary>
        public float SpeedLimit { get; }

        public RoadType RoadType { get; }
    }
}
