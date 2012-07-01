using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperCaissiere.Engine.Content
{
    /// <summary>
    /// Declare ressources to use so they can be loaded with the application
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
    public abstract class ContentAttribute : Attribute
    {
        public string AssetName { get; set; }
        public string AssetPath { get; set; }
        public bool LoadOnStartup { get; set; }

        public ContentAttribute()
        {
            LoadOnStartup = true;
        }

        /// <summary>
        /// HACK due to an official Microsoft bug in multiple attributes reflection
        /// </summary>
        public override object TypeId
        {
            get
            {
                return this;
            }
        }
    }

    public sealed class TextureContentAttribute : ContentAttribute
    {
    }


    public sealed class Model3DContentAttribute : ContentAttribute
    {
    }

    public sealed class SoundEffectContentAttribute : ContentAttribute
    {
    }

    public sealed class FontContentAttribute : ContentAttribute
    {
        public bool IsDefaultFont { get; set; }
    }

    public sealed class SongContentAttribute : ContentAttribute
    {
        public string Artist { get; set; }
        public string Name { get; set; }
    }
}
