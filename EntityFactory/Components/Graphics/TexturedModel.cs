using EntityFactory.Components.Positioning;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using EntityFactory.Systems;
using Frikandisland.Systems;

namespace EntityFactory.Components.Graphics
{
    // Model with textures, not animated
    class TexturedModel : RenderComponent
    {
        // Simple constructor: model & texture have the same name
        // If not found, default to error
        public TexturedModel(string parent, string model) : base(parent)
        {
            try
            {
                this.model = AssetLoader.GetModel(model);
                try
                {
                    texture = AssetLoader.GetTexture(model);
                }
                catch (Exception e)
                {
                    FrikanLogger.Write($"Error setting texture for {parent}: {e}");
                    texture = AssetLoader.GetTexture("error");
                }
            }
            catch (Exception e)
            {
                FrikanLogger.Write($"Error setting model for {parent}: {e}");
                this.model = AssetLoader.GetModel("percolator");
            }
            shader = new DiffuseShader(parent, texture);
        }

        // Constructor with custom texture
        // If not found, default to error
        public TexturedModel(string parent, string model, string texture) : base(parent)
        {
            try
            {
                this.model = AssetLoader.GetModel(model);
                try
                {
                    this.texture = AssetLoader.GetTexture(texture);
                }
                catch (Exception e)
                {
                    FrikanLogger.Write($"Error setting texture for {parent}: {e}");
                    this.texture = AssetLoader.GetTexture("error");
                }
            }
            catch (Exception e)
            {
                FrikanLogger.Write($"Error setting model for {parent}: {e}");
                this.model = AssetLoader.GetModel("percolator");
            }
            shader = new DiffuseShader(parent, this.texture);
        }

        // main draw function, called every loop
        public override void Draw(Matrix projection, Matrix view)
        {
            // Error logging
            if (positioner == null)
            {
                positioner = new PositionComponent("SimpleModelError");
                FrikanLogger.Write($"No position found for {parent}: assign a PositionComponent in entity constructor");
            }
            if (model == null)
            {
                model = AssetLoader.GetModel("percolator");
                FrikanLogger.Write($"No model loaded when rendering component for {parent}; switching to percolator");
            }
            if (texture == null)
            {
                texture = AssetLoader.GetTexture("error");
                FrikanLogger.Write($"No model loaded when rendering component for {parent}; switching to error texture");
            }

            // Actual draw function
            Matrix world = Matrix.CreateRotationZ(positioner.Angle) * Matrix.CreateTranslation(new Vector3(positioner.Position, positioner.OffsetZ));
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    // Failsafe: fallback to standard effect matrix from MonoGame
                    if (EnableStandardEffect | shader.Effect == null)
                    {
                        StandardEffect((BasicEffect)part.Effect, world, view, projection);
                    }
                    else
                    {
                        part.Effect = shader.Effect;
                        shader.SetParameters(world, view, projection);
                    }
                }
                mesh.Draw();
            }
        }
    }
}
