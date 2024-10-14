using EntityFactory.Entities;
using Frikandisland.Systems;

namespace EntityFactory.Components
{
    abstract class Component
    {
        // Every component keeps track of parent entity
        public readonly Entity parent;
        protected Component(Entity parent)
        {
            this.parent = parent;
            // And registers itself with the EntitySystem during construction
            EntitySystem.RegisterComponent(this);
        }

        // Parent ID can be called at any time (mainly for error messages)
        public string getParentId()
        {
            return parent.id;
        }
    }
}
