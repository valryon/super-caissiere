using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperCaissiere.Engine.Input
{
    public class ButtonState
    {
        public bool IsDown { get; set; }
        public bool IsPressed { get; set; }
        public bool IsReleased { get; set; }
    }

    public class ThumbstickState : ButtonState
    {
        public float X { get; set; }
        public float Y { get; set; }

        public float PreviousX { get; set; }
        public float PreviousY { get; set; }
    }

    /// <summary>
    /// Gamepad buttons mapping
    /// </summary>
    public enum MappingButtons
    {
        A,
        B,
        X,
        Y,
        LB,
        RB,
        Start,
        Back,
        Home,
    }

}
