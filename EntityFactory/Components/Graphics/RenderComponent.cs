using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EntityFactory.Entities;
using System;
using EntityFactory.Systems;
using EntityFactory.Components.Bounding;

namespace EntityFactory.Components.Graphics
{
    // RenderComponent is responsible for 
    internal abstract class RenderComponent : Component
    {
        // Model & Texture
        protected Model model;
        protected Texture2D texture;

        // Apply custom shader (default is main shader)
        protected Effect shader;
        public Effect Shader { set { shader = value; } }

        // Entity's PositionComponent provides coordinates & rotation
        protected PositionComponent positioner;

        // Parent is saved as part of Component constructor; try to set shader to default
        public RenderComponent(Entity parent, PositionComponent positioner) : base(parent)
        {
            this.positioner = positioner;
            try
            {
                shader = EntityLoader.GetEffect("main");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                shader = null;
            }
        }

        // RenderComponents always have a Draw method
        public abstract void Draw(Matrix projection, Matrix view);

        // Standard effect configuration (if no shader provided)
        protected void StandardShader(BasicEffect effect, Matrix world, Matrix view, Matrix projection, Texture2D texture = null)
        {
            if (texture != null)
            {
                effect.Texture = texture;
                effect.TextureEnabled = true;
            }
            effect.EnableDefaultLighting();
            effect.World = world;
            effect.View = view;
            effect.Projection = projection;
        }
    }

    // Basic RenderComponent: no textures, no shaders (defaults to base effects)
    internal class SimpleModel : RenderComponent
    {
        public SimpleModel(Entity parent, PositionComponent positioner, string model) : base(parent, positioner)
        {
            this.model = EntityLoader.GetModel(model);
            shader = null;
        }

        public override void Draw(Matrix projection, Matrix view)
        {
            if (model == null) { throw new Exception($"No model loaded when rendering component for {parent.id}"); }

            Matrix world = Matrix.CreateRotationZ(positioner.Angle) * Matrix.CreateTranslation(new Vector3(positioner.Position, positioner.OffsetZ));
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    StandardShader((BasicEffect)part.Effect, world, view, projection);
                }
                mesh.Draw();
            }
        }
    }

    // Basic static model: no animations; texture is optional
    internal class StaticModel : RenderComponent
    {
        // If only model argument is given: implies same name for model & texture
        public StaticModel(Entity parent, PositionComponent positioner, string model) : base(parent, positioner)
        {
            try
            {
                this.model = EntityLoader.GetModel(model);
                texture = EntityLoader.GetTexture(model);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating renderer for {parent.id}: {e}");
            }
        }
        // Else: apply separate texture and model
        public StaticModel(Entity parent, PositionComponent positioner, string model, string texture) : base(parent, positioner)
        {
            try
            {
                this.model = EntityLoader.GetModel(model);
                this.texture = EntityLoader.GetTexture(texture);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating renderer for {parent.id}: {e}");
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
                    // Failsafe
                    if (shader == null || texture == null)
                    {
                        System.Diagnostics.Debug.WriteLine($"Shader or texture for {parent.id} appear to be missing: switching to default effects");
                        StandardShader((BasicEffect)part.Effect, world, view, projection);
                    }
                    else
                    {
                        part.Effect = shader;
                        shader.Parameters["World"].SetValue(world);
                        shader.Parameters["View"].SetValue(view);
                        shader.Parameters["Projection"].SetValue(projection);

                        shader.Parameters["AmbientColor"].SetValue(Color.White.ToVector4());
                        shader.Parameters["AmbientIntensity"].SetValue(1f);

                        shader.Parameters["DiffuseColor"].SetValue(Color.White.ToVector4());
                        shader.Parameters["DiffuseIntensity"].SetValue(0.0001f);
                        shader.Parameters["DiffuseLightDirection"].SetValue(new Vector3(100, -100, 100));

                        Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(world));
                        shader.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                        shader.Parameters["ModelTexture"].SetValue(texture);
                    }
                }
                mesh.Draw();
            }
        }
    }

    // AnimatedModel: same as StaticModel, but also creates an AnimationComponent
    internal class AnimatedModel : RenderComponent
    {
        // AnimationComponent updates model based on gametime
        private AnimationComponent animator;
        public AnimationComponent Animator { get { return animator; } }

        // Again dual constructor: either model & texture are the same, or texture is explicitly defined
        public AnimatedModel(Entity parent, PositionComponent positioner, string model) : base(parent, positioner)
        {
            try
            {
                this.model = EntityLoader.GetModel(model);
                texture = EntityLoader.GetTexture(model);
                animator = new AnimationComponent(parent, this.model);
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
                this.model = EntityLoader.GetModel(model);
                this.texture = EntityLoader.GetTexture(texture);
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
                        StandardShader((BasicEffect)part.Effect, world, view, projection);
                    }
                    else
                    {
                        part.Effect = shader;
                        shader.Parameters["World"].SetValue(world);
                        shader.Parameters["View"].SetValue(view);
                        shader.Parameters["Projection"].SetValue(projection);

                        shader.Parameters["AmbientColor"].SetValue(Color.White.ToVector4());
                        shader.Parameters["AmbientIntensity"].SetValue(1f);

                        shader.Parameters["DiffuseColor"].SetValue(Color.White.ToVector4());
                        shader.Parameters["DiffuseIntensity"].SetValue(0.0001f);
                        shader.Parameters["DiffuseLightDirection"].SetValue(new Vector3(100, -100, 100));

                        Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(world));
                        shader.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                        shader.Parameters["ModelTexture"].SetValue(texture);

                        animator.ApplyUpdateToRender(part);
                    }
                }
                mesh.Draw();
            }
        }
    }
}
