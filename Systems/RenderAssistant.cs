
using EntityFactory.Components.Graphics;
using EntityFactory.Components.Positioning;
using EntityFactory.Entities;
using Frikandisland.Systems;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace EntityFactory.Systems
{
    internal class RenderAssistant
    {
        // Constructor
        public RenderAssistant() { }

        // Camera logic
        private CameraPosition camera;
        public CameraPosition Camera
        {
            get 
            {
                if (camera == null)
                {
                    camera = new Camera().Construct();
                }
                return camera; 
            }
        }
        public void SetFocalPoint(PositionComponent leader)
        {
            camera.Leader = leader;
        }

        // Logic to register components
        private List<RenderComponent> renderComponents = new List<RenderComponent>();
        private List<AnimationComponent> animationComponents = new List<AnimationComponent>();

        public void Register(RenderComponent component)
        {
            renderComponents.Add(component);
        }
        public void Register(AnimationComponent component)
        {
            animationComponents.Add(component);
        }

        // Execution logic: Animation phase
        public void Animate(GameTime gt)
        {
            foreach (AnimationComponent component in animationComponents)
            {
                try
                {
                    component.Update(gt);
                }
                catch (Exception e)
                {
                    FrikanLogger.Write($"Error on entity {component.parent}: {e}");
                }
            }
        }

        // Execution logic: render phase
        public void Render(Matrix projection)
        {
            // First pass
            foreach (RenderComponent component in renderComponents)
            {
                try
                {
                    if (component.Shader != null && !component.Shader.Transparent)
                        component.Draw(projection, camera.View, camera.ViewVector);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine($"Error on entity {component.parent}: {e}");
                }
            }

            // Second pass (for transparent components)
            foreach (RenderComponent component in renderComponents)
            {
                try
                {
                    if (component.Shader != null && component.Shader.Transparent)
                        component.Draw(projection, camera.View, camera.ViewVector);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine($"Error on entity {component.parent}: {e}");
                }
            }
        }
    }
}
