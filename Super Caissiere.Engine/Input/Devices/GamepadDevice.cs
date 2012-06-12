using Microsoft.Xna.Framework;
using XNAInput = Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.GamerServices;

namespace SuperCaissiere.Engine.Input.Devices
{
    public class GamepadDevice : Device
    {
        private XNAInput.GamePadState _gamepad, _previousGamepad;

        public GamepadDevice(LogicalPlayerIndex logicalPlayerIndex, PlayerIndex index)
            : base(logicalPlayerIndex, DeviceType.Gamepad, (int)index)
        { }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _gamepad = XNAInput.GamePad.GetState((PlayerIndex)Index);

            base.Update(gameTime);

            MapButtonValue(_previousGamepad.Buttons.A, _gamepad.Buttons.A, MappingButtons.A);
            MapButtonValue(_previousGamepad.Buttons.B, _gamepad.Buttons.B, MappingButtons.B);
            MapButtonValue(_previousGamepad.Buttons.X, _gamepad.Buttons.X, MappingButtons.X);
            MapButtonValue(_previousGamepad.Buttons.Y, _gamepad.Buttons.Y, MappingButtons.Y);
            MapButtonValue(_previousGamepad.Buttons.Back, _gamepad.Buttons.Back, MappingButtons.Back);
            MapButtonValue(_previousGamepad.Buttons.Start, _gamepad.Buttons.Start, MappingButtons.Start);
            MapButtonValue(_previousGamepad.Buttons.BigButton, _gamepad.Buttons.BigButton, MappingButtons.Home);
            MapButtonValue(_previousGamepad.Buttons.LeftShoulder, _gamepad.Buttons.LeftShoulder, MappingButtons.LB);
            MapButtonValue(_previousGamepad.Buttons.RightShoulder, _gamepad.Buttons.RightShoulder, MappingButtons.RB);

            // Thumbsticks
            // ndDam: no stick "clic" for now
            ThumbStickLeft.X = _gamepad.ThumbSticks.Left.X;
            ThumbStickLeft.Y = _gamepad.ThumbSticks.Left.Y;

            ThumbStickRight.X = _gamepad.ThumbSticks.Right.X;
            ThumbStickRight.Y = _gamepad.ThumbSticks.Right.Y;

            _previousGamepad = _gamepad;
        }

        private void MapButtonValue(XNAInput.ButtonState previousXnaButton, XNAInput.ButtonState xnaButton, MappingButtons button)
        {
            // Mapping
            bool isDown = false;
            bool isPressed = false;
            bool isReleased = false;

            if (xnaButton == XNAInput.ButtonState.Pressed)
            {
                isDown = true;

                if (previousXnaButton == XNAInput.ButtonState.Released)
                {
                    isPressed = true;
                }
            }
            else if (xnaButton == XNAInput.ButtonState.Released)
            {
                if (previousXnaButton == XNAInput.ButtonState.Pressed)
                {
                    isReleased = true;
                }
            }

            SetMappingValue(button, isDown, isPressed, isReleased);
        }

        public override bool IsConnected
        {
            get
            {
                return _gamepad.IsConnected;
            }
        }


        /// <summary>
        /// For a given gamepad, this function determines if there is a profile logged on it
        /// </summary>
        /// <returns></returns>
        public bool HasProfile
        {
            get
            {
#if XBOX
                return DeviceProfile(device) != null;
#else
                return false;
#endif
            }
        }

        /// <summary>
        /// For a given gamepad, this function return the logged profile on it
        /// </summary>
        /// <returns></returns>
        public SignedInGamer DeviceProfile
        {
            get
            {
#if XBOX
                if (Type == DeviceType.Gamepad)
                {
                    foreach (SignedInGamer gamer in SignedInGamer.SignedInGamers)
                    {
                        if ((int)gamer.PlayerIndex == Index)
                        {
                            return gamer;
                        }
                    }
                }
#endif
                return null;
            }
        }
    }
}
