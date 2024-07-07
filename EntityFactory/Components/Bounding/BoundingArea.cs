using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace EntityFactory.Components.Bounding
{
    internal abstract class BoundingArea
    {
        // Coordinates indicate the center of the bounding box
        // X & Y can only be set by setting the Position Vector
        private float x;
        public float X { get { return x; } }
        private float y;
        public float Y { get { return y; } }
        public Vector2 Position
        {
            get => new Vector2(X, Y);
            set { x = value.X; y = value.Y; }
        }

        // Position relative to the center of the entity
        public Vector2 BasePosition { get; set; }

        // Update true position of bounding area
        public void UpdatePosition(float angle, Vector2 origin)
        {
            float sin = (float)Math.Sin(angle);
            float cos = (float)Math.Cos(angle);

            float xPos = origin.X + BasePosition.X * sin + BasePosition.Y * cos;
            float yPos = origin.Y + BasePosition.Y * sin + BasePosition.X * -cos;

            Position = new Vector2(xPos, yPos);
        }

        // Width & Height are used for square/rectangular bounding shapes, radius is used in circular bounding shapes
        public float Width { get; set; }
        public float Height { get; set; }
        public float Radius { get; set; }

        // Implemented for specific shapes
        public abstract bool DetectCollision(List<BoundingArea> targets);

        // Collision detection for circle against circle:
        // compare distance between centerpoints against sum of radii
        public static bool CircleToCircle(float cx1, float cy1, float cx2, float cy2, float r1, float r2)
        {
            float distX = cx1 - cx2; float distY = cy1 - cy2;
            float distance = (float)Math.Sqrt(distX * distX + distY * distY);

            return distance < r1 + r2;
        }

        // Collision detection for circle against orthogonal square
        // Find closest of corner of square to circle center point & compare distance with radius
        public static bool CircleToSquare(float x1, float x2, float y1, float y2, float r, float cx, float cy)
        {
            float closestX = Math.Clamp(cx, x1, x2);
            float closestY = Math.Clamp(cy, y1, y2);
            float distX = cx - closestX; float distY = cy - closestY;
            float distance = (float)Math.Sqrt(distX * distX + distY * distY);

            return distance < r;
        }

        // Basic orthogonal collision detection
        public static bool SquareToSquare(float sx1, float sx2, float sy1, float sy2, float px1, float px2, float py1, float py2)
        {
            return px1 < sx2 && px2 > sx1 && py1 < sy2 && py2 > sy1;
        }

    }

    // Circular bounding box: used for entities
    internal class BoundingCircle : BoundingArea
    {
        public BoundingCircle(Vector2 position, float radius)
        {
            BasePosition = position;
            Position = position;
            Radius = radius;
        }
        public override bool DetectCollision(List<BoundingArea> targets)
        {
            foreach (var target in targets)
            {
                bool check = false;
                if (target is BoundingCircle)
                {
                    check = CircleToCircle(X, Y, target.X, target.Y, Radius, target.Radius);
                }
                else if (target is BoundingOrthogonalSquare)
                {
                    check = CircleToSquare(target.X - target.Width / 2, target.X + target.Width / 2, target.Y - target.Height / 2, target.Y + target.Height / 2, Radius, X, Y);
                }
                // If collision detected, return early
                if (check) { return true; }
            }
            return false;
        }
    }

    // Orthogonal rectangle: sides are always aligned with axes of coordinate field
    // Standard bounding box for map tiles
    internal class BoundingOrthogonalSquare : BoundingArea
    {
        // Pass a radius if you are making a square
        public BoundingOrthogonalSquare(Vector2 position, float radius)
        {
            BasePosition = position;
            Position = position;
            Width = radius * 2;
            Height = radius * 2;
        }

        // Otherwise, pass width & height seperately
        public BoundingOrthogonalSquare(Vector2 position, float width, float height)
        {
            BasePosition = position;
            Position = position;
            Width = width;
            Height = height;
        }

        public override bool DetectCollision(List<BoundingArea> targets)
        {
            foreach (var target in targets)
            {
                bool check = false;
                if (target is BoundingCircle)
                {
                    check = CircleToSquare(X - Width / 2, X + Width / 2, Y - Height / 2, Y + Height / 2, target.Radius, target.X, target.Y);
                }
                else if (target is BoundingOrthogonalSquare)
                {
                    check = SquareToSquare(X - Width / 2, X + Width / 2, Y - Height / 2, Y + Height / 2, target.X - target.Width / 2, target.X + target.Width / 2, target.Y - target.Height / 2, target.Y + target.Height / 2);
                }
                // If collision detected, return early
                if (check) { return true; }
            }
            return false;
        }
    }
}
