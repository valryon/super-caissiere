using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SuperCaissiere.Engine.Content;
using SuperCaissiere.Engine.Graphics;





namespace SuperCaissiere.Engine.Core
{
    public enum STATE
    {
        INACTIVE,
        BAD_ANIM,
        GOOD_ANIM,
        ACTIVE,
    };

    [TextureContent(AssetName = "barcode", AssetPath = "gfxs/sprites/code_barre", LoadOnStartup = false)]
    [FontContent(AssetName = "barcode", AssetPath = "gfxs/sprites/code_barre", LoadOnStartup = false)]
    public class BarCodeQTE
    {

        private string[] RANK_TEXT = { " ", " ", "", "" };
        private int[] POSITIONS = { 0, 105, 135, 165, 195, 255, 285, 315, 345, 375, 446 };
        public const int BARCODE_LENGTH = 10;
        private KeyboardState lastkb, kb;
        private SpriteFont font;
        private int[] barcodeDigits;
        private int currentIdx;
        private Vector2 position;
        private Vector2 size;
        private Texture2D barcodeTex;
        private Texture2D nullTex;
        private Rectangle _dstRect;
        public bool isValidated { get; set; }
        private Random randomizator;

        private Color color;

        private int rank;

        private long time, anim_time;
        private long blink_timer;
        private STATE currentState;
        private bool blink;

        public BarCodeQTE()
        {

            
            currentState = STATE.INACTIVE;
            font = Application.MagicContentManager.Font;
            randomizator = new Random((int)System.DateTime.Now.Ticks);

            barcodeDigits = new int[BARCODE_LENGTH];
            barcodeTex = Application.MagicContentManager.GetTexture("barcode");
            nullTex = Application.MagicContentManager.EmptyTexture;
            int _w=barcodeTex.Width;
            int _h=barcodeTex.Height;
            int _x=(Application.Graphics.GraphicsDevice.Viewport.Width-_w)/2;
            int _y = (Application.Graphics.GraphicsDevice.Viewport.Height - _h) / 2;
            position = new Vector2(_x, _y);
            size = new Vector2(_w, _h);
            _dstRect = new Rectangle(_x, _y, _w, _h);
        }





        public void generateBarCode()
        {
            for (int i = 0; i < BARCODE_LENGTH; i++)
            {
                double n = randomizator.NextDouble();
                barcodeDigits[i] = (int)(n * 9);
            }
        }

        public void start()
        {
            generateBarCode();
            isValidated = false;
            changeStateTo(STATE.ACTIVE);
            time = 0;
        }


        private void getRank()
        {
            if (time > 10000)
            {
                rank = 0;
            }
            else if (time > 5000)
            {
                rank = 1;
            }
            else if (time > 3000)
            {
                rank = 2;
            }
            else
            {
                rank = 3;
            }
        }

        private void blinking(GameTime gameTime)
        {
            blink_timer += gameTime.ElapsedGameTime.Milliseconds;
            if (blink_timer > 100)
            {
                blink_timer = 0;
                blink = !blink;
            }
        }


        private void detectInput()
        {
            lastkb = kb;
            kb = Keyboard.GetState();
            Keys[] keys = kb.GetPressedKeys();
            if (keys.Length == 0) return;

            int n = -1;

            foreach (Keys key in keys.ToList())
            {
                if (key != Keys.None)
                {
                    switch (key)
                    {
                        case Keys.NumPad0: if (lastkb.IsKeyUp(Keys.NumPad0)) n = 0; break;
                        case Keys.NumPad1: if (lastkb.IsKeyUp(Keys.NumPad1)) n = 1; break;
                        case Keys.NumPad2: if (lastkb.IsKeyUp(Keys.NumPad2)) n = 2; break;
                        case Keys.NumPad3: if (lastkb.IsKeyUp(Keys.NumPad3)) n = 3; break;
                        case Keys.NumPad4: if (lastkb.IsKeyUp(Keys.NumPad4)) n = 4; break;
                        case Keys.NumPad5: if (lastkb.IsKeyUp(Keys.NumPad5)) n = 5; break;
                        case Keys.NumPad6: if (lastkb.IsKeyUp(Keys.NumPad6)) n = 6; break;
                        case Keys.NumPad7: if (lastkb.IsKeyUp(Keys.NumPad7)) n = 7; break;
                        case Keys.NumPad8: if (lastkb.IsKeyUp(Keys.NumPad8)) n = 8; break;
                        case Keys.NumPad9: if (lastkb.IsKeyUp(Keys.NumPad9)) n = 9; break;
                    }
                }
            }
            if (n == -1) return;
            if (n == barcodeDigits[currentIdx])
            {
                currentIdx++;
                if (currentIdx >= BARCODE_LENGTH)
                {
                    currentIdx = 0;
                    getRank();
                    changeStateTo(STATE.GOOD_ANIM);
                }
            }
            else
            {
                currentIdx = 0;
                changeStateTo(STATE.BAD_ANIM);
            }


        }

        private void changeStateTo(STATE stt)
        {
            color = Color.Black;
            anim_time = 0;
            currentState = stt;

        }

        private void badInputAnimation(GameTime gameTime)
        {
            anim_time += gameTime.ElapsedGameTime.Milliseconds;
            if (anim_time > 2000) changeStateTo(STATE.ACTIVE);
            blinking(gameTime);
            color = (blink) ? Color.Red : Color.Black;
        }


        
        public bool isActive()
        {
            return (currentState != STATE.INACTIVE);
        }

        private void goodInputAnimation(GameTime gameTime)
        {
            anim_time += gameTime.ElapsedGameTime.Milliseconds;
            if (anim_time > 2000)
            {
                changeStateTo(STATE.INACTIVE);
                isValidated = true;
            }
            blinking(gameTime);
            color = (blink) ? Color.Green : Color.Black;
        }

        public void Update(GameTime gameTime)
        {
            if (currentState == STATE.ACTIVE)
                time += gameTime.ElapsedGameTime.Milliseconds;
            if (currentState == STATE.ACTIVE)
                detectInput();
            else if (currentState == STATE.BAD_ANIM)
                badInputAnimation(gameTime);
            else if (currentState == STATE.GOOD_ANIM)
                goodInputAnimation(gameTime);

        }


        public void Draw(SpriteBatchProxy spritebatch)
        {
            if (currentState == STATE.INACTIVE) return;
            Rectangle rect = _dstRect;

            rect.Width = POSITIONS[currentIdx];
            spritebatch.Draw(nullTex, rect, Color.Green);
            rect.Offset(POSITIONS[currentIdx], 0);
            rect.Width = _dstRect.Width - POSITIONS[currentIdx];
            spritebatch.Draw(nullTex, rect, color);
            spritebatch.Draw(barcodeTex, _dstRect, Color.White);
            //render number
            Vector2 v = new Vector2(80, 248);
            v += position;
            for (int i = 0; i < BARCODE_LENGTH; i++)
            {
                spritebatch.DrawString(font, barcodeDigits[i].ToString(), v, (i < currentIdx) ? Color.Green : color);
                v.X += 30;
                if (i == 4) v.X += 25;
            }
            if (currentState == STATE.GOOD_ANIM)
            {
                v.Y += 50;
                v.X = _dstRect.X + (_dstRect.Width - font.MeasureString(RANK_TEXT[rank]).X) / 2;
                spritebatch.DrawString(font, RANK_TEXT[rank], v, Color.Pink);
            }
        }
    }
}
