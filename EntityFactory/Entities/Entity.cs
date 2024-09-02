using EntityFactory.Systems;
using System;

namespace EntityFactory.Entities
{
    // Entity state is currently tracked in the entity itself because of laziness
    // Later on, this will probably be moved to a component specialized in tracking state
    public enum EntityStates
    {
        idle = 0,
        standing = 1,
        walking = 2,
        running = 3
    }
    internal abstract class Entity
    {
        public readonly string id;
        public EntityStates state;

        public Entity(string id, EntityStates state = default)
        {
            try
            {
                this.id = id + AssetLoader.EntityCount;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                this.id = id;
            }
            this.state = state;
        }
    }
}
