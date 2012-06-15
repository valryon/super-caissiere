using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using SuperCaissiere.Engine.Input.Devices;

namespace SuperCaissiere.Engine.Input
{

    /// <summary>
    /// Manage input for players
    /// </summary>
    public class InputManager
    {
        public JoystickManager JoystickManager { get; private set; }

        private Dictionary<int, JoystickState> _joyState;
        private Dictionary<int, JoystickState> _previousJoyState;
        private List<Device> _registeredDevices;

        public InputManager()
        {
            JoystickManager = new JoystickManager();
            JoystickManager.Scan();

            _joyState = new Dictionary<int, JoystickState>();
            _previousJoyState = new Dictionary<int, JoystickState>();

            _registeredDevices = new List<Device>();
        }

        /// <summary>
        /// Update the input manager and the registered devices
        /// </summary>
        public void Update(GameTime gameTime)
        {
#if WINDOWS

            // Joypads
            // TODO : Disable joypad scan due to performance issues
            //JoystickManager.Scan();

            _joyState.Clear();

            for (int i = 0; i < JoystickManager.ConnectedJoystick.Length; i++)
            {
                if (JoystickManager.ConnectedJoystick[i] == true)
                {
                    _joyState.Add(i, JoystickManager.GetJoyState(i));
                }
            }
#endif

            foreach (Device d in _registeredDevices)
            {
                d.Update(gameTime);
            }

#if WINDOWS
            _previousJoyState.Clear();

            foreach (int k in _joyState.Keys)
            {
                _previousJoyState.Add(k, _joyState[k]);
            }
#endif
        }

        public void RegisterDevice(Device d)
        {
            _registeredDevices.Add(d);
        }

        public void UnregisterDevice(Device d)
        {
            if (_registeredDevices.Contains(d))
            {
                _registeredDevices.Remove(d);
            }
        }

        public T GetDevice<T>(LogicalPlayerIndex playerIndex) where T : Device
        {
            foreach (Device d in _registeredDevices)
            {
                if ((d.LogicalPlayerIndex == playerIndex) && (d is T))
                {
                    return (T)d;
                }
            }

            return null;
        }

        /// <summary>
        /// Get devices registered for a player
        /// </summary>
        /// <param name="playerIndex"></param>
        /// <returns></returns>
        public List<Device> GetLinkedDevices(LogicalPlayerIndex playerIndex)
        {
            List<Device> devices = new List<Device>();

            foreach (Device d in _registeredDevices)
            {
                if (d.LogicalPlayerIndex == playerIndex)
                {
                    devices.Add(d);
                }
            }

            return devices;
        }

#if WINDOWS

        public JoystickState GetJoystickState(int index)
        {
            JoystickState state;

            if (_joyState.TryGetValue(index, out state))
            {
                return state;
            }

            throw new ArgumentException("Invalid joypad index: " + index);
        }

        public JoystickState GetPreviousJoystickState(int index)
        {
            JoystickState state;

            if (_previousJoyState.TryGetValue(index, out state))
            {
                return state;
            }

            throw new ArgumentException("Invalid joypad index: " + index);
        }

#endif
    }
}
