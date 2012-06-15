using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework;

namespace SuperCaissiere.Engine.Input.Devices
{
    /// <summary>
    /// Kind of device that can be used to play TGPA
    /// </summary>
    public enum DeviceType
    {
        Mouse,
        Keyboard,
        Gamepad, //x360 gamepad
        Joystick //PC Joystick/Joypad
    }

    /// <summary>
    /// A game device
    /// </summary>
    public abstract class Device
    {
        /// <summary>
        /// Kind of device
        /// </summary>
        public DeviceType Type { get; set; }

        /// <summary>
        /// Index of the device if required (for gamepad, x360 or not)
        /// </summary>
        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                RumbleAssignation();
            }
        }

        public LogicalPlayerIndex LogicalPlayerIndex { get; set; }

        /// <summary>
        /// Right thumbstick
        /// </summary>
        public ThumbstickState ThumbStickRight { get; protected set; }

        /// <summary>
        /// Left thumbstick
        /// </summary>
        public ThumbstickState ThumbStickLeft { get; protected set; }

        protected Dictionary<MappingButtons, ButtonState> mapping;
        protected Rumble[] rumbles;

        private int _index;

        protected Device(LogicalPlayerIndex logicalPlayerIndex, DeviceType type, int index)
        {
            Type = type;
            _index = index;

            mapping = new Dictionary<MappingButtons, ButtonState>();
            ThumbStickRight = new ThumbstickState();
            ThumbStickLeft = new ThumbstickState();

            rumbles = new Rumble[4];
            for (int i = 0; i < rumbles.Length; i++)
            {
                rumbles[i] = new Rumble(0);
            }

            RumbleAssignation();
        }

        /// <summary>
        /// Update button states
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            for (int i = 0; i < rumbles.Length; i++)
            {
                rumbles[i].Update(gameTime);
            }

            ThumbStickLeft.PreviousX = ThumbStickLeft.X;
            ThumbStickLeft.PreviousY = ThumbStickLeft.Y;

            ThumbStickRight.PreviousX = ThumbStickRight.X;
            ThumbStickRight.PreviousY = ThumbStickRight.Y;
        }

        /// <summary>
        /// Get a button state.
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        /// <remarks>For thumbsticks, use the thumbsticks (left / right) properties</remarks>
        public ButtonState GetState(MappingButtons button)
        {
            if (IsConnected)
            {
                if (mapping.Keys.Contains(button))
                {
                    return mapping[button];
                }
            }

            return new ButtonState();
        }

        /// <summary>
        /// Make the device rumble if possible
        /// </summary>
        /// <param name="power"></param>
        public void Rumble(Vector2 power)
        {
            if (Type != DeviceType.Gamepad) return;

            for (int i = 0; i < rumbles.Length; i++)
            {
                rumbles[i].Left = power.X;
                rumbles[i].Right = power.Y;
            }
        }

        private void RumbleAssignation()
        {
            if (Type != DeviceType.Gamepad) return;

            for (int i = 0; i < rumbles.Length; i++)
            {
                rumbles[i].GamePadIndex = Index;
            }
        }

        protected void SetMappingValue(MappingButtons button, bool isDown, bool isPressed, bool isReleased)
        {
            ButtonState state;

            if (mapping.TryGetValue(button, out state) == false)
            {
                state = new ButtonState();
            }

            state.IsDown = isDown;
            state.IsPressed = isPressed;
            state.IsReleased = isReleased;

            mapping[button] = state;
        }

        public override bool Equals(object obj)
        {
            if (obj is Device)
            {
                Device d2 = (Device)obj;

                return d2.Type == this.Type
                        && d2.Index == this.Index;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public abstract bool IsConnected
        {
            get;
        }
    }
}
