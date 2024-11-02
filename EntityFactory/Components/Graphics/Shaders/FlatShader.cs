using EntityFactory.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EntityFactory.Components.Graphics
{
    public class FlatShader : ShaderComponent
    {
        public Texture2D texture;
        public FlatShader(string parent, Texture2D texture) : base(parent) 
        {
            this.SetEffect("flat");
            this.texture = texture;
        }

        public Vector4 ambientColor = new Vector4(1f, 1f, 1f, 1f);
        public float ambientIntensity = 1f;

        public override void SetParameters(Matrix world, Matrix view, Matrix projection)
        {
            // Base paramters
            effect.Parameters["World"].SetValue(world);
            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);

            // Ambients
            effect.Parameters["AmbientColor"].SetValue(ambientColor);
            effect.Parameters["AmbientIntensity"].SetValue(ambientIntensity);
            
            // Texture
            effect.Parameters["ModelTexture"].SetValue(texture);
        }
    }
}
