using EntityFactory.Components.Bounding;
using EntityFactory.Entities;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFactory.Components.Input
{
    // SimpleKeyBoard: rotate left & right controlled by keyboard; no mouse = no strafing
    internal class SimpleKeyboard : InputComponent
    {
        public SimpleKeyboard(Entity parent, PositionComponent positioner) : base(parent, positioner)
        {

        }

        // Parameters that adjust the feel of input
        private float accelerationRate = 0.0025f;
        private float inertia = 0f;
        private float turnRate = 0.1f;
        private float maxSpeed = 0.05f;

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

            // Check for forwards or backwards movement
            int direction = 0;
            if (keyboard.IsKeyDown(Keys.Down))
            {
                direction = -1;
            }
            else if (keyboard.IsKeyDown(Keys.Up))
            {
                direction = 1;
            }
            float forwardMomentum = direction * accelerationRate + momentum.Y;

            // If not moving forward or backward, apply inertia
            if (!keyboard.IsKeyDown(Keys.Up) && !keyboard.IsKeyDown(Keys.Down))
            {
                forwardMomentum *= inertia;
            }
            // Limit max momentum, round down to approach 0
            forwardMomentum = Math.Clamp(forwardMomentum, -maxSpeed, maxSpeed);
            forwardMomentum = (float)Math.Round(forwardMomentum, 5);

            // Apply momentum to positioning component
            positioner.Momentum = new Vector2(0, forwardMomentum);

            // Set entity state
            parent.state = forwardMomentum != 0 ? EntityStates.walking : EntityStates.idle;

            // Propose new position to positioner
            float newX = (float)Math.Cos(positioner.Angle) * momentum.Y + positioner.X;
            float newY = (float)Math.Sin(positioner.Angle) * momentum.Y + positioner.Y;
            positioner.ProposedPosition = new Vector2(newX, newY);
        }
    }
}
