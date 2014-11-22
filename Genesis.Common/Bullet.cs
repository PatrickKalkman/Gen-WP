using Genesis.Common;

namespace Genesis.WP8
{
    using Microsoft.Xna.Framework;

    public class Bullet : AnimatedSprite
    {
        private const float BulletYVelocity = 9.0f;

        private readonly GenesisGameManager genesisGameManager;

        private readonly int bulletIndex;

        public Bullet(Rectangle screenDimension, ITextureManager textureManager, GenesisGameManager genesisGameManager, int bulletIndex, int width, int height)
            : base(screenDimension, textureManager, width, height, 12, 31, 1, 3, 3, 4, new Point(1, 765), SpriteAnimationType.Continous)
        {
            this.genesisGameManager = genesisGameManager;
            this.bulletIndex = bulletIndex;
            this.SetVelocityY(-BulletYVelocity);
            this.SetStartBulletPosition();
            this.Visible = true;
        }

        public void SetStartBulletPosition()
        {
            this.SetLocation(
                this.genesisGameManager.GetSpaceshipBulletStartPositionX(this.bulletIndex) - (int)(this.Width / 2),
                this.genesisGameManager.GetSpaceshipBulletStartPositionY(this.bulletIndex) - (int)(this.Height / 2));
        }

        public override void UpdateGame(GameTime gameTime)
        {
        }
    }
}