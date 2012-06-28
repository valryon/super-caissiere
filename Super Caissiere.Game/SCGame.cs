using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Super_Caissiere.States;
using SuperCaissiere.Engine.Content;
using SuperCaissiere.Engine.Core;
using SuperCaissiere.Engine.Input.Devices;

namespace Super_Caissiere
{
    [TextureContent(AssetName = "null", AssetPath = "gfxs/misc/1x1", LoadOnStartup = true)]
    [FontContent(AssetName = "font", AssetPath = "fonts/spriteFont", IsDefaultFont = true)]
    public class SCGame : Application
    {
        public SCGame()
            : base("Super Caissière !", "Content", "1.0")
        { }

        protected override void Initialize()
        {
            base.Initialize();

            // Game states
            GameStateManager.RegisterGameState(new SplashscreenState());
            GameStateManager.RegisterGameState(new HomeState());
            GameStateManager.RegisterGameState(new IngameState());
            GameStateManager.RegisterGameState(new CreditsGameState());

            // Register mouse
            MouseDevice mouse = new MouseDevice(SuperCaissiere.Engine.Input.LogicalPlayerIndex.One);
            Application.InputManager.RegisterDevice(mouse);

            //Register keyboard
            KeyboardDevice keyboard = new KeyboardDevice(SuperCaissiere.Engine.Input.LogicalPlayerIndex.One);
            keyboard.MapButton(Microsoft.Xna.Framework.Input.Keys.Space, SuperCaissiere.Engine.Input.MappingButtons.A);
            keyboard.MapLeftThumbstick(Microsoft.Xna.Framework.Input.Keys.Up, Microsoft.Xna.Framework.Input.Keys.Down, Microsoft.Xna.Framework.Input.Keys.Left, Microsoft.Xna.Framework.Input.Keys.Right);

            Application.InputManager.RegisterDevice(keyboard);

            // Load the first scene
            GameStateManager.LoadGameState(GameStateManager.GetGameState<SplashscreenState>());
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

#if DEBUG
            SpriteBatch.BeginNoCamera();
            Application.FpsCounter.Draw(SpriteBatch, new Vector2(GameResolutionWidth - 100, GameResolutionHeight - 30));
            SpriteBatch.End();
#endif
        }

        protected override int GameResolutionWidth { get { return 800; } }

        protected override int GameResolutionHeight { get { return 600; } }

        protected override int ScreenResolutionWidth { get { return 800; } }

        protected override int ScreenResolutionHeight { get { return 600; } }

        protected override bool IsFullscreen { get { return false; } }

        protected override System.Reflection.Assembly[] GameAssemblies
        {
            get { return new Assembly[] { GetType().Assembly}; }
        }
    }
}
