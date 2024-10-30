using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EntityFactory.Components.Positioning;
using EntityFactory.Entities;


namespace EntityFactory.Components.Graphics
{
    abstract class RenderComponent : Component
    {
        // Model & Texture
        protected Model model;
        protected Texture2D texture;

        // Apply custom shader (default is main shader)
        protected ShaderComponent shader;
        public ShaderComponent Shader 
        { 
            set { shader = value; } 
            get { return shader; }
        }

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



    

    
}
