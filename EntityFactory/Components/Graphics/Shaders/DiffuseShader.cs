using EntityFactory.Entities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using EntityFactory.Systems;

namespace EntityFactory.Components.Graphics
{
    public class DiffuseShader : ShaderComponent
    {
        public Texture2D texture;

        public DiffuseShader(string parent, Texture2D texture) : base(parent) 
        {
            this.SetEffect("diffuse");
            this.texture = texture;
        }
        public DiffuseShader(string parent, string textureName) : base(parent)
        {
            this.SetEffect("diffuse");
            this.texture = AssetLoader.GetTexture(textureName);
        }

        public Vector4 ambientColor = new Vector4(1f, 1f, 1f, 1f);
        public float ambientIntensity = 0.7f;

        public Vector4 diffuseColor = new Vector4(1f, 1f, 1f, 1f);
        public float diffuseIntensity = 0.2f;
        public Vector3 diffuseDirection = new Vector3(1f, -5f, 5f);

        public override void SetParameters(Matrix world, Matrix view, Matrix projection)
        {
            // Base paramters
            effect.Parameters["World"].SetValue(world);
            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);

            // Ambients
            effect.Parameters["AmbientColor"].SetValue(ambientColor);
            effect.Parameters["AmbientIntensity"].SetValue(ambientIntensity);

            // Diffuse lighting
            effect.Parameters["DiffuseColor"].SetValue(diffuseColor);
            effect.Parameters["DiffuseIntensity"].SetValue(diffuseIntensity);
            effect.Parameters["DiffuseLightDirection"].SetValue(diffuseDirection);

            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(world));
            effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
            effect.Parameters["ModelTexture"].SetValue(texture);
        }
    }
}
