using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace SuperCaissiere.Engine.Input.Devices
{
    /// <summary>
    /// Keyboard mouse device with mapping
    /// </summary>
    public class KeyboardDevice : Device
    {
        private Dictionary<Keys, MappingButtons> _keyboardMapping;

        private Keys _thumbsLeftUp;
        private Keys _thumbsLeftDown;
        private Keys _thumbsLeftLeft;
        private Keys _thumbsLeftRight;

        private Keys _thumbsRightUp;
        private Keys _thumbsRightDown;
        private Keys _thumbsRightLeft;
        private Keys _thumbsRightRight;

        private KeyboardState _keyboard, _pkeyboard;

        public KeyboardDevice(LogicalPlayerIndex logicalPlayerIndex)
            : base(logicalPlayerIndex,DeviceType.KeyboardMouse, 0)
        {
            _keyboardMapping = new Dictionary<Keys, MappingButtons>();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _keyboard = Keyboard.GetState();

            base.Update(gameTime);

            // Update mapping
            foreach (Keys key in _keyboardMapping.Keys)
            {
                MappingButtons button = _keyboardMapping[key];
                bool isDown = false;
                bool isPressed = false;
                bool isReleased = false;

                if (_keyboard.IsKeyDown(key))
                {
                    isDown = true;

                    if (_pkeyboard.IsKeyUp(key))
                    {
                        isPressed = true;
                    }
                }
                else if (_keyboard.IsKeyUp(key))
                {
                    if (_pkeyboard.IsKeyDown(key))
                    {
                        isReleased = true;
                    }
                }

                SetMappingValue(button, isDown, isPressed, isReleased);
            }

            // Thumbstick emulation
            // -- LEFT
            float leftx = 0f;

            if (_keyboard.IsKeyDown(_thumbsLeftLeft))
            {
                leftx = -1f;
            }
            else if (_keyboard.IsKeyDown(_thumbsLeftRight))
            {
                leftx = 1f;
            }

            ThumbStickLeft.X = leftx;

            float lefty = 0f;
            if (_keyboard.IsKeyDown(_thumbsLeftUp))
            {
                lefty = -1f;
            }
            else if (_keyboard.IsKeyDown(_thumbsLeftDown))
            {
                lefty = 1f;
            }

            ThumbStickLeft.Y = lefty;

            // --RIGHT
            float rightx = 0f;
            if (_keyboard.IsKeyDown(_thumbsRightLeft))
            {
                rightx = -1f;
            }
            else if (_keyboard.IsKeyDown(_thumbsRightRight))
            {
                rightx = 1f;
            }

            ThumbStickRight.X = rightx;

            float righty = 0f;
            if (_keyboard.IsKeyDown(_thumbsRightUp))
            {
                righty = -1f;
            }
            else if (_keyboard.IsKeyDown(_thumbsRightDown))
            {
                righty = 1f;
            }

            ThumbStickRight.Y = righty;

            _pkeyboard = _keyboard;
        }

        public void MapButton(Keys key, MappingButtons button)
        {
            _keyboardMapping[key] = button;
        }

        public Keys? GetMapping(MappingButtons button)
        {
            foreach (Keys key in _keyboardMapping.Keys)
            {
                if (_keyboardMapping[key] == button) return key;
            }

            return null;
        }

        public Keys[] GetLeftThumbstickMapping()
        {
            return new Keys[] { _thumbsLeftUp, _thumbsLeftDown, _thumbsLeftLeft, _thumbsLeftRight };
        }

        public Keys[] GetRightThumbstickMapping()
        {
            return new Keys[] { _thumbsRightUp, _thumbsRightDown, _thumbsRightLeft, _thumbsRightRight };
        }

        public void MapLeftThumbstick(Keys up, Keys down, Keys left, Keys right)
        {
            _thumbsLeftUp = up;
            _thumbsLeftDown = down;
            _thumbsLeftLeft = left;
            _thumbsLeftRight = right;
        }

        public void MapRightThumbstick(Keys up, Keys down, Keys left, Keys right)
        {
            _thumbsRightUp = up;
            _thumbsRightDown = down;
            _thumbsRightLeft = left;
            _thumbsRightRight = right;
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
    }
}
