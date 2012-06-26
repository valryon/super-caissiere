using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SuperCaissiere.Engine.Core;
using SuperCaissiere.Engine.Input;
using SuperCaissiere.Engine.Utils;

namespace Super_Caissiere.SpecialEffects
{
    public class SpecialEffectsHelper
    {
        private static Timer _shakeCameraTimer;

        /// <summary>
        /// Shake the screen for few milliseconds
        /// </summary>
        /// <param name="amplitude"></param>
        /// <param name="speed"></param>
        /// <param name="length">seconds</param>
        public static void ShakeScreen(Vector2 amplitude, float length)
        {
            var camera = Application.GameStateManager.CurrentGameState.SceneCamera;

            camera.ShakeFactor = amplitude;
            camera.ShakeSpeed = new Vector2(50f, 50f);

            if (_shakeCameraTimer != null)
                _shakeCameraTimer.Stop();

            _shakeCameraTimer = Timer.Create(length, false, (t =>
            {
                camera.ShakeFactor = Vector2.Zero;
            }));

            // Make devices rumble
            foreach (var device in Application.InputManager.GetLinkedDevices(LogicalPlayerIndex.One))
            {
                device.Rumble(amplitude / new Vector2(5f));
            }
        }
    }
}
