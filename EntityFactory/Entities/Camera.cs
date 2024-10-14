using EntityFactory.Components.Input;
using EntityFactory.Components.Positioning;
using Frikandisland.Systems;

namespace EntityFactory.Entities
{
    class Camera : Entity
    {
        public Camera(PositionComponent leader) : base("camera")
        {
            CameraPosition positioner = new CameraPosition(this,leader);
            CameraController controller = new CameraController(this, positioner);
            EntitySystem.Camera = positioner;
        }
    }
}
