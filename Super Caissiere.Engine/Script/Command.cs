using System;
using Microsoft.Xna.Framework;

namespace SuperCaissiere.Engine.Script
{
    /// <summary>
    /// Script command
    /// </summary>
    public abstract class Command
    {
        public Command()
            : this(null, null) { }

        public Command(Func<bool> startCondition)
            : this(startCondition, null) { }

        public Command(Func<bool> startCondition, Func<bool> endCondition)
        {
            startCondition = StartCondition;
            endCondition = EndCondition;
        }

        /// <summary>
        /// Command will NOT be executed until this function returns true (or is null)
        /// </summary>
        public Func<bool> StartCondition { get; protected set; }

        /// <summary>
        /// Command will be executed until this function returns true.
        /// If null, the command will be executed only once.
        /// </summary>
        public Func<bool> EndCondition { get; protected set; }

        /// <summary>
        /// Code to execute
        /// </summary>
        public abstract void Execute(GameTime gameTime);
    }
}
