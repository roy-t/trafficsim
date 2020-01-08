using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TrafficUI
{
    public sealed class UIEffect
    {
        private readonly Effect Effect;

        public UIEffect(Effect effect)
        {
            this.Effect = effect;
        }

        public Texture2D Texture
        {
            set => this.Effect.Parameters["Texture"].SetValue(value);
        }

        public Matrix World
        {
            set => this.Effect.Parameters["World"].SetValue(value);
        }

        public Matrix View
        {
            set => this.Effect.Parameters["View"].SetValue(value);
        }

        public Matrix Projection
        {
            set => this.Effect.Parameters["Projection"].SetValue(value);
        }

        public int Index
        {
            set => this.Effect.Parameters["Index"].SetValue((float)value);
        }

        public float Contrast
        {
            set => this.Effect.Parameters["Contrast"].SetValue(value);
        }

        public int Channels
        {
            set => this.Effect.Parameters["Channels"].SetValue((float)value);
        }

        public void Apply() => this.Effect.CurrentTechnique.Passes[0].Apply();
    }
}
