#if WINDOWS
using System;
using System.Runtime.InteropServices;

namespace SuperCaissiere.Engine.Input
{
    /*
     * TUTORIAL: Utilisation d'un Joystick/Joypad en C# 
     * 
     * Ecrit par Ambroise Garel (agar@cafedefaune.com) pour Irrlicht.fr
     * 
     * Source : http://www.irrlicht.fr/forum/viewtopic.php?id=64
     * */

    /// <summary>
    /// Utilisation d'un joystick PC
    /// </summary>
    public class JoystickReader
    {
        [DllImport("winmm.dll")]
        private static extern int joyGetPos(int uJoyID, ref JOYINFO pji);

        private const int MAXJOYBUTTONS = 16;

        private int joystick_id;

        private struct JOYINFO
        {
            public int wXpos;
            public int wYpos;
            public int wZpos;
            public int wButtons;
        }


        ///<summary>
        /// Constructeur: passez en paramètre l'ID du joystick dont vous souhaitez lire les informations.
        /// Si vous n'avez qu'un seul joystick, c'est pas compliqué, son ID est zéro.
        /// Sinon, vous devrez expérimenter : ça commence à 0 et ça finit à "nombre de joysticks - 1"
        /// </summary>
        public JoystickReader(int joyid) { joystick_id = joyid; }

        ///<summary>
        /// Fonction GetJoy, à appeler dans la boucle principale de votre programme, en lui passant
        /// en paramètres 4 valeurs:
        /// - Un tableau (d'une seule dimension) de booléens, qui vous dira si tel ou tel bouton est
        ///   appuyé (true) ou relaché (false)
        /// - Trois entiers, les axes X, Y et Z du joystick.
        ///     Si votre joystick est "au repos" (centré) sur un axe, la valeur sera 32767.
        ///     Si le joy est poussé à fond dans une direction, vous aurez 0 ou 65535 (selon le sens).
        ///       * Avec un controleur analogique (comme un joystick),
        ///         toutes les valeurs intermédiaires sont possibles.
        ///       * Avec un controleur digital (comme un joypad de base),
        ///         les valeurs sont toujours 0, 32767 ou 65535 (il n'y a pas d'intermédiaire)
        ///
        /// La fonction retourne "true" si ça c'est bien passé, "false" s'il y a eu un problème
        /// (par exemple s'il n'y a pas de joystick répondant à cette ID).
        /// </summary>
        public bool GetJoy(out bool[] joybuttons, out int axis_x, out int axis_y, out int axis_z)
        {
            JOYINFO JoyInformation = new JOYINFO();

            joybuttons = new bool[MAXJOYBUTTONS];

            if (joyGetPos(joystick_id, ref JoyInformation) != 0)
            {
                axis_x = 0;
                axis_y = 0;
                axis_z = 0;
                return false;
            }

            for (int i = 0; i < MAXJOYBUTTONS; i++)
                joybuttons[i] = (((int)Math.Pow(2, i) & JoyInformation.wButtons) != 0);

            axis_x = JoyInformation.wXpos;
            axis_y = JoyInformation.wYpos;
            axis_z = JoyInformation.wZpos;

            return true;
        }
    }
}
#endif

