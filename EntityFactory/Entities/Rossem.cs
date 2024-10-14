using EntityFactory.Components.Graphics;
using EntityFactory.Components.Input;
using EntityFactory.Components.Positioning;
using EntityFactory.Entities;
using Frikandisland.Systems;
using Frikandisland.Utilities;
using Microsoft.Xna.Framework;

namespace EntityFactory.Entities
{
    internal class Rossem : Entity
    {
        public Rossem(float x = 0f, float y = 0f) : base("rossem")
        {
            // Step 1: set position component
            Vector2 startPos = new Vector2(x, y);
            PositionComponent positioner = new PositionComponent(this, startPos);
            EntitySystem.Leader = positioner;

            // Step 2: bounding & input
            BoundingArea[] bBoxes = new BoundingArea[]{
                new BoundingCircle(new Vector2(0, 0), 0.25f)
            };
            BoundingComponent bounder = new BoundingComponent(this, positioner, bBoxes);
            SimpleKeyboard inputer = new SimpleKeyboard(this);
            inputer.Positioner = positioner;

            // Step 3: renderer
            AnimatedModel renderer = new AnimatedModel(this, positioner, "rossem");
            renderer.Animator.Dictionary = new AnimationDictionary("Armature|Idle", "Armature|Walk", "Armature|Backtrack", "Armature|Run");

        }
    }
}
