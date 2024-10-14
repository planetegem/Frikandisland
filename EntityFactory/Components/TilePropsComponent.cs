using EntityFactory.Components;
using EntityFactory.Components.Positioning;
using EntityFactory.Entities;
using Frikandisland.Utilities;


namespace EntityFactory.EntityFactory.Components
{
    internal class TilePropsComponent : Component
    {
        // Positioning
        private PositionComponent positioner;
        public readonly BoundingOrthogonalSquare bounds;

        // Collision enabled
        public bool collision;

        public TilePropsComponent(Entity parent, PositionComponent positioner, bool collision) : base(parent)
        {
            this.positioner = positioner;
            this.bounds = new BoundingOrthogonalSquare(positioner.Position, 0.5f);
            this.collision = collision;
        }

    }
}
