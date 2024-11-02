using EntityFactory.Components.Positioning;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using EntityFactory.Systems;
using Frikandisland.Systems;
using EntityFactory.Components.State;

namespace EntityFactory.Components.Graphics
{
    // AnimatedModel: same as TexturedModel, but also creates an AnimationComponent
    class AnimatedModel : RenderComponent
    {
        // AnimationComponent updates model based on gametime
        private AnimationComponent animator;
        public void ConfigureAnimations(AnimationDictionary animationDictionary, EntityBrain brain)
        {
            animator.Dictionary = animationDictionary;
            animator.Brain = brain;
        }

        // Again dual constructor: either model & texture are the same, or texture is explicitly defined
        public AnimatedModel(string parent, string model) : base(parent)
        {
            try { this.model = AssetLoader.GetModel(model); }
            catch (Exception e)
            {
                FrikanLogger.Write($"Error setting model for {parent}: {e}");
                this.model = AssetLoader.GetModel("percolator");
            }
            animator = new AnimationComponent(parent, this.model);
        }
        
        // Main draw function: also call update logic from AnimationComponent
        public override void Draw(Matrix projection, Matrix view, Vector3 viewVector)
        {
            // Error logging
            CheckRequiredParts();

            if (shader == null)
            {
                FrikanLogger.Write($"No shader found for {parent}: switching to basic ambient shader. Set shader in entity constructor.");
                shader = new AmbientShader(parent);
            }

            // Actual draw function: also calls AnimationComponent logic to update bone positions
            Matrix world = Matrix.CreateRotationZ(positioner.Angle) * Matrix.CreateTranslation(new Vector3(positioner.Position, positioner.OffsetZ));
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    // Failsafe
                    if (EnableStandardEffect | shader.Effect == null)
                    {
                        StandardEffect((BasicEffect)part.Effect, world, view, projection);
                    }
                    else
                    {
                        part.Effect = shader.Effect;
                        shader.SetParameters(world, view, viewVector, projection);
                        animator.ApplyUpdateToRender(part);
                    }
                }
                mesh.Draw();
            }
        }
    }
}
