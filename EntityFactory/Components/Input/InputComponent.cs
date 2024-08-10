using EntityFactory.Components.Bounding;
using EntityFactory.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace EntityFactory.Components.Input
{
    // Process input, either player or AI
    internal abstract class InputComponent : Component
    {
        protected PositionComponent positioner;
        public InputComponent(Entity parent, PositionComponent positioner) : base(parent)
        {
            this.positioner = positioner;
        }
        public abstract void Update(GameTime gt);
    }
}
