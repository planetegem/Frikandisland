using EntityFactory.Components.Positioning;
using EntityFactory.Entities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using EntityFactory.Systems;

namespace EntityFactory.Components.Graphics
{
    // Model with textures, not animated
    class TexturedModel : RenderComponent
    {
        // Simple constructor: model & texture have the same name
        public TexturedModel(Entity parent, PositionComponent positioner, string modelName) : base(parent, positioner)
        {
            try
            {
                model = AssetLoader.GetModel(modelName);
                try
                {
                    texture = AssetLoader.GetTexture(modelName);
                    shader = new FlatShader(parent, texture);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine($"Error setting texture for {parent.id}: {e}");
                    texture = AssetLoader.GetTexture("error");
                    shader = new FlatShader(parent, texture);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting model for {parent.id}: {e}");
            }
        }

        // Constructor with custom texture
        public TexturedModel(Entity parent, PositionComponent positioner, string modelName, string textureName) : base(parent, positioner)
        {
            try
            {
                model = AssetLoader.GetModel(modelName);

                try
                {
                    texture = AssetLoader.GetTexture(textureName);
                    shader = new NormalShader(parent, texture);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine($"Error setting texture for {parent.id}: {e}");
                    texture = AssetLoader.GetTexture("error");
                    shader = new FlatShader(parent, texture);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting model for {parent.id}: {e}");
            }
        }

        public override void Draw(Matrix projection, Matrix view)
        {
            if (model == null) { throw new Exception($"No model loaded when rendering component for {parent.id}"); }

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
