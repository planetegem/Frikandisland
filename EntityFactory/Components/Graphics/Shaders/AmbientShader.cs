using EntityFactory.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFactory.EntityFactory.Components.Graphics.Shaders
{
    internal class AmbientShader : ShaderComponent
    {
        public AmbientShader(Entity parent) : base(parent)
        {
            this.SetEffect("ambient");
        }

        public Vector4 ambientColor = new Vector4(0.5f, 0.5f, 0.5f, 0.5f);
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
        }
    }
}
