using Hermann.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    /// <summary>
    /// スプライトに関する補助機能を提供します。
    /// </summary>
    public class SpriteHelper
    {
        /// <summary>
        /// 分割したスプライト画像辞書
        /// </summary>
        private static Dictionary<string, Sprite> Sprites = BuildSprites();

        /// <summary>
        /// 指定したファイル名の分割したスプライト画像を取得します。
        /// </summary>
        /// <param name="name">ファイル名</param>
        /// <returns>指定したファイル名の分割したスプライト画像</returns>
        public static Sprite GetSprite(string name)
        {
            Sprite sprite = null;
            try
            {
                sprite = Sprites[name];
            }
            catch(Exception ex)
            {
                FileHelper.WriteLine(ex.ToString());
                FileHelper.WriteLine("例外発生時名称：" + name);
            }
            return sprite;
        }

        /// <summary>
        /// 分割したスプライト画像辞書を作成します。
        /// </summary>
        /// <returns>分割したスプライト画像辞書</returns>
        private static Dictionary<string, Sprite> BuildSprites()
        {
            var dic = new Dictionary<string, Sprite>();
            var sprites = Resources.LoadAll<Sprite>("Sprites/slime");
            foreach(var s in sprites)
            {
                dic.Add(s.name, s);
            }
            return dic;
        }
    }
}
