using EntityFactory.Components.Graphics;
using EntityFactory.Components.Graphics.Shaders;
using EntityFactory.Components.Input;
using EntityFactory.Components.Positioning;
using EntityFactory.Components.State;
using Frikandisland.Systems;
using Frikandisland.Utilities;
using Microsoft.Xna.Framework;

namespace EntityFactory.Entities
{
    internal class Rossem : Entity
    {
        public Rossem(float x = 0f, float y = 0f) : base("rossem")
        {
            EntityBrain brain = new EntityBrain(this);

            // Step 1: set position component
            Vector2 startPos = new Vector2(x, y);
            PositionComponent positioner = new PositionComponent(this, startPos);
            EntitySystem.AssignLeader(positioner);

            // Step 2: bounding & input
            BoundingArea[] bBoxes = new BoundingArea[]{
                new BoundingCircle(new Vector2(0, 0), 0.25f)
            };
            BoundingComponent bounder = new BoundingComponent(this, positioner, bBoxes);
            SimpleKeyboard inputer = new SimpleKeyboard(this, brain);
            inputer.Positioner = positioner;

            // Step 3: renderer
            AnimatedModel renderer = new AnimatedModel(this, positioner, "rossem");
            renderer.Animator.Dictionary = new AnimationDictionary("Armature|Idle", "Armature|Walk", "Armature|Backtrack", "Armature|Run");
            renderer.Shader = new TransparentShader(this);
            renderer.Animator.Brain = brain;

        }
    }
}
