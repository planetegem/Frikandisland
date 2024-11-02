using Microsoft.Xna.Framework;
using EntityFactory.Components.Positioning;
using EntityFactory.Components.Input;
using Frikandisland.Utilities;
using EntityFactory.Components.Graphics;
using EntityFactory.Components.State;

namespace EntityFactory.Entities
{
    internal class Percolator : Entity
    {
        public Percolator(float x = 0f, float y = 0f) : base("percolator")
        {
            EntityBrain brain = new EntityBrain(this.id);

            // Step 1: set position component
            Vector2 startPos = new Vector2(x, y);
            PositionComponent positioner = new PositionComponent(this.id, startPos);

            // Step 2: bounding & input
            BoundingArea[] bBoxes = new BoundingArea[]{
                new BoundingCircle(new Vector2(0, 0), 0.25f)
            };
            BoundingComponent bounder = new BoundingComponent(this.id, positioner, bBoxes);
            SimpleKeyboard inputer = new SimpleKeyboard(this.id, brain);
            inputer.Positioner = positioner;

            // Step 3: renderer
            SimpleModel renderer = new SimpleModel(this.id, "percolator");
            renderer.Positioner = positioner;
            renderer.EnableStandardEffect = true;

        }
    }
}
