using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SuperCaissiere.Engine.Core;

namespace System.Collections.Generic
{
    public static class ListExtension
    {
        public static T GetRandomElement<T>(this List<T> list)
        {
            return list[Application.Random.GetRandomInt(list.Count)];
        }
    }
}
