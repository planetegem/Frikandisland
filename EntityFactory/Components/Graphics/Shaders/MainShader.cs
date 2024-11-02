using EntityFactory.Systems;
using Frikandisland.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EntityFactory.Components.Graphics.Shaders
{
    public class MainShader : ShaderComponent
    {
        public MainShader(string parent) : base(parent) 
        {
            ResetShader();
        }
        public MainShader(string parent, Texture2D texture) : base(parent)
        {
            ResetShader();
            this.texture = texture;
        }

        private void ResetShader()
        {
            try
            {
                switch (ConfigMaster.shaderLevel)
                {
                    case 0:
                        effect = AssetLoader.GetEffect("ambient");
                        ambientIntensity = 0.7f;
                        break;
                    case 1:
                        effect = AssetLoader.GetEffect("blankDiffuse");
                        ambientIntensity = 0.7f;
                        break;
                    case 2:
                        effect = AssetLoader.GetEffect("flat");
                        ambientIntensity = 0.7f;
                        break;
                    case 3:
                        effect = AssetLoader.GetEffect("diffuse");
                        ambientIntensity = 0.7f;
                        break;
                }
                ConfigMaster.shaderNeedsUpdate = false;
            } 
            catch(Exception e) 
            {
                FrikanLogger.Write($"Something went wrong switching effects in MainShader: {e.Message}");
                ConfigMaster.shaderLevel = 0;
                effect = AssetLoader.GetEffect("ambient");
            }
        }

        public Vector4 ambientColor = new Vector4(1f, 1f, 1f, 1f);
        public float ambientIntensity = 0.7f;
        public Texture2D texture;

        public Vector4 diffuseColor = new Vector4(1f, 1f, 1f, 1f);
        public float diffuseIntensity = 0.3f;
        public Vector3 diffuseDirection = new Vector3(1f, -1f, 2f);

        override public void SetParameters(Matrix world, Matrix view, Matrix projection)
        {
            // If shader level has been changed, evaluate which shader to use
            if (ConfigMaster.shaderNeedsUpdate)
                ResetShader();

            // If no texture: apply error texture
            if (texture == null)
            {
                FrikanLogger.Write($"No texture set for shader of {parent}: setting to default");
                texture = AssetLoader.GetTexture("error");
            }

            // Base parameters
            effect.Parameters["World"].SetValue(world);
            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);

            // Ambient parameters
            effect.Parameters["AmbientColor"].SetValue(ambientColor);
            effect.Parameters["AmbientIntensity"].SetValue(ambientIntensity);

            // Blank Diffuse
            if (ConfigMaster.shaderLevel == 1)
            {
                effect.Parameters["DiffuseColor"].SetValue(diffuseColor);
                effect.Parameters["DiffuseIntensity"].SetValue(diffuseIntensity);
                effect.Parameters["DiffuseLightDirection"].SetValue(diffuseDirection);
                Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(world));
                effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
            }

            // Texture parameters
            if (ConfigMaster.shaderLevel > 1)
                effect.Parameters["ModelTexture"].SetValue(texture);

            // Diffuse lighting
            if (ConfigMaster.shaderLevel > 2)
            {
                effect.Parameters["DiffuseColor"].SetValue(diffuseColor);
                effect.Parameters["DiffuseIntensity"].SetValue(diffuseIntensity);
                effect.Parameters["DiffuseLightDirection"].SetValue(diffuseDirection);
                Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(world));
                effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
            }
        }

    }
}
