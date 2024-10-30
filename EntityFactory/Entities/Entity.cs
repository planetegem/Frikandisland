using System;

namespace EntityFactory.Entities
{
    // Entity state is currently tracked in the entity itself because of laziness
    // Later on, this will probably be moved to a component specialized in tracking state

    public abstract class Entity
    {
        public readonly string id;

        // Constructor: apply name to entity
        public Entity(string id)
        {
            try
            {
                this.id = id + entityCount;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                this.id = id;
            }
        }

        // Count amount of entities created (part of naming scheme)
        private static int entityCount = 0;

    }
}
