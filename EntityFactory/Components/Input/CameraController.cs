
using EntityFactory.Components.Positioning;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EntityFactory.Components.Input
{
    public class CameraController : InputComponent
    {
        // Comparison fields to process camera movement
        private int startX;
        private int startY;
        private float previousAngle;
        private float previousPivot;
        private MouseState oldState;
        private MouseState newState;

        // Camera position component
        private CameraPosition camera;

        // Constructor: link to camera position + get current mouse state for comparison
        public CameraController(string parent, CameraPosition camera) : base(parent) 
        {
            this.camera = camera;
            oldState = Mouse.GetState();
        }

        public override void Update(GameTime gt)
        {
            newState = Mouse.GetState();

            // On click, save x coordinate & current camera angle
            if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
            {
                startX = newState.X;
                previousAngle = camera.Angle;

                startY = newState.Y;
                previousPivot = camera.Pivot;
            }

            // If dragging mouse, update camera angle
            if (newState.LeftButton == ButtonState.Pressed)
            {
                int diffX = startX - newState.X;
                camera.Angle = previousAngle + diffX * 0.01f;

                int diffY = startY - newState.Y;
                camera.Pivot = previousPivot + diffY * 0.01f;
            }

            // Check for differences in scroll wheel value and apply magnification
            int scrollValue = oldState.ScrollWheelValue - newState.ScrollWheelValue;

            if (scrollValue != 0)
            {
                camera.Magnification += scrollValue * 0.001f;
            }

            // Save mouse state to check against next time
            oldState = newState;
        }
    }
}
