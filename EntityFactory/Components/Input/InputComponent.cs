using EntityFactory.Components.Positioning;
using EntityFactory.Entities;
using Microsoft.Xna.Framework;

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
