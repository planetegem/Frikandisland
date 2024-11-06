using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using EntityFactory.Systems;
using Frikandisland.Systems;
using System;

namespace EntityFactory.Components.Graphics
{
    public class DiffuseShader : ShaderComponent
    {
        // Constructors
        public DiffuseShader(string parent, Texture2D texture) : base(parent) 
        {
            this.SetEffect("diffuse");
            this.texture = texture;
        }
        public DiffuseShader(string parent, string textureName) : base(parent)
        {
            this.SetEffect("diffuse");
            try
            {
                this.texture = AssetLoader.GetTexture(textureName);
            }
            catch (Exception e)
            {
                FrikanLogger.Write($"Error assigning texture for {parent}: {e}");
                this.texture = AssetLoader.GetTexture("error");
            }
        }

        // Fields
        public Vector4 ambientColor = new Vector4(1f, 1f, 1f, 1f);
        public float ambientIntensity = 0.7f;

        public Texture2D texture;

        public Vector4 diffuseColor = new Vector4(1f, 1f, 1f, 1f);
        public float diffuseIntensity = 0.2f;
        public Vector3 diffuseDirection = new Vector3(1f, -5f, 5f);

        // Main function (called every draw cycle)
        public override void SetParameters(Matrix world, Matrix view, Vector3 viewVector, Matrix projection)
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

            // Texture
            effect.Parameters["ModelTexture"].SetValue(texture);
        }
    }
}
