using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperCaissiere.Engine.Core;


namespace SuperCaissiere.Engine.Graphics
{
    public class Model3DRenderer
    {

        private GraphicsDevice graphicDevice;
        private Matrix world, view, projection;
        private Vector3 Rotation;
        private SpriteBatchProxy spriteBatch;
        private RenderTarget2D tmpTarget;
        private Model model, modelcol;
        private Random randomizator;
        private float time = 0.00f;
        public bool isActive { set; get; }

        public Model3DRenderer(GraphicsDevice _dev, SpriteBatchProxy _sprbtch, Matrix _proj, Matrix _view, Matrix _world)
        {
            Rotation = new Vector3();
            world = _world;
            projection = _proj;
            view = _view;
            graphicDevice = _dev;
            spriteBatch = _sprbtch;
            tmpTarget = new RenderTarget2D(graphicDevice, graphicDevice.Viewport.Width, graphicDevice.Viewport.Height);
            randomizator = new Random((int)System.DateTime.Now.Ticks);
            isActive = false;
        }

        public void setModel(Model _model, Model _modelcol)
        {
            model = _model;
            modelcol = _modelcol;
            getRandomRotation();
        }

        private void getRandomRotation()
        {

            world *= Matrix.CreateRotationX((float)randomizator.NextDouble() * 360);
            world *= Matrix.CreateRotationY((float)randomizator.NextDouble() * 360);
            world *= Matrix.CreateRotationZ((float)randomizator.NextDouble() * 360);
        }

        public void resetRotate()
        {
            Rotation = Vector3.Zero;
        }

        public void rotateX(float x)
        {
            Rotation.X = x;
        }
        public void rotateY(float y)
        {
            Rotation.Y = y;
        }
        public void rotateZ(float z)
        {
            Rotation.Z = z;
        }
        public void Update(GameTime gameTime)
        {
            if (!isActive) return;
            world *= Matrix.CreateRotationX(Rotation.X);
            world *= Matrix.CreateRotationY(Rotation.Y);
            world *= Matrix.CreateRotationZ(Rotation.Z);
        }


        private void draw(Model _m)
        {
            if (!isActive) return;
            if (_m == null) return;
            foreach (ModelMesh mesh in _m.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.Projection = projection;
                    be.View = view;
                    be.World = world;
                }
                mesh.Draw();
            }
        }


        public void Draw()
        {
            draw(model);
        }



        public bool isClicked(int mouseX, int mouseY)
        {
            if (!(mouseX >= graphicDevice.Viewport.Width || mouseX < 0 || mouseY >= graphicDevice.Viewport.Height || mouseY < 0))
            {
                Texture2D capture = sceneToTexture();
                Color[] map = TextureTo2DArray(capture);
                //Console.WriteLine("Get " + map[mouseX + mouseY * capture.Width]);
                Color c = map[mouseX + mouseY * capture.Width];
                if (c.R > c.G && c.R > c.B)
                {
                    return true;
                }
            }
            return false;
        }

        protected Texture2D sceneToTexture()
        {
            graphicDevice.SetRenderTarget(tmpTarget);
            graphicDevice.Clear(Color.CornflowerBlue);
            Application.SpriteBatch.BeginNoCamera();

            draw(modelcol);
            Application.SpriteBatch.End();
            graphicDevice.SetRenderTarget(null);
            return tmpTarget;
        }


        private Color[] TextureTo2DArray(Texture2D texture)
        {
            Color[] colors1D = new Color[texture.Width * texture.Height];
            texture.GetData(colors1D);
            return colors1D;
        }
    }
}
