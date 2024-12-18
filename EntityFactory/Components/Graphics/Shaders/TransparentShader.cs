﻿using EntityFactory.Entities;
using Microsoft.Xna.Framework;

namespace EntityFactory.Components.Graphics
{
    public class TransparentShader : ShaderComponent
    {
        public TransparentShader(string parent) : base(parent)
        {
            this.SetEffect("transparent");
            this.transparent = true;
        }

        public Vector4 color = new Vector4(0.5f, 0.5f, 0.5f, 1f);
        public float intensity = 0.1f;

        public override void SetParameters(Matrix world, Matrix view, Vector3 viewVector, Matrix projection)
        {
            // Base paramters
            effect.Parameters["World"].SetValue(world);
            effect.Parameters["View"].SetValue(view);
            effect.Parameters["Projection"].SetValue(projection);

            // Light
            effect.Parameters["Color"].SetValue(color);
            effect.Parameters["Intensity"].SetValue(intensity);
        }
    }
}
