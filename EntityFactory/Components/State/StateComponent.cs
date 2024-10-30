
using EntityFactory.Entities;

namespace EntityFactory.Components.State
{
    public abstract class StateComponent : Component
    {
        // Will contain entity destruction logic
        public StateComponent(Entity parent) : base(parent) { }

    }
}
