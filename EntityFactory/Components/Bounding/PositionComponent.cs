using Microsoft.Xna.Framework;
using EntityFactory.Entities;

namespace EntityFactory.Components.Bounding
{
    // PositionComponent keeps track of position and rotation of an entity
    // Shares data with MotorComponent & RenderComponent
    internal class PositionComponent : Component
    {
        // Positioning of component on the 2D field
        // Primarily saved as Vector2 & rotation
        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set
            {
                position = value;
                x = value.X; y = value.Y;
                ProposedPosition = value;
            }
        }
        public float Angle { get; set; }

        // Also keep separate x & y coordinates as shorthand
        private float x;
        private float y;
        public float X { get { return x; } }
        public float Y { get { return y; } }

        // Offset on Z-axis: default to 0, can be manually corrected
        public float OffsetZ { get; set; }

        // Proposed changes to position
        public Vector2 Momentum { get; set; }
        public Vector2 ProposedPosition { get; set; }
        public float ProposedAngle { get; set; }

        public void Resolve()
        {
            position = ProposedPosition;
            Angle = ProposedAngle;
        }

        // Multiple constructors possible:
        // 1. No positioning, everything to default
        public PositionComponent(Entity parent) : base(parent)
        {
            OffsetZ = 0f;
            Position = Vector2.Zero;
            Angle = 0f;
        }
        // 2. With position, angle optional
        public PositionComponent(Entity parent, Vector2 position, float angle = 0f) : base(parent)
        {
            OffsetZ = 0f;
            Position = position;
            Angle = angle;
        }
        // 3. Only with angle
        public PositionComponent(Entity parent, float angle) : base(parent)
        {
            OffsetZ = 0f;
            Position = Vector2.Zero;
            Angle = angle;
        }
    }
}
