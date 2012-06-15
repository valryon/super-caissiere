using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using XNAInput = Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SuperCaissiere.Engine.Input.Devices
{
    /// <summary>
    /// mouse device with mapping
    /// </summary>
    public class MouseDevice : Device
    {
        private MouseState _mouse, _pmouse;

        public MouseDevice(LogicalPlayerIndex logicalPlayerIndex)
            : base(logicalPlayerIndex, DeviceType.Mouse, 0)
        {
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _mouse = Mouse.GetState();

            base.Update(gameTime);

            MapButtonValue(_pmouse.LeftButton, _mouse.LeftButton, MappingButtons.A);
            MapButtonValue(_pmouse.MiddleButton, _mouse.MiddleButton, MappingButtons.X);
            MapButtonValue(_pmouse.RightButton, _mouse.RightButton, MappingButtons.B);

            _pmouse = _mouse;
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
#if WINDOWS
                return true;
#else
                return false;
#endif
            }
        }

        public Vector2 MouseLocation
        {
            get
            {
                return new Vector2(_mouse.X, _mouse.Y);
            }
        }

        public int ScrollValue
        {
            get
            {
                return _mouse.ScrollWheelValue;
            }
        }
    }
}
