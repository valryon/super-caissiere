using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SuperCaissiere.Engine.Core;

namespace SuperCaissiere.Engine.Input.Devices
{
    /// <summary>
    /// TODO Joypad support
    /// </summary>
#if WINDOWS
    public class JoypadDevice : Device
    {
        private JoystickState _joyState, _previousJoystate;

        public JoypadDevice(LogicalPlayerIndex logicalPlayerIndex, int index)
            : base(logicalPlayerIndex,DeviceType.Joystick, index)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _joyState = Application.InputManager.GetJoystickState(Index);

            // Mapping
            // TODO Joypad mapping

            // End
            _previousJoystate = _joyState;
        }

        public override bool IsConnected
        {
            get { return Application.InputManager.JoystickManager.ConnectedJoystick[Index] == true; }
        }
    }
#endif
}
