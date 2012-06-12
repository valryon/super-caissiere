using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Threading;

namespace SuperCaissiere.Engine.Core
{
    /// <summary>
    /// Keep track of the game states and manage changes
    /// </summary>
    public class GameStateManager : Manager
    {
        private List<GameState> _gameStates;
        private LoadingGameState _loadingState;
        private GameState _currentGameState;
        private bool _isLoadingScreenDisplayed;

        public GameStateManager()
            : base()
        {
            _gameStates = new List<GameState>();
            _isLoadingScreenDisplayed = false;
        }

        #region Manager

        public void Initialize()
        {

        }

        public void Update(GameTime gameTime)
        {
            if (_currentGameState != null)
            {
                _currentGameState.Update(gameTime);

                if (_currentGameState.ChangeCurrentGameState)
                {
                    if (_isLoadingScreenDisplayed)
                    {
                        _isLoadingScreenDisplayed = false;
                        changeCurrentGameState(_currentGameState.NextGameState);
                    }
                    else
                    {
                        LoadGameState(_currentGameState.NextGameState);
                    }
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (_currentGameState != null)
            {
                _currentGameState.Draw(Application.SpriteBatch);
            }
        }

        #endregion

        public void RegisterGameState(GameState newGameState)
        {
            if (newGameState is LoadingGameState)
            {
                if (_loadingState != null) throw new ArgumentException("Loading state already set.");

                _loadingState = newGameState as LoadingGameState;
            }

            if (_gameStates.Contains(newGameState)) throw new ArgumentException("State already registered.");
            _gameStates.Add(newGameState);
        }

        /// <summary>
        /// Look for a game state, using its type
        /// </summary>
        /// <typeparam name="TGameState"></typeparam>
        /// <returns></returns>
        public GameState GetGameState<TGameState>()
        {
            GameState gs = null;

            _gameStates.ForEach(state =>
            {
                if (state.GetType() == typeof(TGameState))
                    gs = state;
            });


            if (gs == null)
                throw new ArgumentException("Unregistered gamestate: " + typeof(TGameState).Name);

            return gs;
        }

        /// <summary>
        /// Loads the provided GameState instance
        /// </summary>
        /// <param name = "newGameState">The GameState to load</param>
        public void LoadGameState(GameState newGameState)
        {
            newGameState.LoadingComplete += LoadingCompleteHandler;

            // Loading screen
            bool useLoading = ((_loadingState != null) && (CurrentGameState != null) && (CurrentGameState.UseLoading));
            if(useLoading)
            {
                _loadingState.Load();
                _loadingState.SetNextGameState(newGameState);
                changeCurrentGameState(_loadingState);

                _isLoadingScreenDisplayed = true;
            }

            // Load the game state
            if (useLoading)
            {
                new Thread(new ThreadStart(() =>
                {
                    newGameState.Load();
                })).Start();
            }
            else
            {
                newGameState.Load();
            }
        }

        private void LoadingCompleteHandler(GameState newGameState, bool loadingResult, Exception exception)
        {
            newGameState.LoadingComplete -= LoadingCompleteHandler;

            if (loadingResult)
            {
                if (_isLoadingScreenDisplayed)
                {
                    _loadingState.IsNextGameStateLoadingComplete = true;
                }
                else
                {
                    changeCurrentGameState(newGameState);
                }
            }
            else
            {
                throw new ApplicationException("Error while loading game state " + newGameState + ": " + exception, exception);
            }
        }

        private void changeCurrentGameState(GameState newGameState)
        {
            if (_currentGameState != null)
            {
                _currentGameState.IsActive = false;
            }
            _currentGameState = newGameState;
            _currentGameState.IsActive = true;
        }

        public GameState CurrentGameState
        {
            get { return _currentGameState; }
        }
    }
}
