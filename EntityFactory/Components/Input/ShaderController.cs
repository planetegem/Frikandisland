using EntityFactory.Components.Input;
using Frikandisland.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace EntityFactory.EntityFactory.Components.Input
{
    public class ShaderController : InputComponent
    {
        private KeyboardState previousState = Keyboard.GetState();
        public ShaderController(string parent) : base(parent) { }

        override public void Update(GameTime gt)
        {
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.A) && previousState.IsKeyUp(Keys.A)){
                ConfigMaster.ShaderLevel--;
            }
            else if (newState.IsKeyDown(Keys.Z) && previousState.IsKeyUp(Keys.Z))
            {
                ConfigMaster.ShaderLevel++;
            }
            previousState = newState;
        }

    }
}
