using Microsoft.Xna.Framework;
using EntityFactory.Entities;

namespace EntityFactory.Components.Positioning
{
    // PositionComponent keeps track of position and rotation of an entity
    class PositionComponent : Component
    {
        // Positioning of component on the 2D field
        private Vector2 position;
        public Vector2 Position
        {
            get
            {
                return position;
            }
            // Updating position is private
            // To update position: update ProposedPosition & then call Resolve method
            private set
            {
                position = value;
                x = value.X; y = value.Y;
            }
        }

        // X & Y coordinates are available as shorthand
        private float x;
        private float y;
        public float X { get { return x; } }
        public float Y { get { return y; } }

        // Angle: like position, updating is private
        // To update: use ProposedAngle and then call Resolve method
        private float angle;
        public float Angle { get { return angle; } }

        // Proposed changes to position
        // Resolve is called every tick by System
        public Vector2 ProposedPosition { get; set; }
        public float ProposedAngle { get; set; }
        public void ResolvePosition()
        {
            Position = ProposedPosition;
            angle = ProposedAngle;
        }

        // Offset on Z-axis: default to 0, can be manually corrected
        public float OffsetZ { get; set; }

        // Momentum is tracked as a separate vector: only as fallback; not really used in determining position
        public Vector2 Momentum { get; set; }

        // Multiple constructors possible:
        // 1. No positioning, everything to default
        public PositionComponent(Entity parent) : base(parent)
        {
            OffsetZ = 0f;
            Momentum = Vector2.Zero;

            ProposedPosition = Vector2.Zero;
            ProposedAngle = 0f;
            ResolvePosition();
        }
        // 2. With position, angle optional
        public PositionComponent(Entity parent, Vector2 position, float angle = 0f) : base(parent)
        {
            OffsetZ = 0f;
            Momentum = Vector2.Zero;

            ProposedPosition = position;
            ProposedAngle = angle;
            ResolvePosition();
        }
        // 3. Only with angle
        public PositionComponent(Entity parent, float angle) : base(parent)
        {
            OffsetZ = 0f;
            Momentum = Vector2.Zero;

            ProposedPosition = Vector2.Zero;
            ProposedAngle = angle;
            ResolvePosition();
        }
    }
}
