using Genesis.Common;
using Genesis.Management.WP8;

namespace Genesis.WP8
{
    using System.Collections.Generic;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    public class BackgroundManager
    {
        private readonly ContentManager contentManager;
        private readonly int width;
        private readonly int height;

        private readonly List<BackgroundLayer> backgrounds = new List<BackgroundLayer>();

        public BackgroundManager(ContentManager contentManager, int width, int height)
        {
            this.contentManager = contentManager;
            this.width = width;
            this.height = height;
        }

        public void Scroll()
        {
            foreach (BackgroundLayer backgroundLayer in backgrounds)
            {
                backgroundLayer.Scroll();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (BackgroundLayer backgroundLayer in backgrounds)
            {
                backgroundLayer.Draw(spriteBatch);
            }
        }

        public void SetAndLoadLayers(BackgroundLayerCollection backgroundCollection)
        {
            backgrounds.Clear();
            foreach (BackgroundLayerDefinition definition in backgroundCollection)
            {
                var backgroundLayer = new BackgroundLayer(contentManager, definition.Image, definition.YSpeed, definition.XSpeed, width, height);
                backgroundLayer.LoadContent();
                backgrounds.Add(backgroundLayer);
            }
        }
    }
}
