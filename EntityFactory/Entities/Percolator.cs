using Microsoft.Xna.Framework;
using EntityFactory.Components.Positioning;
using EntityFactory.Components.Graphics;
using EntityFactory.Components.Input;
using Frikandisland.Utilities;

namespace EntityFactory.Entities
{
    internal class Percolator : Entity
    {
        public Percolator(float x = 0f, float y = 0f) : base("percolator")
        {
            // Step 1: set position component
            Vector2 startPos = new Vector2(x, y);
            PositionComponent positioner = new PositionComponent(this, startPos);

            // Step 2: bounding & input
            BoundingArea[] bBoxes = new BoundingArea[]{
                new BoundingCircle(new Vector2(0, 0), 0.25f)
            };
            BoundingComponent bounder = new BoundingComponent(this, positioner, bBoxes);
            SimpleKeyboard inputer = new SimpleKeyboard(this, positioner);

            // Step 3: renderer
            SimpleModel renderer = new SimpleModel(this, positioner, "percolator");
            renderer.EnableStandardEffect = true;

        }
    }
}
