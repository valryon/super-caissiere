using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SuperCaissiere.Engine.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace SuperCaissiere.Engine.Content
{
    /// <summary>
    /// Load and buffer content item of the game.
    /// Content items are declared on class that required them.
    /// </summary>
    public class MagicContentManager : Manager
    {
        private Dictionary<String, ContentItem> _data;
        private ContentManager _contentManager;
        private Assembly[] _assemblies;

        /// <summary>
        /// Assemblies to look at for the content load
        /// </summary>
        /// <param name="assemblies"></param>
        /// <param name="Content"></param>
        public MagicContentManager(Assembly[] assemblies,ContentManager Content)
        {
            _contentManager = Content;
            _assemblies = assemblies;
            _data = new Dictionary<string, ContentItem>();
        }


        public void Initialize()
        {
            // Get all ContentAttribute from all types
            var contentAttributes = (
                                        from assembly in _assemblies
                                        from t in assembly.GetTypes() // Get all types...
                                        let attributes = t.GetCustomAttributes(true).Where(a => a is ContentAttribute).ToArray()
                                        where attributes != null && attributes.Length > 0
                                        from att in attributes
                                        select att
                                    ).ToList();

            // Get textures, then sounds, fonts, etc
            var textures = (
                from att in contentAttributes
                where att is TextureContentAttribute
                let text = att as TextureContentAttribute
                select new Texture2DContentItem()
                {
                    AssetName = text.AssetName,
                    AssetPath = text.AssetPath,
                    LoadOnStartup = text.LoadOnStartup
                }
                ).ToList();

            textures.ForEach(t =>
            {
                addContentItem(t);
            });

            var sounds = (
                from att in contentAttributes
                where att is SoundEffectContentAttribute
                let sound = att as SoundEffectContentAttribute
                select new SoundEffectContentItem()
                {
                    AssetName = sound.AssetName,
                    AssetPath = sound.AssetPath,
                    LoadOnStartup = sound.LoadOnStartup
                }
                ).ToList();

            sounds.ForEach(s =>
            {
                addContentItem(s);
            });

            var musics = (
                from att in contentAttributes
                where att is SongContentAttribute
                let music = att as SongContentAttribute
                select new SongContentItem()
                {
                    AssetName = music.AssetName,
                    AssetPath = music.AssetPath,
                    LoadOnStartup = music.LoadOnStartup,
                    Artist = music.Artist,
                    Name = music.Name
                }
               ).ToList();

            musics.ForEach(s =>
            {
                addContentItem(s);
            });

            var fonts = (
                from att in contentAttributes
                where att is FontContentAttribute
                let font = att as FontContentAttribute
                select new FontContentItem()
                {
                    AssetName = font.AssetName,
                    AssetPath = font.AssetPath,
                    LoadOnStartup = font.LoadOnStartup,
                    IsDefaultFont = font.IsDefaultFont
                }
                ).ToList();

            fonts.ForEach(f =>
            {
                if (f.IsDefaultFont)
                    f.LoadOnStartup = true;

                addContentItem(f);
            });

            var models = (
               from att in contentAttributes
               where att is Model3DContentAttribute
               let font = att as Model3DContentAttribute
               select new Model3DContentItem()
               {
                   AssetName = font.AssetName,
                   AssetPath = font.AssetPath,
                   LoadOnStartup = font.LoadOnStartup
               }
               ).ToList();

            models.ForEach(f =>
            {

                addContentItem(f);
            });


            loadAllContent();
        }

        private void addContentItem(ContentItem ci)
        {
            if (_data.Keys.Contains(ci.AssetName))
            {
                //throw new ArgumentException("Duplicated content id: " + ci.AssetName);
                return;
            }

            _data.Add(ci.AssetName, ci);
        }

        /// <summary>
        /// Load every content that will be required later
        /// </summary>
        /// <param name="content"></param>
        private void loadAllContent()
        {
            foreach (var key in _data.Keys)
            {
                var item = _data[key];
                if ((item.LoadOnStartup) || (item.AssetName == "null"))
                {
                    loadContent(item);
                }
            }
        }

        private void loadContent(ContentItem item)
        {
            if (item is Texture2DContentItem)
            {
                var textItem = item as Texture2DContentItem;
                textItem.Texture = _contentManager.Load<Texture2D>(@item.AssetPath);

                if (textItem.AssetName == "null")
                {
                    EmptyTexture = textItem.Texture;
                }
            }
            else if (item is SoundEffectContentItem)
            {
                var soundItem = item as SoundEffectContentItem;
                soundItem.SoundEffect = _contentManager.Load<SoundEffect>(@item.AssetPath);
            }
            else if (item is FontContentItem)
            {
                var fontItem = item as FontContentItem;
                fontItem.Font = _contentManager.Load<SpriteFont>(@item.AssetPath);

                if (fontItem.IsDefaultFont)
                {
                    Font = fontItem.Font;
                }
            }
            else if (item is SongContentItem)
            {
                var songItem = item as SongContentItem;
                songItem.Song = _contentManager.Load<Song>(@item.AssetPath);
            }
            else if (item is Model3DContentItem)
            {
                var modelItem = item as Model3DContentItem;
                 modelItem.Model3D = _contentManager.Load<Model>(@item.AssetPath);
            }
            else
            {
                throw new ArgumentException("Unknow content type: " + item);
            }
            item.IsLoaded = true;
        }

        /// <summary>
        /// External texture add (Ogmo for example)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="texture"></param>
        /// <returns></returns>
        public void AddTexture(string id, Texture2D texture)
        {
            if (_data.Keys.Contains(id) == false)
            {
                _data.Add(id, new Texture2DContentItem()
                {
                    AssetName = id,
                    AssetPath = "",
                    IsLoaded = true,
                    Texture = texture
                });
            }
        }

        /// <summary>
        /// Retrieve a texture from its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Texture2D GetTexture(String id)
        {
            var item = GetContent(id);

            if (item is Texture2DContentItem)
                return ((Texture2DContentItem)item).Texture;

            throw new ArgumentException(id + " is not a texture content item ?!");
        }

        /// <summary>
        /// Retrieve a sound from its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SoundEffect GetSound(String id)
        {
            var item = GetContent(id);

            if (item is SoundEffectContentItem)
                return ((SoundEffectContentItem)item).SoundEffect;

            throw new ArgumentException(id + " is not a soundeffect content item ?!");
        }

        /// <summary>
        /// Retrieve a sound from one of the given ID, choosen randonly
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SoundEffect GetSound(params String[] ids)
        {
            String id = ids.ToList().GetRandomElement<String>();

            var item = GetContent(id);

            if (item is SoundEffectContentItem)
                return ((SoundEffectContentItem)item).SoundEffect;

            throw new ArgumentException(id + " is not a soundeffect content item ?!");
        }

        public Model3DContentItem GetModel(String id)
        {
            var item = GetContent(id);

            if (item is Model3DContentItem)
                return ((Model3DContentItem)item);

            throw new ArgumentException(id + " is not a model content item ?!");
        }




        /// <summary>
        /// Retrieve a music from its ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SongContentItem GetMusic(String id)
        {
            var item = GetContent(id);

            if (item is SongContentItem)
                return ((SongContentItem)item);

            throw new ArgumentException(id + " is not a song content item ?!");
        }

        /// <summary>
        /// Get a content from its Asset Name
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="ArgumentException">The content is not found</exception>
        /// <returns></returns>
        private ContentItem GetContent(String id)
        {
            ContentItem item = null;

            if (String.IsNullOrEmpty(id))
                throw new ArgumentException("Null or empty content id");

            if (_data.TryGetValue(id, out item))
            {
                if (item.IsLoaded == false)
                {
                    loadContent(item);
                }

                return item;
            }

            //Crash the game
            throw new ArgumentException("Missing content : " + id);
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime)
        {
            // Not drawn
        }

        /// <summary>
        /// Empty texture
        /// </summary>
        public Texture2D EmptyTexture { get; private set; }

        /// <summary>
        /// Default writing font
        /// </summary>
        public SpriteFont Font { get; private set; }
    }
}
