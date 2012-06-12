using System.Diagnostics;

namespace SuperCaissiere.Engine.Input
{
    /// <summary>
    /// Encapsulate Joystick state data for a frame
    /// </summary>
    public class JoystickState
    {
        public bool[] Buttons { get; set; }
        public int XAxis { get; set; }
        public int YAxis { get; set; }
        public int ZAxis { get; set; }
    }

    /// <summary>
    /// This class analyzes and manages joystick/joypad devices using JoystickReader
    /// </summary>
    public class JoystickManager
    {
        /// <summary>
        /// Arbitrary decide the maximum number of joystick to test.
        /// But who the hell have more than 4 joysticks connected to USB ports simultanely ?!
        /// </summary>
        public static int MaxJoystickNumber = 4;

        /// <summary>
        /// The value of a joystick axis when centered
        /// </summary>
        public static int MiddleAxisValue = 32767;

#if WINDOWS
        /// <summary>
        /// Actually connected joystick. This may be updated by calling the Scan method.
        /// Tell you if a joystick ID (between 0 and MaxJoystickNumber) is available.
        /// </summary>
        public bool[] ConnectedJoystick { get; set; }

        /// <summary>
        /// Joystick data. Be sure the joystick is connected to get accurate data.
        /// </summary>
        public JoystickReader[] JoystickReaders { get; set; }

        /// <summary>
        /// Initialize the manager
        /// </summary>
        public JoystickManager()
        {
            ConnectedJoystick = new bool[MaxJoystickNumber];
            JoystickReaders = new JoystickReader[MaxJoystickNumber];

            for (int i = 0; i < MaxJoystickNumber; i++)
            {
                ConnectedJoystick[i] = false;
                JoystickReaders[i] = new JoystickReader(i);
            }

            Scan();
        }

        /// <summary>
        /// Update devices status
        /// </summary>
        public void Scan()
        {
            for (int i = 0; i < MaxJoystickNumber; i++)
            {
                ScanID(i);
            }
        }

        /// <summary>
        /// Update a precise device status
        /// </summary>
        /// <param name="id"></param>
        private void ScanID(int id)
        {
            bool[] buttons;
            int axisX, axisY, axisZ;

            bool oldState = ConnectedJoystick[id];
            ConnectedJoystick[id] = JoystickReaders[id].GetJoy(out buttons, out axisX, out axisY, out axisZ);

            if (ConnectedJoystick[id] != oldState)
            {
#if TRACE
                Trace.WriteLine(" Joystick " + id + " : connected = " + ConnectedJoystick[id]);
#endif
            }
        }

        /// <summary>
        /// Get button and axis state for a joypad (SHOULD BE CONNECTED !!!)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JoystickState GetJoyState(int id)
        {
            JoystickState state = new JoystickState();

            if (ConnectedJoystick[id] == true)
            {
                bool[] buttons;
                int axisX, axisY, axisZ;

                if (JoystickReaders[id].GetJoy(out buttons, out axisX, out axisY, out axisZ) == false)
                {
#if DEBUG && TRACE
                    Trace.WriteLine("Joystick " + id + " used but not connected !");
#endif
                }

                state.Buttons = buttons;
                state.XAxis = axisX;
                state.YAxis = axisY;
                state.ZAxis = axisZ;
            }
            else
            {
#if DEBUG && TRACE
                Trace.WriteLine("Joystick " + id + " used but not connected !");
#endif
            }
            return state;
        }
#endif
    }
}
