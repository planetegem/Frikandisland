using EntityFactory.Components;
using EntityFactory.Entities;
using EntityFactory.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;


namespace EntityFactory.EntityFactory.Components.Graphics
{
    abstract class ShaderComponent : Component
    {
        protected Effect effect = null;
        public Effect Effect { get { return effect; } }

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
        protected ShaderComponent(Entity parent) : base(parent) {}
        public abstract void SetParameters(Matrix world, Matrix view, Matrix projection);
    }
}
