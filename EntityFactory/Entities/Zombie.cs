using Microsoft.Xna.Framework;
using EntityFactory.Components.Positioning;
using EntityFactory.Components.Graphics;
using EntityFactory.Components.Input;
using Frikandisland.Utilities;
using EntityFactory.Components.Graphics.Shaders;
using EntityFactory.Components.State;

namespace EntityFactory.Entities
{
    internal class Zombie : Entity
    {
        public Zombie(float x = 0f, float y = 0f) : base("zombie")
        {
            EntityBrain brain = new EntityBrain(this);

            // Step 1: set position component
            Vector2 startPos = new Vector2(x, y);
            PositionComponent positioner = new PositionComponent(this, startPos);

            // Step 2: set renderer (and if animated, animation names)
            AnimatedModel renderer = new AnimatedModel(this, positioner, "zombie");
            renderer.Animator.Dictionary = new AnimationDictionary("Armature|ArmatureAction");
            renderer.Animator.Brain = brain;
            renderer.Shader = new RiemerShader(this);

            // Step 3: set input & bounding
            BoundingArea[] bBoxes = new BoundingArea[]{
                new BoundingCircle(new Vector2(0, 0), 0.25f)
                };
            BoundingComponent bounder = new BoundingComponent(this, positioner, bBoxes);
            SimpleKeyboard inputer = new SimpleKeyboard(this, brain);
            inputer.Positioner = positioner;

        }
    }
}
