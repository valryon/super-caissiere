using SuperCaissiere.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperCaissiere.Engine.Particules
{
    /// <summary>
    /// Manage, move, create particules
    /// </summary>
    public class ParticuleManager : Manager
    {
        private const int MaxParticules = 512;
        private Particule[] _particules;

        public ParticuleManager()
        {
            _particules = new Particule[MaxParticules];
        }

        /// <summary>
        /// Clear particule manager on init
        /// </summary>
        public void Initialize()
        {
            for (int i = 0; i < MaxParticules; i++)
            {
                _particules[i] = null;
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < _particules.Length; i++)
            {
                if (_particules[i] != null)
                {
                    _particules[i].Update(gameTime);

                    if (_particules[i].IsAlive == false)
                    {
                        _particules[i] = null;
                    }
                }
            }
        }

        public void Draw(bool background)
        {
            var spriteBatch = Application.SpriteBatch;
            var camera = Application.GameStateManager.CurrentGameState.SceneCamera;

            //Additive ones
            spriteBatch.Begin(camera, SpriteSortMode.Deferred, BlendState.Additive);

            for (int i = 0; i < _particules.Length; i++)
            {
                if ((_particules[i] != null) && (_particules[i].IsAdditive) && (_particules[i].IsBackground == background))
                {
                    _particules[i].Draw(spriteBatch);
                }
            }

            spriteBatch.End();

            //Normal ones
            spriteBatch.Begin(camera, SpriteSortMode.Deferred, BlendState.AlphaBlend);

            for (int i = 0; i < _particules.Length; i++)
            {
                if ((_particules[i] != null) && (_particules[i].IsAdditive == false))
                {
                    _particules[i].Draw(spriteBatch);
                }
            }

            spriteBatch.End();
        }

        /// <summary>
        /// Add a particule
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Added to the list</returns>
        public bool AddParticule(Particule p)
        {
            for (int i = 0; i < _particules.Length; i++)
            {
                if (_particules[i] == null)
                {
                    _particules[i] = p;
                    return true;
                }
            }

            return false;
        }
    }
}
