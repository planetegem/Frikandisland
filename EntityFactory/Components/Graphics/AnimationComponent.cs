using Aether.Animation;
using EntityFactory.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EntityFactory.Components.Graphics
{
    internal class AnimationComponent : Component
    {
        private Model model;
        private Animations animations;

        // Control variables for animation
        private TimeSpan startTime;
        private bool animationRunning = false;

        public AnimationComponent(Entity parent, Model model) : base(parent)
        {
            this.model = model;
        }
        public void SetAnimation(string animationName)
        {
            animations = model.GetAnimations();
            var clip = animations.Clips[animationName];
            animations.SetClip(clip);
        }
        public void Update(GameTime gt)
        {
            // First check if animations have been named
            if (animations == null)
                throw new Exception($"Warning: animation for {parent.id} has not been set");

            // Temporary: clean up later
            bool moving = (parent.state == EntityStates.walking);

            // If moving, play animation
            if (moving || animationRunning)
            {
                if (!animationRunning)
                {
                    animationRunning = true;
                    startTime = TimeSpan.Zero;
                }

                animations.Update(gt.ElapsedGameTime * 3.5, true, Matrix.Identity);
                startTime += gt.ElapsedGameTime * 3.5;

                if (startTime >= animations.CurrentClip.Duration)
                {
                    animationRunning = false;
                }
            }
            // When completed, remain motionless
            else
            {
                animations.Update(TimeSpan.Zero, false, Matrix.Identity);
            }
        }

        public void ApplyUpdateToRender(ModelMeshPart part)
        {
            // First check if animations have been named
            if (animations == null)
                throw new Exception($"Warning: animation for {parent.id} has not been set");

            // Update vertices to new animation state
            part.UpdateVertices(animations.AnimationTransforms);
        }
    }
}
