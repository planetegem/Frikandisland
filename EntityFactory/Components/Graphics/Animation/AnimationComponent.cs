using Aether.Animation;
using EntityFactory.Components.State;
using EntityFactory.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace EntityFactory.Components.Graphics
{
    // AnimationComponent is constructed by AnimatedModel (extension of RenderComponent)
    // Responsible for updating bone positions on model
    internal class AnimationComponent : Component
    {
        // Dictionary of animations (loaded from fbx)
        private Animations animations;

        // State props (save previous state for comparison)
        private EntityStates animationState;

        private EntityBrain brain;
        public EntityBrain Brain { set { brain = value; } }

        // Set AnimationDictionary to save names of different animations
        private AnimationDictionary dictionary;
        public AnimationDictionary Dictionary
        {
            set
            {
                dictionary = value;
                SetAnimation(dictionary.idle);
            }
        }

        // Constructor: extract animations from model
        public AnimationComponent(string parent, Model model) : base(parent)
        {
            animations = model.GetAnimations();
        }

        // Method to set current animation
        public void SetAnimation(string animationName)
        {
            var clip = animations.Clips[animationName];
            animations.SetClip(clip);
        }

        // Check which animation should be selected and move it forward
        // (executed during animation phase; prior to render phase)
        public void Update(GameTime gt)
        {
            // First check if animations have been named
            if (animations == null || dictionary.idle == null)
                throw new Exception($"Warning: animations for {parent} were not set");

            // Check if animation needs to be changed
            if (animationState != brain.state)
            {
                animationState = brain.state;
                switch (animationState)
                {
                    case EntityStates.walking:
                        SetAnimation(dictionary.walking);
                        break;
                    case EntityStates.backtracking:
                        SetAnimation(dictionary.backtracking);
                        break;
                    case EntityStates.running:
                        SetAnimation(dictionary.running);
                        break;
                    default:
                        SetAnimation(dictionary.idle);
                        break;
                }
            }
            animations.Update(gt.ElapsedGameTime * 1.5f, true, Matrix.Identity);
        }

        // Update vertices to new animation state: called during render phase
        public void ApplyUpdateToRender(ModelMeshPart part)
        {
            // First check if animations have been named
            if (animations == null || dictionary.idle == null)
                throw new Exception($"Warning: animations for {parent} were not set");

            part.UpdateVertices(animations.AnimationTransforms);
        }
    }
}
