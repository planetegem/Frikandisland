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
            EntityBrain brain = new EntityBrain(this.id);

            // Step 1: set position component
            Vector2 startPos = new Vector2(x, y);
            PositionComponent positioner = new PositionComponent(this.id, startPos);
            EntitySystem.AssignLeader(positioner);

            // Step 2: bounding & input
            BoundingArea[] bBoxes = new BoundingArea[]{
                new BoundingCircle(new Vector2(0, 0), 0.25f)
            };
            BoundingComponent bounder = new BoundingComponent(this.id, positioner, bBoxes);
            SimpleKeyboard inputer = new SimpleKeyboard(this.id, brain);
            inputer.Positioner = positioner;

            // Step 3: renderer
            AnimatedModel renderer = new AnimatedModel(this.id, "rossem");
            
            renderer.ConfigureAnimations(
                new AnimationDictionary("Armature|Idle", "Armature|Walk", "Armature|Backtrack", "Armature|Run"),
                brain
            );
            
            renderer.Shader = new MainShader(this.id, "rossem");
            renderer.Positioner = positioner;

        }
    }
}
