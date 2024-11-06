using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using EntityFactory.Systems;
using Frikandisland.Systems;

namespace EntityFactory.Components.Graphics
{
    // Static model: no animation
    class StaticModel : RenderComponent
    {
        // Constructor: try to load model, if error go to default and report to logger
        public StaticModel(string parent, string modelName) : base(parent)
        {
            try
            {
                model = AssetLoader.GetModel(modelName);
            }
            catch (Exception e)
            {
                FrikanLogger.Write($"Could not set model {modelName} for {parent}: {e}");
                model = AssetLoader.GetModel("percolator");
            }
            shader = new AmbientShader(parent);
        }

        // Main draw function: called every loop
        public override void Draw(Matrix projection, Matrix view, Vector3 viewVector)
        {
            // Always check if all required fields have been assigned
            CheckRequiredParts();

            // Actual draw function
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
                        shader.SetParameters(world, view, viewVector, projection);
                    }
                }
                mesh.Draw();
            }
        }
    }
}
