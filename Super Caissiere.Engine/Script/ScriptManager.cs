using System.Collections.Generic;
using SuperCaissiere.Engine.Core;
using Microsoft.Xna.Framework;
using System;

namespace SuperCaissiere.Engine.Script
{
    /// <summary>
    /// Manage and execute commands
    /// </summary>
    public class ScriptManager : Manager
    {
        /// <summary>
        /// Commands to execute
        /// </summary>
        private List<Command> _commands;

        /// <summary>
        /// Internal values dictionary
        /// </summary>
        private Dictionary<string, object> _flags;

        public ScriptManager()
        {
            _commands = new List<Command>();
            _flags = new Dictionary<string, object>();
        }

        public void Initialize()
        {
            _commands.Clear();
            _flags.Clear();
        }

        public void Update(GameTime gameTime)
        {
            _commands.ForEach(c =>
            {
                if (c.StartCondition == null || c.StartCondition())
                {
                    c.Execute(gameTime);

                    if (c.EndCondition == null || c.EndCondition())
                    {
                        _commands.Remove(c);
                    }
                }
            });
        }

        /// <summary>
        /// Add a new command to the manager
        /// </summary>
        /// <param name="command"></param>
        public void AddCommand(Command command)
        {
            _commands.Add(command);
        }

        /// <summary>
        /// Get flag value
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="id"></param>
        public TValue GetFlag<TValue>(string id)
        {
            object val;

            if (_flags.TryGetValue(id, out val))
            {
                if (val.GetType() == typeof(TValue))
                {
                    return (TValue)val;
                }

                throw new ArgumentException("Wrong type for " + id);
            }

            throw new ArgumentException("Unknow flag " + id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        public void SetFlag(string id, object value)
        {
            if (_flags.ContainsKey(id))
            {
                // Make sure types are equals
                if (_flags[id].GetType() != value.GetType())
                    throw new ArgumentException("value");
            }
            _flags[id] = value;
        }
    }
}
