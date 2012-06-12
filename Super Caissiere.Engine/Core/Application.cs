using System;
using System.Reflection;
using SuperCaissiere.Engine.Content;
using SuperCaissiere.Engine.Graphics;
using SuperCaissiere.Engine.Input;
using SuperCaissiere.Engine.Physics;
using SuperCaissiere.Engine.Script;
using SuperCaissiere.Engine.Util;
using SuperCaissiere.Engine.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace SuperCaissiere.Engine.Core
{
    /// <summary>
    /// Provides the basics features/helpers/services
    /// </summary>
    public abstract class Application : Game
    {
        protected static Application Instance;

        private GraphicsDeviceManager _graphics;
        private SpriteBatchProxy _spriteBatchProxy;
        private FpsCounter _fpsCounter;
        private RandomMachine _randomMachine;

        private InputManager _inputManager;
        private MagicContentManager _magicContentManager;
        private GameStateManager _gameStateManager;
        private ScriptManager _scriptManager;
        private PhysicsManager _physicsManager;
        private bool _isdebugMode;
        private string _name;
        private string _version;

        /// <summary>
        /// Constructor
        /// </summary>
        protected Application(string name, string rootContentDirectory, string version)
        {
            // associates the static application instance to the current one
            Instance = this;

            _name = name;
            _version = version;

#if WINDOWS
            Window.Title = _name;
#endif

            Content.RootDirectory = rootContentDirectory;

            _graphics = new GraphicsDeviceManager(this);

            _inputManager = new InputManager();
            _magicContentManager = new MagicContentManager(GameAssemblies, Content);
            _gameStateManager = new GameStateManager();
            _scriptManager = new ScriptManager();
            _physicsManager = new PhysicsManager();

            _fpsCounter = new FpsCounter();
            _randomMachine = new RandomMachine(DateTime.Now.Millisecond);

            //Xbox Live
#if XBOX
            this.Components.Add(new GamerServicesComponent(this));
#endif
        }

        protected override void LoadContent()
        {
            _spriteBatchProxy = new SpriteBatchProxy(new SpriteBatch(GraphicsDevice));

            // Initialize the resolution
            Resolution.Initialize(Graphics, ScreenResolutionWidth, ScreenResolutionHeight, GameResolutionWidth, GameResolutionHeight, IsFullscreen);

            _magicContentManager.Initialize();
            _gameStateManager.Initialize();
            _scriptManager.Initialize();
            _physicsManager.Initialize();

            base.LoadContent();
        }

        /// <summary>
        /// Reference page contains links to related conceptual articles.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Update.</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // we update the Interpolator and Timer providers
            Interpolator.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            Timer.Update((float)gameTime.ElapsedGameTime.TotalSeconds);

            // Update managers
            _inputManager.Update(gameTime);
            _gameStateManager.Update(gameTime);
            _scriptManager.Update(gameTime);
            _physicsManager.Update(gameTime);

            _fpsCounter.Update(gameTime);
        }

        /// <summary>
        /// Reference page contains code sample.
        /// </summary>
        /// <param name="gameTime">Time passed since the last call to Draw.</param>
        protected override void Draw(GameTime gameTime)
        {
            _spriteBatchProxy.ClearDevice(Color.Black);
            _spriteBatchProxy.ClearViewport(Color.CornflowerBlue);

            _gameStateManager.Draw(gameTime);

            base.Draw(gameTime);
        }

        public static void Quit()
        {
            Instance.Exit();
        }

        public static bool IsApplicationActive
        {
            get { return Instance.IsActive; }
        }

        public static GraphicsDeviceManager Graphics
        {
            get { return Instance._graphics; }
        }

        public static SpriteBatchProxy SpriteBatch
        {
            get { return Instance._spriteBatchProxy; }
        }

        public static InputManager InputManager
        {
            get { return Instance._inputManager; }
        }

        public static MagicContentManager MagicContentManager
        {
            get { return Instance._magicContentManager; }
        }

        public static GameStateManager GameStateManager
        {
            get { return Instance._gameStateManager; }
        }

        public static ScriptManager ScriptManager
        {
            get { return Instance._scriptManager; }
        }

        public static PhysicsManager PhysicsManager
        {
            get { return Instance._physicsManager; }
        }

        public static FpsCounter FpsCounter
        {
            get { return Instance._fpsCounter; }
        }

        public static RandomMachine Random
        {
            get { return Instance._randomMachine; }
        }

        public static bool IsDebugMode
        {
            get { return Instance._isdebugMode; }
            set { Instance._isdebugMode = value; }
        }

        public static string Name { get { return Instance._name; } }
        public static string Version { get { return Instance._version; } }
        protected abstract int GameResolutionWidth { get; }
        protected abstract int GameResolutionHeight { get; }
        protected abstract int ScreenResolutionWidth { get; }
        protected abstract int ScreenResolutionHeight { get; }
        protected abstract bool IsFullscreen { get; }

        protected abstract Assembly[] GameAssemblies { get; }
    }
}
