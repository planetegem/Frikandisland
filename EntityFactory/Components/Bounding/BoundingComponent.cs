using EntityFactory.Entities;
using EntityFactory.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace EntityFactory.Components.Bounding
{
    internal class BoundingComponent : Component
    {
        private BoundingArea[] bounds;
        private PositionComponent positioner;
        
        public BoundingComponent(Entity parent, PositionComponent positioner, BoundingArea bounds) : base(parent)
        {
            this.positioner = positioner;
            this.bounds = new BoundingArea[]{bounds};
        }
        public BoundingComponent(Entity parent, PositionComponent positioner, BoundingArea[] bounds) : base(parent)
        {
            this.positioner = positioner;
            this.bounds = bounds;
        }        

        public void ResolvePosition(List<BoundingArea> neighbours)
        {
            // Prepare new values
            float newX = positioner.ProposedPosition.X;
            float newY = positioner.ProposedPosition.Y;

            // Attempt movement along X-axis
            foreach (BoundingArea bBox in bounds)
            {
                bBox.UpdatePosition(positioner.Angle, new Vector2(newX, positioner.Y));
                if (bBox.DetectCollision(neighbours))
                {
                    newX = positioner.X;
                    break;
                }
            }

            // Attempt movement along Y-axis
            foreach (BoundingArea bBox in bounds)
            {
                bBox.UpdatePosition(positioner.Angle, new Vector2(positioner.X, newY));
                if (bBox.DetectCollision(neighbours))
                {
                    newY = positioner.Y;
                    break;
                }
            }

            // Amend ProposedPosition based on collision results
            Vector2 newPos = new Vector2(newX, newY);
            positioner.ProposedPosition = newPos;
            positioner.Position = newPos;

            // Attempt rotation
            float newAngle = positioner.ProposedAngle;
            foreach (BoundingArea bBox in bounds)
            {
                bBox.UpdatePosition(newAngle, positioner.Position);
                if (bBox.DetectCollision(neighbours))
                {
                    newAngle = positioner.Angle;
                    break;
                }
            }

            // Amend ProposedAngle
            positioner.ProposedAngle = newAngle;
            positioner.Angle = newAngle;

            // Resolve everything
            foreach (BoundingArea bBox in bounds)
            {
                bBox.UpdatePosition(newAngle, newPos);
            }
        }

        // ONLY FOR DEBUG
        // 1. Render BoundingArea in 3D field
        public void RenderBounds(Matrix projection, Matrix view)
        {
            foreach (BoundingArea bound in this.bounds)
            {
                Matrix world = Matrix.Identity;
                Model model = EntityLoader.GetModel("circle");

                if (bound is BoundingCircle)
                {
                    world = Matrix.CreateScale(bound.Radius * 2);
                }
                else if (bound is BoundingOrthogonalSquare)
                {
                    world = Matrix.CreateScale(bound.Width, bound.Height, 1f);
                    model = EntityLoader.GetModel("square");
                }
                world *= Matrix.CreateTranslation(new Vector3(bound.Position, 0.01f));

                Effect shader = EntityLoader.GetEffect("ambient");

                foreach (ModelMesh mesh in model.Meshes)
                {
                    foreach (ModelMeshPart part in mesh.MeshParts)
                    {
                        part.Effect = shader;
                        shader.Parameters["World"].SetValue(world);
                        shader.Parameters["View"].SetValue(view);
                        shader.Parameters["Projection"].SetValue(projection);
                        shader.Parameters["AmbientColor"].SetValue(Color.Blue.ToVector4());
                        shader.Parameters["AmbientIntensity"].SetValue(0.8f);
                    }
                    mesh.Draw();
                }
            }
        }
        
    }
}
