using EntityFactory.Components.Positioning;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using EntityFactory.Systems;
using Frikandisland.Systems;

namespace EntityFactory.Components.Graphics
{
    // Basic RenderComponent: does not have a texture, for testing purposes
    class SimpleModel : RenderComponent
    {
        // Constructor: try to load model, if error go to default and report to logger
        public SimpleModel(string parent, string modelName) : base(parent)
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
                        shader.SetParameters(world, view, projection);
                    }
                }
                mesh.Draw();
            }
        }
    }
}
