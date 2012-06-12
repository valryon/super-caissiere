using Microsoft.Xna.Framework;
using System;
using SuperCaissiere.Engine.Graphics;

namespace SuperCaissiere.Engine.Core
{
    public class Camera
    {
        public const float ZoomInMaxValue = 5f;
        public const float ZoomOutMaxValue = 0.3f;

        private Vector2 _position; // Camera top-left corner
        private float _rotation; // Camera Rotation
        private Matrix _transform; // Matrix Transform
        private Rectangle _visibilityRectangle, _awarenessRectangle;
        private float _zoom; // Camera Zoom

        private bool _useBounds;
        private Rectangle _bounds;
        private Color _fadingColor;

        // Shake
        private Vector2 _currentShakeCooldown;

        // Fade
        private float _currentFadeInOut, _currentFadeDelta;
        private Action _onFadeComplete;

        public Camera()
        {
            Reset();
        }

        /// <summary>
        /// Set default values for the camera
        /// </summary>
        public void Reset()
        {
            _position = new Vector2(Resolution.DeviceWidth / 2, Resolution.DeviceHeight / 2);
            _zoom = 1.0f;
            _rotation = 0.0f;
            _useBounds = false;

            ShakeSpeed = new Vector2(50f, 100f);
            _currentShakeCooldown = ShakeSpeed;
        }

        /// <summary>
        /// Update camera matrix and visibility rectangle
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // Bounds
            if (_useBounds)
            {
                int x = (int)(_position.X - ((Resolution.DeviceWidth / 2) / Zoom));
                int y = (int)(_position.Y - ((Resolution.DeviceHeight / 2) / Zoom));

                if (x < _bounds.Left)
                {
                    _position.X = _bounds.Left + (Resolution.DeviceWidth / 2 / Zoom);
                }
                else if (x + (int)(Resolution.DeviceWidth / Zoom) > _bounds.Right)
                {
                    _position.X = _bounds.Right - (Resolution.DeviceWidth / 2 / Zoom);
                }

                // No top bounds
                //if (y < _bounds.Top)
                //{
                //    _position.Y = _bounds.Top + (Resolution.DeviceHeight / 2 / Zoom);
                //}
                if (y + (int)(Resolution.DeviceHeight / Zoom) > _bounds.Bottom)
                {
                    _position.Y = _bounds.Bottom - (Resolution.DeviceHeight / 2 / Zoom);
                }

            }

            if (ShakeFactor != Vector2.Zero)
            {
                _currentShakeCooldown.X -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                _currentShakeCooldown.Y -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                if (_currentShakeCooldown.X <= 0)
                {
                    Move(new Vector2(ShakeFactor.X, 0));
                    ShakeFactor = new Vector2(-ShakeFactor.X, ShakeFactor.Y);

                    _currentShakeCooldown.X = ShakeSpeed.X;
                }

                if (_currentShakeCooldown.Y <= 0)
                {
                    Move(new Vector2(0, ShakeFactor.Y));
                    ShakeFactor = new Vector2(ShakeFactor.X, -ShakeFactor.Y);

                    _currentShakeCooldown.Y = ShakeSpeed.Y;
                }
            }

            // Fade in out
            _currentFadeInOut += _currentFadeDelta;
            if (_currentFadeInOut > 1.0f)
            {
                _currentFadeInOut = 1.0f;
                _currentFadeDelta = 0f;
                if (_onFadeComplete != null) _onFadeComplete();
            }
            else if (_currentFadeInOut < 0.0f)
            {
                _currentFadeInOut = 0.0f;
                _currentFadeDelta = 0f;
                if (_onFadeComplete != null) _onFadeComplete();
            }

            // Create the camera
            _transform = Matrix.CreateTranslation(new Vector3(-_position.X, -_position.Y, 0)) *
                         Matrix.Identity * /* For reliable results */
                         Matrix.CreateRotationZ(_rotation) *
                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                         Matrix.CreateTranslation(new Vector3(Resolution.DeviceWidth * 0.5f, Resolution.DeviceHeight * 0.5f, 0));

            // Apply the resolution trick
            _transform *= Resolution.ResolutionMatrix;

            // Visibility rectangle
            _visibilityRectangle = new Rectangle
            {
                X = (int)(_position.X - ((Resolution.DeviceWidth / 2) / Zoom)),
                Y = (int)(_position.Y - ((Resolution.DeviceHeight / 2) / Zoom)),
                Width = (int)(Resolution.DeviceWidth / Zoom),
                Height = (int)(Resolution.DeviceHeight / Zoom)
            };

            _awarenessRectangle = _visibilityRectangle;
            _awarenessRectangle.Inflate(Resolution.VirtualWidth, Resolution.VirtualHeight);
        }

        public Matrix ApplyParallax(Vector2 parallax)
        {
            var transform = Matrix.CreateTranslation(new Vector3(-_position * parallax, 0)) *
                         Matrix.Identity * /* For reliable results */
                         Matrix.CreateRotationZ(_rotation) *
                         Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                         Matrix.CreateTranslation(new Vector3(Resolution.DeviceWidth * 0.5f, Resolution.DeviceHeight * 0.5f, 0));

            // Apply the resolution trick
            transform *= Resolution.ResolutionMatrix;

            return transform;
        }

        public void SetBounds(Rectangle bounds)
        {
            _bounds = bounds;
            _useBounds = true;
        }

        /// <summary>
        /// Translate the camera
        /// </summary>
        /// <param name="amount"></param>
        public void Move(Vector2 amount)
        {
            _position += amount;
        }

        /// <summary>
        /// Fade screen to black
        /// </summary>
        /// <param name="frameCount">Number of frames during wich the fade will be visible</param>
        /// <param name="onFadeComplete">Called when the fade is complete</param>
        public void FadeIn(float frameCount, Action onFadeComplete)
        {
            FadeIn(frameCount, onFadeComplete, Color.Black);
        }
        public void FadeIn(float frameCount, Action onFadeComplete, Color col)
        {
            _currentFadeInOut = 0.0f;
            _currentFadeDelta = (1 / frameCount);
            _fadingColor = col;
            _onFadeComplete = onFadeComplete;
        }

        /// <summary>
        /// Fade screen from black to normal
        /// </summary>
        /// <param name="frameCount">Number of frames during wich the fade will be visible</param>
        /// <param name="onFadeComplete">Called when the fade is complete</param>
        public void FadeOut(float frameCount, Action onFadeComplete)
        {
            FadeOut(frameCount,onFadeComplete, Color.Black);
        }

        public void FadeOut(float frameCount, Action onFadeComplete, Color col)
        {
            _currentFadeInOut = 1.0f;
            _currentFadeDelta = -(1 / frameCount);
            _fadingColor = col;
            _onFadeComplete = onFadeComplete;
        }



        public void DrawFade(SpriteBatchProxy spriteBatch)
        {
            spriteBatch.DrawRectangle(new Rectangle(0, 0, Resolution.VirtualWidth, Resolution.VirtualHeight), _fadingColor * _currentFadeInOut);
        }

        public Matrix CameraMatrix
        {
            get
            {
                return _transform;
            }
        }

        public Rectangle VisibilityRectangle
        {
            get
            {
                return _visibilityRectangle;
            }
        }

        public Rectangle AwarenessRectangle
        {
            get
            {
                return _awarenessRectangle;
            }
        }

        /// <summary>
        /// Zoom in or out
        /// </summary>
        public float Zoom
        {
            get { return _zoom; }
            set
            {
                _zoom = MathHelper.Clamp(value, ZoomOutMaxValue, ZoomInMaxValue);
            }
        }

        /// <summary>
        /// Rotate the camera
        /// </summary>
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }

        /// <summary>
        /// This is the center of the camera
        /// </summary>
        public Vector2 Location
        {
            get { return _position; }
            set { _position = value; }
        }

        /// <summary>
        /// Make the camera shake
        /// </summary>
        public Vector2 ShakeFactor
        {
            get;
            set;
        }

        /// <summary>
        /// Shaking speed (in millisceonds)
        /// </summary>
        public Vector2 ShakeSpeed
        {
            get;
            set;
        }

        /// <summary>
        /// Transform a Camera position into a world position
        /// </summary>  
        /// <remarks>Example: Mouse pointer on the screen to world coordinates</remarks>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector2 ToWorldLocation(Vector2 position)
        {
            return Vector2.Transform(position, Matrix.Invert(_transform));
        }

        /// <summary>
        /// Transform a world position into a camera one
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector2 ToLocalLocation(Vector2 position)
        {
            return Vector2.Transform(position, _transform);
        }
    }
}