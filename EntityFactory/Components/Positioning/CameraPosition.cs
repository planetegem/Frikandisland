using EntityFactory.Entities;
using Microsoft.Xna.Framework;
using System;

namespace EntityFactory.Components.Positioning
{
    public class CameraPosition : Component
    {
        // Constructor: set leader as basic positon component
        public CameraPosition(string parent) : base(parent)
        {
            this.leader = new PositionComponent(parent);
        }

        // Look-at point = position of player
        private PositionComponent leader;
        public PositionComponent Leader
        {
            set { this.leader = value; }
        }

        // Angle (on the horizontal plane)
        private float angle = MathHelper.ToRadians(90f);
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }

        // Pivot (vertical plane): limited to range between 10 & 80 degrees
        private float pivot = MathHelper.ToRadians(45f);
        private float minPivot = 0.1745f;
        private float maxPivot = 1.3962f;
        public float Pivot 
        { 
            get { return pivot; }
            set
            { 
                this.pivot = Math.Clamp(value, minPivot, maxPivot);
            } 
        }

        // Magnification - determines distance from look-at-point
        private float magnification = 1f;
        private float minMagnification = 0.3f;
        private float maxMagnification = 5f;
        public float Magnification
        {
            get { return magnification; }
            set
            {
                magnification = Math.Clamp(value, minMagnification, maxMagnification);
            }
        }

        // Base distance: multiplied with magnification for real distance to look-at-point
        private float distance = 10f;

        // Camera Location: saved seperately for use in specular lighting shader
        public Vector3 CameraLocation
        {
            get
            {
                float xPos = (float)Math.Sin(angle) * magnification * distance * (float)Math.Sin(pivot) + leader.X;
                float yPos = (float)Math.Cos(angle) * magnification * distance * (float)Math.Sin(pivot) + leader.Y;
                float zPos = (float)Math.Cos(pivot) * magnification * distance;

                return new Vector3(xPos, yPos, zPos);
            }
        }
        // Camera Lookat: position of leader, z-pos 0
        public Vector3 CameraLookAt
        {
            get
            {
                return new Vector3(leader.Position, 1f);
            }
        }

        // Return final matrix
        public Matrix View 
        { 
            get 
            {
                return Matrix.CreateLookAt(CameraLocation, CameraLookAt, Vector3.UnitZ); ; 
            } 
        }
    }
}
