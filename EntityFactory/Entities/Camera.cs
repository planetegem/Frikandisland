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
            CameraPosition positioner = new CameraPosition(this.id);
            CameraController controller = new CameraController(this.id, positioner);
            return positioner;
        }
    }
}
