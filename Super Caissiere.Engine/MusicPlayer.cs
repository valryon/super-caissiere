using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System.Threading;
using SuperCaissiere.Engine.Core;
using SuperCaissiere.Engine.Content;
using SuperCaissiere.Engine.Graphics;

namespace  Super_Caissiere.Music
{
    [SongContent(AssetName = "bgm1", AssetPath = "musics/bgm", Artist = "nope", Name = "nope")]
    public class MusicPlayer
    {
        private enum State
        {
            None,
            Title,
            Game,
            GameOver,
            Victory
        }

        private static MusicPlayer _instance;
        private float _volume;

        private SongContentItem _titleSong;
        private SongContentItem _winSong, _loseSong;

        private SongContentItem _currentSong;
        private List<SongContentItem> _gameSong;
        private State _state;

        private float _alpha;

        public MusicPlayer()
        {
            _instance = this;
            _volume = 0.5f;
            _gameSong = new List<SongContentItem>();

            _gameSong.Add(Application.MagicContentManager.GetMusic("bgm1"));

            MediaPlayer.IsRepeating = true;
        }

        public void Update(GameTime gameTime)
        {
            if (_alpha > 0f)
            {
                _alpha -= 0.001f;
            }

            if (_state == State.Game)
            {
                if (MediaPlayer.PlayPosition == _currentSong.Song.Duration)
                {
                    PlayGameMusic();
                }
            }
        }

        public void Draw(SpriteBatchProxy spriteBatch)
        {
            if (_alpha > 0)
            {
                spriteBatch.BeginNoCamera();

                string str = _currentSong.Artist + " - " + _currentSong.Name;
                Vector2 size = Application.MagicContentManager.Font.MeasureString(str);
                Vector2 loc = new Vector2(Resolution.VirtualWidth - size.X - 10, 50);

                var dstMenu = new Rectangle((int)(loc.X - 20), (int)loc.Y - 2 * (int)size.Y / 4, (int)size.X * 3 / 2, (int)size.Y * 2);
                var srcMenu = new Rectangle(0, 0, 127, 32);

                spriteBatch.Draw(Application.MagicContentManager.GetTexture("menu"), dstMenu, srcMenu, Color.White * _alpha, 0f, Vector2.Zero, Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally, 1.0f);
                spriteBatch.DrawString(Application.MagicContentManager.Font, str, loc, Color.White * _alpha);

                spriteBatch.End();
            }
        }

        public static void PlayGameMusic()
        {
            var list = new List<SongContentItem>(_instance._gameSong);

            if (_instance._currentSong != null)
            {
                list.Remove(_instance._currentSong);
            }

            var song = list.GetRandomElement<SongContentItem>();

            _instance._currentSong = song;

            _instance._state = State.Game;
            _instance.PlaySong(song.Song);
        }

        public static void PlayHomeMusic()
        {
            _instance._currentSong = _instance._titleSong;

            _instance.PlaySong(_instance._titleSong.Song);
            _instance._state = State.Title;
        }

        public static void PlayGameOver(bool win)
        {
            if (win)
            {
                _instance._currentSong = _instance._winSong;

                _instance.PlaySong(_instance._winSong.Song);
                _instance._state = State.Victory;
            }
            else
            {
                _instance._currentSong = _instance._loseSong;

                _instance.PlaySong(_instance._loseSong.Song);
                _instance._state = State.GameOver;
            }
        }

        private void PlaySong(Song song)
        {
            _alpha = 1.0f;

            new Thread(new ThreadStart(() =>
            {
                MediaPlayer.Volume = _volume;
                MediaPlayer.Play(song);
            })).Start();
        }
    }
}
