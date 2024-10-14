using EntityFactory.Components;
using EntityFactory.Components.Positioning;
using EntityFactory.Components.Graphics;
using EntityFactory.Components.Input;
using EntityFactory.Entities;
using Frikandisland.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using EntityFactory.EntityFactory.Components;

namespace Frikandisland.Systems
{
    // EntitySystem keeps track of all components and handles their execution
    // Uses singleton pattern
    internal sealed class EntitySystem
    {
        // SINGLETON DESIGN PATTERN
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

        // Player & camera props
        public static PositionComponent Leader 
        {
            set
            {
                if (instance is null) 
                    throw new Exception("Invalid instance of EntitySystem: tried to initialize camera without system");
                
                Camera camera = new Camera(value);
            }
        }
        private CameraPosition camera;
        public static CameraPosition Camera
        {
            set
            {
                if (instance is null) 
                    throw new Exception("Invalid instance of EntitySystem: tried to initialize camera without system");
                
                instance.camera = value;
            }
        }

        // Constructor is private to hide it
        private EntitySystem() { }

        // Register components
        private List<RenderComponent> renderComponents = new List<RenderComponent>();
        private List<AnimationComponent> animationComponents = new List<AnimationComponent>();
        private List<InputComponent> inputComponents = new List<InputComponent>();
        private List<BoundingComponent> boundingComponents = new List<BoundingComponent>();
        private List<PositionComponent> positionComponents = new List<PositionComponent>();
        private List<TilePropsComponent> tilePropsComponents = new List<TilePropsComponent>();

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
            if (component is TilePropsComponent)
                instance.tilePropsComponents.Add((TilePropsComponent)component);
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
        public static void ResolvePosition(GameTime gameTime)
        {
            if (instance is null)
                throw new Exception("Invalid instance of EntitySystem: set instance of EntitySystem during game start");

            // Make list of all tiles with collision enabled
            List<BoundingArea> tiles = new List<BoundingArea>();
            foreach (TilePropsComponent tile in instance.tilePropsComponents)
            {
                if (tile.collision) tiles.Add(tile.bounds);
            }

            // First check all bounding components
            foreach (BoundingComponent component in instance.boundingComponents)
            {
                try
                {
                    component.ResolvePosition(tiles);
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
        public static void Render(Matrix projection, bool debug)
        {
            if (instance is null)
                throw new Exception("Invalid instance of EntitySystem: set instance of EntitySystem during game start");

            // First update camera
            foreach(RenderComponent component in instance.renderComponents)
            {
                try { 
                    component.Draw(projection, instance.camera.View);
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
                        component.RenderBounds(projection, instance.camera.View);
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
