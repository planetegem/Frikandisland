namespace EntityFactory.Entities
{
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

        public Entity(string id, EntityStates state = default(EntityStates))
        {
            this.id = id;
            this.state = state;
        }
    }
}
