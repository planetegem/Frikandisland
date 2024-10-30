using EntityFactory.Components;
using EntityFactory.Components.Positioning;
using EntityFactory.Components.Graphics;
using EntityFactory.Components.Input;
using Frikandisland.Utilities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using EntityFactory.Systems;
using EntityFactory.Components.State;

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
                    {
                        instance = new EntitySystem();
                        CameraPosition cam = instance.renderAssistant.Camera;
                    }
                }
            }
            return instance;
        }

        // Constructor is private to hide it
        private EntitySystem() 
        {
            renderAssistant = new RenderAssistant();
        }

        // Player & camera props
        public static void AssignLeader(PositionComponent leader)
        {
            if (instance is null)
                throw new Exception("Invalid instance of EntitySystem: tried to initialize camera without system");

            instance.renderAssistant.SetFocalPoint(leader);
        }

        // Register components
        private List<InputComponent> inputComponents = new List<InputComponent>();
        private List<BoundingComponent> boundingComponents = new List<BoundingComponent>();
        private List<PositionComponent> positionComponents = new List<PositionComponent>();
        private List<TileProps> tilePropsComponents = new List<TileProps>();

        private RenderAssistant renderAssistant;

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
                instance.renderAssistant.Register((RenderComponent)component);
            if (component is AnimationComponent)
                instance.renderAssistant.Register((AnimationComponent)component);
                
            if (component is InputComponent)
                instance.inputComponents.Add((InputComponent)component);
            if (component is BoundingComponent)
                instance.boundingComponents.Add((BoundingComponent)component);
            if (component is PositionComponent)
                instance.positionComponents.Add((PositionComponent)component);
            if (component is TileProps)
                instance.tilePropsComponents.Add((TileProps)component);
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
            foreach (TileProps tile in instance.tilePropsComponents)
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

            instance.renderAssistant.Animate(gt);
        }

        // Phase 4: Render components are fired to draw entity in viewport
        public static void Render(Matrix projection, bool debug)
        {
            if (instance is null)
                throw new Exception("Invalid instance of EntitySystem: set instance of EntitySystem during game start");

            instance.renderAssistant.Render(projection);

            if (debug)
            {
                foreach(BoundingComponent component in instance.boundingComponents)
                {
                    try
                    {
                        component.RenderBounds(projection, instance.renderAssistant.Camera.View);
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
