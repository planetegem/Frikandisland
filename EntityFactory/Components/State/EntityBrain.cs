
using EntityFactory.Entities;

namespace EntityFactory.Components.State
{
    public enum EntityStates
    {
        idle = 0,
        backtracking = 1,
        walking = 2,
        running = 3
    }
    public class EntityBrain : StateComponent
    {
        public EntityStates state;

        public EntityBrain(string parent, EntityStates state = default) : base(parent) 
        {
            this.state = state;
        }
    }
}
