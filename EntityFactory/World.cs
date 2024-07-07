using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using EntityFactory.Entities;
using EntityFactory.Components;
using EntityFactory.Systems;

using System;
using System.Collections.Generic;
using EntityFactory.Components.Bounding;

namespace EntityFactory
{
    internal struct Cell
    {
        public int X; // X-coordinate in field
        public int Y; // Y-coordinate in field
        public bool Tile; // Cell is valid tile or not (for collision detection)
    }

    internal class World
    {
        // Models and textures: need to be loaded after running constructor
        private Texture2D tileTexture;
        private Texture2D squareTexture;
        private Texture2D square2Texture;
        private Texture2D circleTexture;
        private Model tileModel;
        private SpriteFont coordinateFont;
        private Effect ambientShader;

        // Projection matrix used in 3D renders
        private Matrix projection;

        // Width & height of field, provided in constructor
        private int width;
        private int height;

        // Field: boolean value of a tile determines if player can move here
        private Cell[,] field;
        private int tileSize;

        // Entities
        private Entity player;
        private Camera camera;

        // In constructor, create field on which player will move
        public World(int width, int height, Entity player)
        {
            this.width = width;
            this.height = height;
            this.field = new Cell[width, height];

            // Start filling the field: outer layer = not valid for movement
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Cell newCell = new Cell();
                    newCell.X = x;
                    newCell.Y = y;
                    newCell.Tile = !(x == 0 || y == 0 || x == width - 1 || y == height - 1);
                    this.field[x, y] = newCell;
                }
            }

            // will determine size of a tile when drawn on screen
            this.tileSize = 20;
            this.player = player;

            // Load all models & textures
            this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 200f);
            this.camera = new Camera();
        }

        // Load all relevant models and textures
        public bool LoadAssets(ContentManager cm)
        {
            try
            {
                this.tileTexture = cm.Load<Texture2D>("textures/tile");
                this.circleTexture = cm.Load<Texture2D>("textures/circle");
                this.squareTexture = cm.Load<Texture2D>("textures/square");
                this.square2Texture = cm.Load<Texture2D>("textures/square2");
                this.tileModel = cm.Load<Model>("models/tile");
                this.coordinateFont = cm.Load<SpriteFont>("fonts/coordinateFont");
                this.ambientShader = cm.Load<Effect>("effects/AmbientShader");

                return true;
            }
            catch { return false; }
        }

        // Every update: move player, check validity of movement, move camera
        public void Update(GameTime gameTime)
        {
            // Phase 1: process input
            EntitySystem.ProcessInput(gameTime);

            // Then check if move was valid
            List<BoundingArea> targets = new List<BoundingArea>();
            foreach (Cell cell in this.field)
            {
                if (!cell.Tile)
                {
                    Vector2 pos = new Vector2(cell.X + 0.5f - this.width/2, cell.Y + 0.5f - this.height/2);
                    targets.Add(new BoundingOrthogonalSquare(pos, 0.5f));
                }
            }
            EntitySystem.ResolvePosition(gameTime, targets);

            EntitySystem.Animate(gameTime);
            
            // this.player.Update(gameTime, targets);

            // Check if camera needs to be moved
            this.camera.Update(Vector2.Zero);
        }

        // Draw functions
        public void Draw(SpriteBatch sb, bool debug)
        {
            // First draw 3D map
            float startX = 0.5f - (this.width * 0.5f);
            float startY = 0.5f - (this.height * 0.5f);

            Matrix world;

            foreach (Cell cell in field)
            {
                if (cell.Tile)
                {
                    world = Matrix.CreateTranslation(new Vector3(startX + cell.X, startY + cell.Y, 0));

                    foreach (ModelMesh mesh in tileModel.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.TextureEnabled = true;
                            effect.Texture = this.tileTexture;
                            effect.World = world;
                            effect.View = this.camera.View;
                            effect.Projection = this.projection;
                        }
                        mesh.Draw();
                    }
                }
            }

            // Draw entities
            EntitySystem.Render(this.projection, this.camera.View, debug);

            // Debug mode is active
            
            if (debug)
            {
                /*
                
                // Draw minimap
                sb.Begin();
                foreach (Cell cell in this.field)
                {
                    if (cell.Tile)
                    {
                        sb.Draw(this.squareTexture, new Rectangle(cell.X * this.tileSize, cell.Y * this.tileSize, this.tileSize, this.tileSize), Color.White);
                    }
                }
                
                // Draw player bounding box(es) on field
                foreach (BoundingArea bound in this.player.Bounds.Parts)
                {
                    int correctedX = (int)((bound.X + this.width * 0.5f) * this.tileSize);
                    int correctedY = this.height * this.tileSize - (int)((bound.Y + this.height * 0.5f) * this.tileSize);
                    
                    if (bound is BoundingCircle)
                    {
                        int radius = (int)(bound.Radius * this.tileSize);
                        sb.Draw(this.circleTexture, new Rectangle(correctedX - radius, correctedY - radius, radius * 2, radius * 2), Color.White);
                    }
                    else if (bound is BoundingOrthogonalSquare)
                    {
                        int width = (int)(bound.Width * this.tileSize);
                        int height = (int)(bound.Height * this.tileSize);
                        sb.Draw(this.square2Texture, new Rectangle(correctedX - width/2, correctedY - height/2, width, height), Color.White);
                    }
                }

                // Show player coordinates under minimap
                string coordinates = "X" + Math.Round(this.player.Position.X, 2) + " / Y" + Math.Round(this.player.Position.Y, 2);
                float stringX = Instructions.AlignText(coordinates, this.coordinateFont, new Rectangle(0, 0, this.width * this.tileSize, 0));

                sb.DrawString(this.coordinateFont, coordinates, new Vector2(stringX, (this.height - 0.5f) * this.tileSize), Color.Black);
                sb.End();
                */
            }         
        }
    }
}
