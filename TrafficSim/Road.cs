using Microsoft.Xna.Framework;

namespace TrafficSim
{
    public sealed class Road
    {
        public Road(Vector2 start, Vector2 end)
        {
            this.Start = start;
            this.End = end;
        }

        public Vector2 Start { get; }
        public Vector2 End { get; }
    }
}
