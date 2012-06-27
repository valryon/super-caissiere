using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;


namespace SuperCaissiere.Engine.Graphics
{
    public class Model3DRenderer
    {

        GraphicsDevice graphicDevice;
        Matrix World, View, Projection;
        Vector3 Rotation;
        SpriteBatchProxy spriteBatch;
        RenderTarget2D tmpTarget;
        Model model, modelcol;
        Random randomizator;
        float time = 0.00f;


        public Model3DRenderer(GraphicsDevice _dev, SpriteBatchProxy _sprbtch, Matrix _proj, Matrix _view, Matrix _world)
        {
            Rotation = new Vector3();
            World = _world;
            Projection = _proj;
            View = _view;
            graphicDevice = _dev;
            spriteBatch = _sprbtch;
            tmpTarget = new RenderTarget2D(graphicDevice, graphicDevice.Viewport.Width, graphicDevice.Viewport.Height);
            randomizator = new Random((int)System.DateTime.Now.Ticks);
        }

        public void setModel(Model _model, Model _modelcol)
        {
            model = _model;
            modelcol = _modelcol;
            getRandomRotation();
        }

        private void getRandomRotation()
        {

            World *= Matrix.CreateRotationX((float)randomizator.NextDouble() * 360);
            World *= Matrix.CreateRotationY((float)randomizator.NextDouble() * 360);
            World *= Matrix.CreateRotationZ((float)randomizator.NextDouble() * 360);
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
            World *= Matrix.CreateRotationX(Rotation.X);
            World *= Matrix.CreateRotationY(Rotation.Y);
            World *= Matrix.CreateRotationZ(Rotation.Z);
            time += 0.01f;
        }


        private void draw(Model _m)
        {
            if (_m == null) return;
            foreach (ModelMesh mesh in _m.Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.Projection = Projection;
                    be.View = View;
                    be.World = World;
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
            spriteBatch.Begin(null);

            draw(modelcol);
            spriteBatch.End();
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
