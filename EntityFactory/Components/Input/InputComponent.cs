using EntityFactory.Components.Positioning;
using Microsoft.Xna.Framework;

namespace EntityFactory.Components.Input
{
    // Process input, either player or AI
    public abstract class InputComponent : Component
    {
        protected PositionComponent positioner;
        public PositionComponent Positioner { set { positioner = value; } }
        public InputComponent(string parent) : base(parent) { }
        public abstract void Update(GameTime gt);
    }
}
