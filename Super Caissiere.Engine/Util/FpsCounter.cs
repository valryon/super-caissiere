using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SuperCaissiere.Engine.Graphics;
using SuperCaissiere.Engine.Core;

namespace SuperCaissiere.Engine.Util
{
    public class FpsCounter
    {
        private int _totalFrames;
        private float _elapsedTime;
        private int _fps;

        public FpsCounter()
        {
            Initialize();
        }

        public void Initialize()
        {
            _totalFrames = 0;
            _elapsedTime = 0f;
            _fps = 0;
        }

        /// <summary>
        /// Doit etre appelé à chaque frame pour être valide
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // 1 Seconde
            if (_elapsedTime >= 1000.0f)
            {
                _fps = _totalFrames;
                _totalFrames = 0;
                _elapsedTime = 0;
            }
        }

        /// <summary>
        /// Affichage du nombre de FPS à l'écran et calcul du nombre d'affichages
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="location"></param>
        public void Draw(SpriteBatchProxy spriteBatch, Vector2 location)
        {
            // On compte un affichage supplémentaire
            _totalFrames++;

            Color color = Color.Green;

#if !WINDOWS_PHONE
            if (_fps < 40) color = Color.Coral;
#endif

            if (_fps < 25) color = Color.Red;

            spriteBatch.DrawString(Application.MagicContentManager.Font, string.Format("FPS={0}", _fps), location, color);
        }
    }
}
