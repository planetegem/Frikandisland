using EntityFactory.Entities;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EntityFactory.Components.Graphics
{
    class NormalShader : ShaderComponent
    {
        public Texture2D texture;

        // Constructor: set effect to "normal"
        public NormalShader(Entity parent, Texture2D texture) : base(parent) 
        {
            this.SetEffect("normal");
            this.texture = texture;
        }

        public Vector4 ambientColor = new Vector4(0.6f, 0.6f, 0.6f, 1f);
        public float ambientIntensity = 0.1f;

        public Vector4 diffuseColor = new Vector4(1f, 1f, 1f, 1f);
        public float diffuseIntensity = 0.8f;
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
