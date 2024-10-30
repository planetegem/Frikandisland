using EntityFactory.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using EntityFactory.Systems;


namespace EntityFactory.Components.Graphics
{
    abstract class ShaderComponent : Component
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
                System.Diagnostics.Debug.WriteLine($"Could not set shader {effectName} for {parent.id}: {e}");
                System.Diagnostics.Debug.WriteLine($"Shader for for {parent.id} was not changed.");
            }
        }
        protected ShaderComponent(Entity parent) : base(parent) { }
        public abstract void SetParameters(Matrix world, Matrix view, Matrix projection);
    }
}
