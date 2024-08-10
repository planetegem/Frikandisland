using EntityFactory.Components;
using EntityFactory.Components.Bounding;
using EntityFactory.Components.Graphics;
using EntityFactory.Components.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace EntityFactory.Systems
{
    // EntitySystem keeps track of all components and handles their execution
    // Uses singleton pattern
    internal sealed class EntitySystem
    {
        // Constructor is private to hide it
        private EntitySystem() 
        { 
            renderComponents = new List<RenderComponent>();
            animationComponents = new List<AnimationComponent>();
            inputComponents = new List<InputComponent>();
            boundingComponents = new List<BoundingComponent>();
            positionComponents = new List<PositionComponent>();
        }

        // Instance property & padlock for thread safety
        private static EntitySystem instance = null;
        private static readonly object instanceLock = new object();

        // Static method to call or create instance: done lazily, with thread safety
        public static EntitySystem getInstance()
        {
            if (instance == null)
            {
                lock (instanceLock)
                {
                    if (instance == null)
                        instance = new EntitySystem();      
                }
            }
            return instance;
        }

        // Register components
        private List<RenderComponent> renderComponents;
        private List<AnimationComponent> animationComponents;
        private List<InputComponent> inputComponents;
        private List<BoundingComponent> boundingComponents;
        private List<PositionComponent> positionComponents;

        public static void RegisterComponent(Component[] components)
        {
            if (instance is null)
                throw new Exception("Invalid instance of EntitySystem: set instance of EntitySystem during game start");
          
            foreach (var component in components)
            {
                RegisterComponent(component);
            }
        }
        public static void RegisterComponent(Component component)
        {
            if (instance is null)
                throw new Exception("Invalid instance of EntitySystem: set instance of EntitySystem during game start");

            if (component is RenderComponent)            
                instance.renderComponents.Add((RenderComponent)component);
            if (component is AnimationComponent)
                instance.animationComponents.Add((AnimationComponent)component);
            if (component is InputComponent)
                instance.inputComponents.Add((InputComponent)component);
            if (component is BoundingComponent)
                instance.boundingComponents.Add((BoundingComponent)component);
            if (component is PositionComponent)
                instance.positionComponents.Add((PositionComponent)component);
        }

        // Phase 1: input components propose changes to position components
        public static void ProcessInput(GameTime gameTime)
        {
            if (instance is null)
                throw new Exception("Invalid instance of EntitySystem: set instance of EntitySystem during game start");
            
            foreach (InputComponent component in instance.inputComponents)
            {
                try
                {
                    component.Update(gameTime);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine($"Error on entity {component.getParentId()}: {e}");
                }
            }
        }
        // Phase 2: Bounding components check proposed positions for collision detection and resolve position
        public static void ResolvePosition(GameTime gameTime, List<BoundingArea> neighbours)
        {
            if (instance is null)
                throw new Exception("Invalid instance of EntitySystem: set instance of EntitySystem during game start");

            // First check all bounding components
            foreach (BoundingComponent component in instance.boundingComponents)
            {
                try
                {
                    component.ResolvePosition(neighbours);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine($"Error on entity {component.getParentId()}: {e}");
                }
            }
            // Next go through all position components and resolve proposed changes as well (for entities without bounding component)
            foreach (PositionComponent component in instance.positionComponents)
            {
                component.ResolvePosition();
            }

        }
        // Phase 3: animation components update bone positions of model (based on entity state)
        public static void Animate(GameTime gt)
        {
            if (instance is null)
                throw new Exception("Invalid instance of EntitySystem: set instance of EntitySystem during game start");

            foreach (AnimationComponent component in instance.animationComponents)
            {
                try
                {
                    component.Update(gt);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine($"Error on entity {component.getParentId()}: {e}");
                }
            }
        }
        // Phase 4: Render components are fired to draw entity in viewport
        public static void Render(Matrix projection, Matrix view, bool debug)
        {
            if (instance is null)
                throw new Exception("Invalid instance of EntitySystem: set instance of EntitySystem during game start");
            
            foreach(RenderComponent component in instance.renderComponents)
            {
                try { 
                    component.Draw(projection, view);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine($"Error on entity {component.getParentId()}: {e}");
                }
            }

            if (debug)
            {
                foreach(BoundingComponent component in instance.boundingComponents)
                {
                    try
                    {
                        component.RenderBounds(projection, view);
                    }
                    catch (Exception e)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error on entity {component.getParentId()}: {e}");
                    }
                }
            }
        }
    }
}
