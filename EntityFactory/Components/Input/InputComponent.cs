using EntityFactory.Components.Positioning;
using EntityFactory.Entities;
using Microsoft.Xna.Framework;

namespace EntityFactory.Components.Input
{
    // Process input, either player or AI
    internal abstract class InputComponent : Component
    {
        protected PositionComponent positioner;
        public PositionComponent Positioner { set { positioner = value; } }
        public InputComponent(Entity parent) : base(parent) { }
        public abstract void Update(GameTime gt);
    }
}
