using EntityFactory.Systems;
using Frikandisland.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EntityFactory.Components.Graphics.Shaders
{
    public class MainShader : ShaderComponent
    {
        // Constructors
        public MainShader(string parent) : base(parent) 
        {
            ResetShader();
        }
        public MainShader(string parent, Texture2D texture) : base(parent)
        {
            ResetShader();
            this.texture = texture;
        }
        public MainShader(string parent, string textureName) : base(parent)
        {
            ResetShader();
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

        private void ResetShader()
        {
            try
            {
                switch (ConfigMaster.shaderLevel)
                {
                    case -1:
                    case 0:
                        effect = AssetLoader.GetEffect("ambient");
                        ambientIntensity = 1f;
                        break;
                    case -2:
                        effect = AssetLoader.GetEffect("blankDiffuse");
                        ambientIntensity = 0.7f;
                        break;
                    case -3:
                        effect = AssetLoader.GetEffect("blankSpecular");
                        ambientIntensity = 0.7f;
                        FrikanLogger.Write("test");
                        break;
                    case 1:
                        effect = AssetLoader.GetEffect("flat");
                        ambientIntensity = 1f;
                        break;
                    case 2:
                        effect = AssetLoader.GetEffect("diffuse");
                        ambientIntensity = 0.7f;
                        break;
                    case 3:
                        effect = AssetLoader.GetEffect("specular");
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
        public float ambientIntensity = 0.8f;
        public Texture2D texture;

        public Vector4 diffuseColor = new Vector4(1f, 1f, 1f, 1f);
        public float diffuseIntensity = 0.25f;
        public Vector3 diffuseDirection = new Vector3(1f, -1f, 2f);

        public float shininess = 20f;
        public Vector4 specularColor = new Vector4(0.7f, 0.7f, 0.7f, 0.7f);
        public float specularIntensity = 0.55f;


        override public void SetParameters(Matrix world, Matrix view, Vector3 viewVector, Matrix projection)
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

            // Ambient lighting parameters
            effect.Parameters["AmbientColor"].SetValue(ambientColor);
            effect.Parameters["AmbientIntensity"].SetValue(ambientIntensity);

            // Texture parameters
            if (ConfigMaster.shaderLevel > 0)
                effect.Parameters["ModelTexture"].SetValue(texture);

            // Diffuse lighting paremeters
            if (ConfigMaster.shaderLevel < -1 || ConfigMaster.shaderLevel > 1)
            {
                effect.Parameters["DiffuseColor"].SetValue(diffuseColor);
                effect.Parameters["DiffuseIntensity"].SetValue(diffuseIntensity);
                effect.Parameters["DiffuseLightDirection"].SetValue(diffuseDirection);
                Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(world));
                effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
            }

            // Specular (test)
            if (ConfigMaster.shaderLevel < -2 || ConfigMaster.shaderLevel > 2)
            {
                effect.Parameters["Shininess"].SetValue(shininess);
                effect.Parameters["SpecularColor"].SetValue(specularColor);
                effect.Parameters["SpecularIntensity"].SetValue(specularIntensity);

                viewVector.Normalize();
                effect.Parameters["ViewVector"].SetValue(viewVector);

            }
        }
    }
}
