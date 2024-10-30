using EntityFactory.Components.Input;
using EntityFactory.Components.Positioning;
using Frikandisland.Systems;

namespace EntityFactory.Entities
{
    class Camera : Entity
    {
        public Camera() : base("camera")
        {

        }
        public CameraPosition Construct()
        {
            CameraPosition positioner = new CameraPosition(this);
            CameraController controller = new CameraController(this, positioner);
            return positioner;
        }
    }
}
