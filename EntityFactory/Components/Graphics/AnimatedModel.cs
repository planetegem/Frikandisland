using EntityFactory.Components.Positioning;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using EntityFactory.Systems;
using Frikandisland.Systems;
using EntityFactory.Components.State;
using EntityFactory.Components.Graphics.Shaders;

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
            animator = new AnimationComponent(parent, this.model);
            shader = new MainShader(parent, texture);
        }
        public AnimatedModel(string parent, string model, string texture) : base(parent)
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

            shader = new FlatShader(parent, this.texture);
            animator = new AnimationComponent(parent, this.model);
        }

        // Main draw function: also call update logic from AnimationComponent
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
                        shader.SetParameters(world, view, projection);
                        animator.ApplyUpdateToRender(part);
                    }
                }
                mesh.Draw();
            }
        }
    }
}
