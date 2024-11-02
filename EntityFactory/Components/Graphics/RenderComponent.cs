using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using EntityFactory.Components.Positioning;
using EntityFactory.Systems;
using Frikandisland.Systems;

namespace EntityFactory.Components.Graphics
{
    public abstract class RenderComponent : Component
    {
        // Model & Texture
        protected Model model;

        // Apply custom shader (default is main shader)
        protected ShaderComponent shader;
        public ShaderComponent Shader 
        { 
            set { shader = value; } 
            get { return shader; }
        }

        // Entity's PositionComponent provides coordinates & rotation
        protected PositionComponent positioner;
        public PositionComponent Positioner { set { positioner = value; } }

        // Parent is saved as part of Component constructor
        public RenderComponent(string parent) : base(parent) { }

        // RenderComponents always have a Draw method
        public abstract void Draw(Matrix projection, Matrix view, Vector3 viewVector);

        // Standard effect configuration for MonoGame (fallback in case of shader failure)
        public bool EnableStandardEffect = false;
        protected void StandardEffect(BasicEffect effect, Matrix world, Matrix view, Matrix projection)
        {
            effect.TextureEnabled = false;
            effect.EnableDefaultLighting();
            effect.World = world;
            effect.View = view;
            effect.Projection = projection;
        }
        
        // Other defaults and error logging
        protected void CheckRequiredParts()
        {
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
        }
    }



    

    
}
