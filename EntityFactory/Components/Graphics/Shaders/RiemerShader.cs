using EntityFactory.Entities;
using EntityFactory.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFactory.Components.Graphics.Shaders
{
    internal class RiemerShader : ShaderComponent
    {
        private Texture2D texture;
        public string Texture 
        { set
            {
                texture = AssetLoader.GetTexture(value);
            } 
        }


        public RiemerShader(string parent) : base(parent)
        {
            this.SetEffect("riemer");
            Texture = "rossem";
        }
        public override void SetParameters(Matrix world, Matrix view, Vector3 viewVector, Matrix projection)
        {
            effect.CurrentTechnique = effect.Techniques["Simplest"];
            effect.Parameters["xViewProjection"].SetValue(world * view * projection);
            
            effect.Parameters["xWorld"].SetValue(world);
            effect.Parameters["xTexture"].SetValue(texture);
            effect.Parameters["xLightPos"].SetValue(new Vector3(-10f, -10f, 10f));
            effect.Parameters["xLightPower"].SetValue(1f);
            effect.Parameters["xAmbient"].SetValue(0.4f);
        }
    }
}
