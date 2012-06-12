using Microsoft.Xna.Framework;

namespace SuperCaissiere.Engine.Core
{
    /// <summary>
    /// Game service (input, renderer, etc)
    /// </summary>
    public interface Manager
    {
        /// <summary>
        /// Initialize the service
        /// </summary>
        /// <param name="Content"></param>
        void Initialize();

        /// <summary>
        /// Update the service
        /// </summary>
        /// <param name="gameTime"></param>
        void Update(GameTime gameTime);

    }
}
