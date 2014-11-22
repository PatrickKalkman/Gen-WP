using Microsoft.Xna.Framework.Graphics;

namespace Genesis.Common
{
    public interface ITextureManager
    {
        Texture2D Texture { get; }

        void Load(string textureFilename);
    }
}