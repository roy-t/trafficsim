using Microsoft.Xna.Framework.Graphics;

namespace TrafficUI
{
    public sealed class TextureReference
    {
        public TextureReference(Texture2D texture, int index)
        {
            this.Texture = texture;
            this.Index = index;
        }

        public Texture2D Texture { get; }
        public int Index { get; }
    }
}
