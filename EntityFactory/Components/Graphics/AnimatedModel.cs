using EntityFactory.Components.Positioning;
using EntityFactory.Entities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using EntityFactory.Systems;

namespace EntityFactory.Components.Graphics
{
    // AnimatedModel: same as StaticModel, but also creates an AnimationComponent
    class AnimatedModel : RenderComponent
    {
        // AnimationComponent updates model based on gametime
        private AnimationComponent animator;
        public AnimationComponent Animator { get { return animator; } }

        // Again dual constructor: either model & texture are the same, or texture is explicitly defined
        public AnimatedModel(Entity parent, PositionComponent positioner, string model) : base(parent, positioner)
        {
            try
            {
                this.model = AssetLoader.GetModel(model);
                texture = AssetLoader.GetTexture(model);
                animator = new AnimationComponent(parent, this.model);
                shader = new FlatShader(parent, texture);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating renderer for {parent.id}: {e}");
            }
        }
        public AnimatedModel(Entity parent, PositionComponent positioner, string model, string texture) : base(parent, positioner)
        {
            try
            {
                this.model = AssetLoader.GetModel(model);
                this.texture = AssetLoader.GetTexture(texture);
                animator = new AnimationComponent(parent, this.model);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating renderer for {parent.id}: {e}");
            }
        }

        // Main draw function: also call update logic from AnimationComponent
        public override void Draw(Matrix projection, Matrix view)
        {
            if (model == null) { throw new Exception("No model loaded when rendering component"); }

            Matrix world = Matrix.CreateRotationZ(positioner.Angle) * Matrix.CreateTranslation(new Vector3(positioner.Position, positioner.OffsetZ));
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    // Failsafe
                    if (shader == null || texture == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Shader or texture for {parent.id} appear to be missing: switching to default effects");
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
