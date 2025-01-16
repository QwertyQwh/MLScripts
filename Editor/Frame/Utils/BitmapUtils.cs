using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using UnityEngine;

namespace QFramework.Utils
{
    public static class BitmapUtils
    {
        private static Dictionary<string,Sprite> s_DictSpriteCache = new();

        public static Sprite LoadSprite(string path,string key)
        {
            if (s_DictSpriteCache.ContainsKey(key))
            {
                return s_DictSpriteCache[key];
            }
            if (string.IsNullOrEmpty(path)) return null;
            if (System.IO.File.Exists(path))
            {
                byte[] bytes = System.IO.File.ReadAllBytes(path);
                Texture2D texture = new Texture2D(2,2, TextureFormat.RGB24, false);
                texture.LoadImage(bytes);
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                s_DictSpriteCache[key] = sprite;
                return sprite;
            }
            return null;
        }
    }
}