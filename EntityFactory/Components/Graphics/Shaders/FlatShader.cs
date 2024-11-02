using EntityFactory.Entities;
using EntityFactory.Systems;
using Frikandisland.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EntityFactory.Components.Graphics
{
    public class FlatShader : ShaderComponent
    {
        // Constructors
        public FlatShader(string parent, Texture2D texture) : base(parent) 
        {
            this.SetEffect("flat");
            this.texture = texture;
        }
        public FlatShader(string parent, string textureName) : base(parent)
        {
            this.SetEffect("flat");
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
        public float ambientIntensity = 1f;
        public Texture2D texture;

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
            
            // Texture
            effect.Parameters["ModelTexture"].SetValue(texture);
        }
    }
}
