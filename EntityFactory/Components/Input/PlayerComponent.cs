using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using EntityFactory.Components.Positioning;
using EntityFactory.Entities;
using EntityFactory.Components.State;

namespace EntityFactory.Components.Input
{
    // SimpleKeyBoard: rotate left & right controlled by keyboard; no mouse = no strafing
    internal class SimpleKeyboard : InputComponent
    {
        private EntityBrain brain;

        public SimpleKeyboard(Entity parent, EntityBrain brain) : base(parent) 
        { 
            this.brain = brain;
        }

        // Parameters that adjust the feel of input
        private float accelerationRate = 0.0025f;
        private float inertia = 0f;
        private float turnRate = 0.1f;
        private float maxSpeed = 0.03f;

        // Override customizable parameters
        public void SetParameters(float accelerationRate, float inertia, float turnRate, float maxSpeed)
        {
            this.accelerationRate = accelerationRate;
            this.inertia = inertia;
            this.turnRate = turnRate;
            this.maxSpeed = maxSpeed;
        }

        // Update function called during input phase
        public override void Update(GameTime gt)
        {
            if (positioner == null) throw new Exception($"Error on {parent.id}: link positioner to input component!");

            // Get keyboard state
            KeyboardState keyboard = Keyboard.GetState();

            // Prepare variables
            positioner.ProposedAngle = positioner.Angle;
            Vector2 momentum = positioner.Momentum;

            // Left and right turning behavior
            if (keyboard.IsKeyDown(Keys.Left))
                positioner.ProposedAngle += turnRate;

            if (keyboard.IsKeyDown(Keys.Right))
                positioner.ProposedAngle -= turnRate;

            brain.state = EntityStates.idle;

            float maxForwardSpeed = maxSpeed;
            // Check for forwards or backwards movement
            int direction = 0;
            if (keyboard.IsKeyDown(Keys.Down))
            {
                direction = -1;
                brain.state = EntityStates.backtracking;
            }
            else if (keyboard.IsKeyDown(Keys.Up))
            {
                direction = 1;
                if (keyboard.IsKeyDown(Keys.LeftShift))
                {
                    brain.state = EntityStates.running;
                    maxForwardSpeed = maxSpeed * 2.5f;
                } 
                else
                {
                    brain.state = EntityStates.walking;
                }
            }
            float forwardMomentum = direction * accelerationRate + momentum.Y;

            // If not moving forward or backward, apply inertia
            if (!keyboard.IsKeyDown(Keys.Up) && !keyboard.IsKeyDown(Keys.Down))
            {
                forwardMomentum *= inertia;
            }
            // Limit max momentum, round down to approach 0
            forwardMomentum = Math.Clamp(forwardMomentum, -maxSpeed, maxForwardSpeed);
            forwardMomentum = (float)Math.Round(forwardMomentum, 5);

            // Apply momentum to positioning component
            positioner.Momentum = new Vector2(0, forwardMomentum);

            // Propose new position to positioner
            float newX = (float)Math.Cos(positioner.Angle) * momentum.Y + positioner.X;
            float newY = (float)Math.Sin(positioner.Angle) * momentum.Y + positioner.Y;
            positioner.ProposedPosition = new Vector2(newX, newY);
        }
    }
}
