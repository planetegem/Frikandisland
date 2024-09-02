using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;


namespace EntityFactory.Systems
{
    internal class Camera
    {
        // Positioning fields
        private float cameraAngle;
        private float cameraPivot;
        private float cameraMagnification;
        private float cameraBaseDistance;

        // Comparison fields
        private int startX;
        private int startY;
        private float previousAngle;
        private float previousPivot;
        private MouseState oldState;
        private MouseState newState;

        // Final matrix
        private Matrix view;
        public Matrix View { get { return view; } }

        public Camera()
        {
            cameraAngle = MathHelper.ToRadians(180f);
            cameraPivot = MathHelper.ToRadians(45f);
            cameraMagnification = 1f;
            cameraBaseDistance = 10f;

            // Mouse interaction: save old mouse state to figure out when state changes
            oldState = Mouse.GetState();
        }

        public void Update(Vector2 pos)
        {
            newState = Mouse.GetState();

            // On click, save x coordinate & current camera angle
            if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
            {
                startX = newState.X;
                previousAngle = cameraAngle;

                startY = newState.Y;
                previousPivot = cameraPivot;
            }

            // If dragging mouse, update camera angle
            if (newState.LeftButton == ButtonState.Pressed)
            {
                int diffX = startX - newState.X;
                cameraAngle = previousAngle + diffX * 0.01f;

                int diffY = startY - newState.Y;
                cameraPivot = previousPivot + diffY * 0.01f;
                // Limit pivot: between 10 & 80 degrees
                cameraPivot = Math.Clamp(cameraPivot, 0.1745f, 1.3962f);

            }

            // Check for differences in scroll wheel value and apply magnification
            int scrollValue = oldState.ScrollWheelValue - newState.ScrollWheelValue;

            if (scrollValue != 0)
            {
                cameraMagnification = Math.Clamp(cameraMagnification + scrollValue * 0.001f, 0.3f, 5f);
            }

            // Update camera with new angle
            float xPos = (float)Math.Sin(cameraAngle) * cameraMagnification * cameraBaseDistance * (float)Math.Sin(cameraPivot) + pos.X;
            float yPos = (float)Math.Cos(cameraAngle) * cameraMagnification * cameraBaseDistance * (float)Math.Sin(cameraPivot) + pos.Y;
            float zPos = (float)Math.Cos(cameraPivot) * cameraMagnification * cameraBaseDistance;

            view = Matrix.CreateLookAt(new Vector3(xPos, yPos, zPos), new Vector3(pos, 0), Vector3.UnitZ);

            // Save mouse state to check against next time
            oldState = newState;
        }
    }
}
