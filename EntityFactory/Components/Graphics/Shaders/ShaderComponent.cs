using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using EntityFactory.Systems;
using Frikandisland.Systems;


namespace EntityFactory.Components.Graphics
{
    public abstract class ShaderComponent : Component
    {
        protected Effect effect = null;
        public Effect Effect { get { return effect; } }

        protected bool transparent = false;
        public bool Transparent { get { return transparent; } }

        public void SetEffect(string effectName)
        {
            try
            {
                effect = AssetLoader.GetEffect(effectName);
            }
            catch (Exception e)
            {
                FrikanLogger.Write($"Could not set shader {effectName} for {parent}: {e}");
                FrikanLogger.Write($"Shader for for {parent} was not changed.");
            }
        }
        protected ShaderComponent(string parent) : base(parent) { }
        public abstract void SetParameters(Matrix world, Matrix view, Vector3 viewVector, Matrix projection);
    }
}
