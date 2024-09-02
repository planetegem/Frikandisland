using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using EntityFactory.Systems;
using EntityFactory.Components.Positioning;
using EntityFactory.Entities;
using EntityFactory.EntityFactory.Components.Graphics.Shaders;
using EntityFactory.EntityFactory.Components.Graphics;

namespace EntityFactory.Components.Graphics
{
    abstract class RenderComponent : Component
    {
        // Model & Texture
        protected Model model;
        protected Texture2D texture;

        // Apply custom shader (default is main shader)
        protected ShaderComponent shader;
        public ShaderComponent Shader { set { shader = value; } }

        // Entity's PositionComponent provides coordinates & rotation
        protected PositionComponent positioner;

        // Parent is saved as part of Component constructor
        public RenderComponent(Entity parent, PositionComponent positioner) : base(parent)
        {
            this.positioner = positioner;
        }

        // RenderComponents always have a Draw method
        public abstract void Draw(Matrix projection, Matrix view);


        // Standard effect configuration for MonoGame (fallback in case of shader failure)
        public bool EnableStandardEffect = false;
        protected void StandardEffect(BasicEffect effect, Matrix world, Matrix view, Matrix projection, Texture2D texture = null)
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

    // Basic RenderComponent: does not have a texture
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
                shader = new NormalShader(parent, texture);
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
