using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Genesis.Common
{
    public abstract class AnimatedSprite
    {
        protected Vector2 location;

        protected Vector2 previousLocation;

        protected ITextureManager textureManager;

        protected float scale;

        protected Color color = Color.White;
        
        private readonly SpriteAnimationType typeOfAnimation;

        protected readonly Rectangle screenDimension;

        private float timePerFrame;

        private Vector2 velocity;

        private int currentFrame;

        private double elapsedTime;

        protected AnimatedSprite(Rectangle screenDimension, ITextureManager textureManager, int width, int height, int frameWidth, int frameHeight, float scale) 
            : this(screenDimension, textureManager, width, height, frameWidth, frameHeight, scale, 1, 1, 0, Point.Zero, SpriteAnimationType.None)
        {
        }

        protected AnimatedSprite(Rectangle screenDimension, ITextureManager textureManager, int width, int height, int frameWidth, int frameHeight, float scale, int numberOfFrames, int numberOfFramesPerRow, int framesPerSecond, Point textureSourceStart, SpriteAnimationType typeOfAnimation)
        {
            this.screenDimension = screenDimension;
            this.textureManager = textureManager;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.scale = scale;
            this.typeOfAnimation = typeOfAnimation;
            this.timePerFrame = 1.0f / framesPerSecond;
            this.Width = width;
            this.Height = height;
            this.Visible = true;
            this.NumberOfFrames = numberOfFrames;
            this.NumberOfFramesPerRow = numberOfFramesPerRow;
            this.TextureSourceStart = textureSourceStart;
        }

        protected AnimatedSprite(Rectangle screenDimension, ITextureManager textureManager, SpriteAnimationType typeOfAnimation)
        {
            this.screenDimension = screenDimension;
            this.textureManager = textureManager;
            this.typeOfAnimation = typeOfAnimation;
        }

        public int Width { get; set; }

        public int Height { get; set; }

        public int NumberOfFrames { get; set; }

        public int NumberOfFramesPerRow { get; set; }

        public int FrameWidth { get; set; }

        public int FrameHeight { get; set; }

        public Point TextureSourceStart { get; set; }

        public float? Angle { get; set; }

        public Vector2 RotationOrigin { get; set; }

        public Vector2 Location
        {
            get
            {
                return location;
            }
        }

        public Vector2 PreviousLocation
        {
            get
            {
                return previousLocation;
            }
        }

        public int FramesPerSecond
        {
            set
            {
                timePerFrame = 1.0f / value;
            }
        }

        public float XPosition
        {
            get
            {
                return this.location.X;
            }
        }

        public float YPosition
        {
            get
            {
                return this.location.Y;
            }
        }

        public float XVelocity
        {
            get
            {
                return this.velocity.X;
            }

            set
            {
                this.velocity = new Vector2(value, this.velocity.Y);
            }
        }

        public float YVelocity
        {
            get
            {
                return this.velocity.Y;
            }

            set
            {
                this.velocity = new Vector2(this.velocity.X, value);
            }
        }

        public bool Visible
        {
            get; set;
        }

        public Rectangle SourceRectangle { get; set; }

        public void Update(GameTime gameTime)
        {
            this.previousLocation = new Vector2(this.location.X, this.Location.Y);
            this.location += this.velocity;

            if (this.NumberOfFrames > 1)
            {
                this.elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (this.elapsedTime > this.timePerFrame)
                {
                    this.currentFrame++;
                    if (this.currentFrame >= this.NumberOfFrames)
                    {
                        this.currentFrame = 0;
                        if (this.typeOfAnimation == SpriteAnimationType.SingleShot)
                        {
                            this.Visible = false;
                        }
                    }

                    this.elapsedTime -= this.timePerFrame;
                }
            }

            this.UpdateGame(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                var textureSourceRectangle = new Rectangle
                    {
                        Width = this.Width,
                        Height = this.Height,
                        X = this.TextureSourceStart.X,
                        Y = this.TextureSourceStart.Y
                    };

                if (NumberOfFrames > 1)
                {
                    textureSourceRectangle.X += (this.currentFrame % this.NumberOfFramesPerRow) * FrameWidth;

                    if (NumberOfFramesPerRow < NumberOfFrames)
                    {
                        textureSourceRectangle.Y += (this.currentFrame / NumberOfFramesPerRow) * FrameHeight;
                    }

                    SourceRectangle = new Rectangle(textureSourceRectangle.X, textureSourceRectangle.Y, textureSourceRectangle.Width, textureSourceRectangle.Height);
                }

                if (spriteBatch != null)
                {
                    if (!Angle.HasValue)
                    {
                        spriteBatch.Draw(this.textureManager.Texture, this.location, textureSourceRectangle, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
                    }
                    else
                    {
                        spriteBatch.Draw(this.textureManager.Texture, this.location, textureSourceRectangle, color, Angle.Value, RotationOrigin, scale, SpriteEffects.None, 0);
                    }
                }
            }
        }

        public virtual Rectangle GetBoundingBox()
        {
            return new Rectangle((int)this.location.X, (int)this.location.Y, Width, Height);
        }

        public Point Center
        {
            get
            {
                return new Point((int)(this.location.X - this.Width * this.scale / 2), (int)(this.location.Y - this.Height * this.scale / 2));
            }
        }

        public abstract void UpdateGame(GameTime gameTime);


        public void SetLocation(float xPosition, float yPosition)
        {
            this.location = new Vector2(xPosition, yPosition);
        }

        public virtual void SetLocation(Point position)
        {
            this.location = new Vector2(position.X, position.Y);
        }

        public void SetVelocity(Vector2 velocityToSet)
        {
            this.velocity = new Vector2(velocityToSet.X, velocityToSet.Y);
        }

        public void SetVelocityY(float velocityY)
        {
            this.velocity = new Vector2(velocity.X, velocityY);
        }

        public void SetVelocityX(float velocityX)
        {
            this.velocity = new Vector2(velocityX, velocity.Y);
        }
    }
}