using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TrafficSim.Network;

namespace TrafficSim
{
    public sealed class NetworkVisualization
    {
        private const float radius = 0.05f;

        private static readonly Color HighlightColor = Color.White;

        private readonly BasicEffect Effect;
        private readonly List<Road> Roads;
        private readonly HashSet<Road> HighlightedRoads;

        private short[] indices;
        private VertexPositionColor[] vertices;

        public NetworkVisualization(GraphicsDevice device)
        {
            this.indices = new short[0];
            this.vertices = new VertexPositionColor[0];

            this.Effect = new BasicEffect(device)
            {
                VertexColorEnabled = true,
                World = Matrix.Identity,
                // See world from straight above
                View = Matrix.CreateLookAt(Vector3.Up * 10.0f, Vector3.Zero, Vector3.Forward),
                Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, device.Viewport.AspectRatio, 0.01f, 100.0f)
            };

            this.Roads = new List<Road>();
            this.HighlightedRoads = new HashSet<Road>();
        }

        public void Add(Road road)
        {
            this.Roads.Add(road);
            this.Rebuild();
        }

        public void Highlight(Road road)
        {
            if (!this.Roads.Contains(road))
            {
                throw new Exception("Cannot highlight a road that is not part of the network");
            }

            this.HighlightedRoads.Add(road);
            this.Rebuild();
        }

        public void Remove(Road road)
        {
            this.Roads.Remove(road);
            this.HighlightedRoads.Remove(road);
        }

        private void Rebuild()
        {
            var vertexCount = this.Roads.Count * 9 * 2;
            this.vertices = new VertexPositionColor[vertexCount];
            this.indices = new short[vertexCount];

            short vertexIndex = 0;
            for (var i = 0; i < this.Roads.Count; i++)
            {
                var road = this.Roads[i];
                var color = this.HighlightedRoads.Contains(road) ? HighlightColor : GetRoadColor(road);

                this.indices[vertexIndex] = vertexIndex;
                this.vertices[vertexIndex++] = new VertexPositionColor(new Vector3(road.Start.X, 0, road.Start.Y), color);

                this.indices[vertexIndex] = vertexIndex;
                this.vertices[vertexIndex++] = new VertexPositionColor(new Vector3(road.End.X, 0, road.End.Y), color);

                this.CreateMarker(ref vertexIndex, road.Start, color);
                this.CreateMarker(ref vertexIndex, road.End, color);
            }
        }

        private static Color GetRoadColor(Road road)
        {
            var range = Road.MaxSpeedLimit - Road.MinSpeedLimit;
            var value = (road.SpeedLimit - Road.MinSpeedLimit) / range;

            return Color.Lerp(Color.Red, Color.Green, value);
        }

        public void Clear()
        {
            this.Roads.Clear();
            this.HighlightedRoads.Clear();
            this.Rebuild();
        }

        public void ClearHighLights()
        {
            this.HighlightedRoads.Clear();
            this.Rebuild();
        }

        public IReadOnlyList<Road> All() => this.Roads;

        private void CreateMarker(ref short vertexIndex, Vector2 position, Color color)
        {
            var corners = new Vector3[]
            {
                new Vector3(position.X - radius, 0, position.Y - radius),
                new Vector3(position.X + radius, 0, position.Y - radius),

                new Vector3(position.X + radius, 0, position.Y - radius),
                new Vector3(position.X + radius, 0, position.Y + radius),

                new Vector3(position.X + radius, 0, position.Y + radius),
                new Vector3(position.X - radius, 0, position.Y + radius),

                new Vector3(position.X - radius, 0, position.Y + radius),
                new Vector3(position.X - radius, 0, position.Y - radius)
            };

            for (var i = 0; i < corners.Length; i++)
            {
                this.indices[vertexIndex] = vertexIndex;
                this.vertices[vertexIndex++] = new VertexPositionColor(corners[i], color);
            }
        }

        public void Draw()
        {
            if (this.vertices.Length > 0)
            {
                this.Effect.CurrentTechnique.Passes[0].Apply();
                this.Effect.GraphicsDevice.DrawUserIndexedPrimitives(PrimitiveType.LineList, this.vertices, 0, this.vertices.Length, this.indices, 0, this.indices.Length / 2);
            }
        }
    }
}
