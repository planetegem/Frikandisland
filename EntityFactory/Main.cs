using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Frikandisland.Systems;
using EntityFactory.Entities;
using Frikandisland.Content;

namespace EntityFactory
{
    public class Main : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Entity entity;
        private World world;

        private bool debugMode;
        private KeyboardState keyboardState;

        // For instructions
        private SpriteFont font;
        private Instructions instructions;

        private EntitySystem entitySystem;
        private AssetLoader entityLoader;

        // Projection matrix used in 3D renders
        private Matrix projection;
        private Camera camera;

        public Main()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 720;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            debugMode = true;
            keyboardState = Keyboard.GetState();
        }

        protected override void Initialize()
        {
            entitySystem = EntitySystem.getInstance();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            entityLoader = AssetLoader.GetInstance(Content);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("fonts/coordinateFont");
            instructions = new Instructions(graphics.PreferredBackBufferWidth, font);

            // Create player object & load model
            entity = new Rossem();

            // Create world
            world = new World(12, 12, entity);

            // Camera & perspective
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 200f);
        }

        protected override void Update(GameTime gameTime)
        {
            // KEYBOARD
            KeyboardState newState = Keyboard.GetState();

            // Exit using escape
            if (newState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // Enable debug mode (visible bounding boxes)
            if (newState.IsKeyDown(Keys.Tab) && keyboardState.IsKeyUp(Keys.Tab))
            {
                debugMode = !debugMode;
            }
            keyboardState = newState;

            // ENTITY SYSTEM UPDATE LOGIC
            EntitySystem.ProcessInput(gameTime);
            EntitySystem.ResolvePosition(gameTime);
            EntitySystem.Animate(gameTime);

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(255, 205, 205));

            // Draw 3D assets
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.BlendState = BlendState.Opaque;
            EntitySystem.Render(projection, debugMode);

            // Add instructions
            instructions.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
