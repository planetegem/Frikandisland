using EntityFactory.Entities;
using Frikandisland.Systems;

namespace EntityFactory.Components
{
    public abstract class Component
    {
        // Every component keeps track of parent entity
        public readonly string parent;
        protected Component(string parent)
        {
            this.parent = parent;
            // And registers itself with the EntitySystem during construction
            EntitySystem.RegisterComponent(this);
        }
    }
}
