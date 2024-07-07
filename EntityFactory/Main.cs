using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using EntityFactory.Entities;
using EntityFactory.Systems;

namespace EntityFactory
{
    public class Main : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Matrix projection;

        private Entity entity;
        private World world;

        private bool debugMode; 
        private KeyboardState keyboardState;

        // For instructions
        private SpriteFont font;
        private Instructions instructions;

        private EntitySystem entitySystem;
        private EntityLoader entityLoader;

        public Main()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.graphics.PreferredBackBufferWidth = 1200;
            this.graphics.PreferredBackBufferHeight = 720;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.debugMode = true;
            this.keyboardState = Keyboard.GetState();
        }

        protected override void Initialize()
        {
            this.entitySystem = EntitySystem.getInstance();
            base.Initialize();

            
        }

        protected override void LoadContent()
        {
            this.entityLoader = EntityLoader.GetInstance(Content);


            this.spriteBatch = new SpriteBatch(GraphicsDevice);
            this.font = Content.Load<SpriteFont>("fonts/coordinateFont");
            this.instructions = new Instructions(this.graphics.PreferredBackBufferWidth, this.font);

            // Create player object & load model
            this.entity = new Zombie();

            // Create world
            this.world = new World(12, 12, this.entity);
            this.world.LoadAssets(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();
            // Exit using escape
            if (newState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            if (newState.IsKeyDown(Keys.Tab) && keyboardState.IsKeyUp(Keys.Tab))
            {
                this.debugMode = !this.debugMode;
            }
            this.keyboardState = newState;

            this.world.Update(gameTime);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(255, 205, 205));
    
            // Draw 3D assets
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            this.world.Draw(this.spriteBatch, this.debugMode);

            // Add instructions
            this.instructions.Draw(spriteBatch);
            
            base.Draw(gameTime);
        }
    }
}
