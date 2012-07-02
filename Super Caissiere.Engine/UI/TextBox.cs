using System;
using Microsoft.Xna.Framework;
using SuperCaissiere.Engine.Content;
using SuperCaissiere.Engine.Graphics;
using SuperCaissiere.Engine.Core;

namespace SuperCaissiere.Engine.UI
{
    public interface ITextBox
    {
        void Update(GameTime gametime);
        void Draw(SpriteBatchProxy spriteBatch);
    }

    /// <summary>
    /// Simple HUD modal txtbox to display information to player
    /// </summary>
    /// 

    [TextureContent(AssetName = "txtbox", AssetPath = "gfxs/misc/textbox", LoadOnStartup = false)]
    public class TextBox : ITextBox
    {
        private string m_text;
        private Rectangle m_srcRect;
        private Vector2 m_size, m_location;
        private string m_skip;
        private Vector2 m_skip_size;
        private int m_anim = 0;
        private double m_time = 0;
        public bool IsModal{get; set;}
        private int m_times = 0;

        public TextBox(string _text, bool _modal)
        {
            m_text = _text;
            m_srcRect = new Rectangle(0, 0, 16, 16);
            m_size = Application.MagicContentManager.Font.MeasureString(m_text);
            IsModal = _modal;
            m_location = new Vector2(Resolution.VirtualWidth / 2 - m_size.X / 2, Resolution.VirtualHeight / 2 - m_size.Y / 2);
            m_skip = "Espace: fermer";
            m_skip_size = Application.MagicContentManager.Font.MeasureString(m_skip);
        }

        /**
         * True when the timer is finish
         **/

        public bool isDone()
        {
            return (!IsModal) && (m_times > 6); //3Seconds
        }

        public void Update(GameTime gametime)
        {
            m_time += gametime.ElapsedGameTime.Milliseconds;
            if (m_time > 500)
            {
                m_time = 0;
                m_times++;
                m_anim = (m_anim == 0) ? 1 : 0;
            }

        }

        private const int MARGIN = 32;
        public void Draw(SpriteBatchProxy spriteBatch)
        {
            Rectangle dst = new Rectangle((int)m_location.X - MARGIN, (int)m_location.Y - MARGIN, 16, 16);
            m_srcRect.X = m_srcRect.Y = 0;

            for (int j = 0; j <= Math.Floor((double)((m_size.Y + MARGIN * 2) / m_srcRect.Height)); j++)
            {
                for (int i = 0; i <= Math.Floor((double)((m_size.X + MARGIN * 2) / m_srcRect.Width)); i++)
                {
                    spriteBatch.Draw(Application.MagicContentManager.EmptyTexture, dst, m_srcRect, Color.White);

                    dst.Offset(16, 0);

                    if (i == 0)
                    {
                        // first so offset for srcRect
                        m_srcRect.Offset(16, 0);
                    }
                    else if (i == Math.Floor((double)((m_size.X + MARGIN * 2) / m_srcRect.Width)) - 1)
                    {
                        //last
                        m_srcRect.Offset(16, 0);
                    }
                }

                dst.Offset(-16 * (int)(Math.Floor((double)((m_size.X + MARGIN * 2) / m_srcRect.Width)) + 1), 16);
                m_srcRect.X = 0;

                if (j == 0)
                {
                    // first so offset for srcRect
                    m_srcRect.Offset(0, 16);
                }
                else if (j == Math.Floor((double)((m_size.Y + MARGIN * 2) / m_srcRect.Height)) - 1)
                {
                    //last
                    m_srcRect.Offset(0, 16);
                }
            }

            m_srcRect.X = m_anim * 16;
            m_srcRect.Y = 48;

            dst.Offset((int)(m_size.X + MARGIN * 2) - (16 + (int)m_skip_size.X + 8), -((int)m_skip_size.Y + 8));

            spriteBatch.Draw(Application.MagicContentManager.EmptyTexture, dst, m_srcRect, Color.White);

            dst.Offset(16, 0);

            spriteBatch.DrawString(Application.MagicContentManager.Font, m_skip, dst.Location.ToVector2(), Color.Black);
            spriteBatch.DrawString(Application.MagicContentManager.Font, m_text, m_location, Color.Black);
        }



    }
}
