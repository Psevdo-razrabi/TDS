using Input;

namespace Game.Player.AnyScripts
{
    public class PlayerInputStorage
    {
        public InputSystemMovement InputSystem { get; private set; }
        public InputSystemMouse InputSystemMouse { get; private set; }
        public InputObserver InputObserver { get; private set; }

        public PlayerInputStorage(InputSystemMovement inputSystem, InputSystemMouse inputSystemMouse, InputObserver inputObserver)
        {
            InputSystem = inputSystem;
            InputSystemMouse = inputSystemMouse;
            InputObserver = inputObserver;
        }
    }
}