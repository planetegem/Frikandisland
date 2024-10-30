using EntityFactory.Components.Positioning;
using EntityFactory.Entities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using EntityFactory.Systems;

namespace EntityFactory.Components.Graphics
{
    // Basic RenderComponent: does not have a texture, for testing purposes
    class SimpleModel : RenderComponent
    {
        public SimpleModel(Entity parent, PositionComponent positioner, string modelName) : base(parent, positioner)
        {
            try
            {
                model = AssetLoader.GetModel(modelName);
                shader = new AmbientShader(parent);
                texture = null;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Could not set model {modelName} for {parent.id}: {e}");
                model = null;
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
