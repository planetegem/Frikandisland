using EntityFactory.Components.Positioning;
using EntityFactory.Entities;
using Frikandisland.Utilities;


namespace EntityFactory.Components.State
{
    internal class TileProps : StateComponent
    {
        // Positioning
        private PositionComponent positioner;
        public readonly BoundingOrthogonalSquare bounds;

        // Collision enabled
        public bool collision;

        public TileProps(string parent, PositionComponent positioner, bool collision) : base(parent)
        {
            this.positioner = positioner;
            bounds = new BoundingOrthogonalSquare(positioner.Position, 0.5f);
            this.collision = collision;
        }

    }
}
