using Genesis.Common;
using Genesis.Common.Enemy;
using Genesis.Common.GameStates;
using Genesis.Common.PowerUp;
using Genesis.Common.Score;
using Genesis.Management.WP8;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Genesis.WP8
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public partial class GenesisGame : Game
    {
        private Spaceship spaceship;
        private EnemyManager enemyManager;
        private BulletManager bulletManager;
        private readonly GraphicsDeviceManager graphics;
        private ITextureManager textureManager;
        private CurveGenerator curveGenerator;
        private CollisionManager collisionManager;
        private EnemyWaveManager enemyWaveManager;
        private EnemyBulletManager enemyBulletManager;
        private RandomGenerator randomGenerator;
        private PowerUpManager powerUpManager;
        private IAngleCalculator angleCalculator;
        private BackgroundManager backgroundManager;
        private GameDefinitionLoader gameDefinitionLoader;
        private Score score;
        private GameState gameState;
        private IInputManager inputManager;
        private SpriteBatch spriteBatch;
        private GameDefinition gameDefinition;
        private HighScore highScoreLayer;
        private GenesisGameManager gameManager;
        private HighScoreManager highScoreManager;
        private StageCompletedLayer stageCompletedLayer;
        private CongratulationsLayer congratulations;

        private int killedFrameCounter = 300;
        private int currentStage;

        private const string HDFont = "HighScoreHD";
        private const string NormalFont = "HighScore";

        public GenesisGame()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.PlatformSpecificFactory = new WindowsPhoneFactory();
        }

        private double scaleY;
        private double scaleX;

        public IPlatformSpecificFactory PlatformSpecificFactory
        {
            get; set;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Content.RootDirectory = "Content";

            int width = GraphicsDevice.Viewport.Width;
            int height = GraphicsDevice.Viewport.Height;

            scaleX = width / 480.0;
            scaleY = height / 800.0;
            var screenSize = new Rectangle(0, 0, width, height);
            CreateInstances(screenSize, width, height);

            base.Initialize();
        }

        private void CreateInstances(Rectangle screenSize, int width, int height)
        {
            this.inputManager = PlatformSpecificFactory.CreateInputManager();
            this.textureManager = new TextureManager(Content);
            this.spaceship = new Spaceship(screenSize, inputManager, this.textureManager, 92, 98, 95, 60);
            this.IsMouseVisible = true;
            this.angleCalculator = new AngleCalculator();
            this.curveGenerator = new CurveGenerator();
            this.randomGenerator = new RandomGenerator();
            this.enemyBulletManager = new EnemyBulletManager(screenSize, textureManager, Content, randomGenerator);
            this.enemyManager = new EnemyManager(screenSize, this.textureManager, this.curveGenerator, this.angleCalculator,
                this.enemyBulletManager, this.randomGenerator);
            this.gameManager = new GenesisGameManager(enemyManager) { Spaceship = this.spaceship };
            this.bulletManager = new BulletManager(screenSize, this.textureManager, gameManager, 12, Content, inputManager,
                this.angleCalculator);
            this.powerUpManager = new PowerUpManager(screenSize, new PowerUpFactory(textureManager),
                new PowerUpStorage(spaceship, Content, gameManager), enemyManager);
            this.gameDefinitionLoader = new GameDefinitionLoader(PlatformSpecificFactory.CreateGameDefinitionXmlReader(),
                new GameDefinitionParser());
            this.enemyWaveManager = new EnemyWaveManager(this.enemyManager, this.curveGenerator, powerUpManager, scaleX, scaleY);
            backgroundManager = new BackgroundManager(Content, width, height);
            this.highScoreManager = new HighScoreManager(PlatformSpecificFactory.CreateCachingService(), new HighScoreListFactory(),
                PlatformSpecificFactory.CreateFlipTileCreator());
            this.highScoreManager.Initialize();
            this.highScoreLayer = new HighScore(Content, screenSize, new HighScorePositionNumberFormatter(),
                highScoreManager.GetHighScoreList(), scaleX, scaleY);
            this.score = new Genesis.Common.Score.Score(Content, screenSize, spaceship, this.highScoreManager.GetHigestScore());
            this.collisionManager = new CollisionManager(this.bulletManager, this.enemyBulletManager, this.enemyManager, screenSize,
                this.textureManager, Content, spaceship, score);
            this.stageCompletedLayer = new StageCompletedLayer(Content, screenSize, scaleX, scaleY);
            this.congratulations = new CongratulationsLayer(Content, screenSize, scaleX, scaleY);
            this.gameState = new TitleScreenState(this, 1, "Gen");
            this.inputManager.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            this.textureManager.Load("Spaceship_Sprite_Package_by_Kryptid");
            this.gameDefinition = this.gameDefinitionLoader.LoadAllStagesContent(PlatformSpecificFactory.GetGameDefinition());
            this.backgroundManager.SetAndLoadLayers(gameDefinition.Stages[currentStage].BackgroundLayers);
            this.bulletManager.Load();
            this.collisionManager.Load();
            this.enemyBulletManager.Load();
            this.powerUpManager.Load();

            if (scaleY > 1)
            {
                score.Load(HDFont);
                this.highScoreLayer.Load(HDFont);
                this.stageCompletedLayer.Load(HDFont);
                this.congratulations.Load(HDFont);
            }
            else
            {
                score.Load(NormalFont);
                this.highScoreLayer.Load(NormalFont);
                this.stageCompletedLayer.Load(NormalFont);
                this.congratulations.Load(NormalFont);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            this.gameState.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            this.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullNone);
            this.gameState.Draw(spriteBatch);
            this.spriteBatch.End();

            base.Draw(gameTime);
        }

        public string PlayerName { get; set; }

        private bool IsFinished()
        {
            return currentStage >= gameDefinition.Stages.Count;
        }

        public IGamePageController GamePageController { get; set; }

        public void SetHighScore(string playerName)
        {
            var scoreData = new ScoreData();
            scoreData.Name = playerName;
            scoreData.Level = currentStage + 1;
            scoreData.Score = this.score.PlayerScore;

            if (highScoreManager.IsHighScore(scoreData))
            {
                highScoreManager.StoreHighScore(scoreData);
            }
        }

        public void ClearHighScores()
        {
            highScoreManager.Reset();
            highScoreLayer.RefreshHighScore(highScoreManager.GetHighScoreList());
        }

        public void Start(GameDifficulty difficulty)
        {
            this.enemyWaveManager.SetDifficulty(difficulty);
            this.enemyWaveManager.IntializeStage(gameDefinition.Stages[currentStage]);
            gameState.Start(difficulty);
        }

    }
}
