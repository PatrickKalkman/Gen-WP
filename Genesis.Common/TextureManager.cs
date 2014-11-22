using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Genesis.Common
{
    public class TextureManager : ITextureManager
    {
        private readonly ContentManager contentManager;

        private Texture2D texture;

        public TextureManager(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        public Texture2D Texture
        {
            get
            {
                return texture;
            }
        }

        public void Load(string textureFilename)
        {
            texture = contentManager.Load<Texture2D>(textureFilename);
        }
    }
}
