using Hermann.Models;
using Hermann.Updaters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Hermann.Client.ConsoleClient
{
    /// <summary>
    /// キーに関するマッピング情報を提供します。
    /// </summary>
    public static class KeyMap
    {
        /// <summary>
        /// プレイヤ辞書
        /// </summary>
        private static Dictionary<KeyCode, Player.Index> PlayerDic = builtPlayerDic();

        /// <summary>
        /// 方向辞書
        /// </summary>
        private static Dictionary<KeyCode, Direction> DirectionDic = builtDirectionDic();

        /// <summary>
        /// 指定したキーコードが操作対象のキーに含まれているかどうかを判定します。
        /// </summary>
        /// <param name="key">キーコード</param>
        /// <returns>指定したコンソールキーが操作対象のキーに含まれているかどうか</returns>
        public static bool ContainsKey(KeyCode key)
        {
            return PlayerDic.ContainsKey(key);
        }

        /// <summary>
        /// 入力を受け付けるキーリストを取得します。
        /// </summary>
        /// <returns>入力を受け付けるキーリスト</returns>
        public static IEnumerable<KeyCode> GetKeys()
        {
            return PlayerDic.Keys;
        }

        /// <summary>
        /// 指定したキーコードに対応したプレイヤを取得します。
        /// </summary>
        /// <param name="key">キーコード</param>
        /// <returns>プレイヤ</returns>
        public static Player.Index GetPlayer(KeyCode key)
        {
            if (!PlayerDic.ContainsKey(key))
            {
                throw new ArgumentException("不正なコンソールキーです。コンソールキー：" + key);
            }

            return PlayerDic[key];
        }

        /// <summary>
        /// 指定したキーコードに対応した方向を取得します。
        /// </summary>
        /// <param name="key">キーコード</param>
        /// <returns>方向</returns>
        public static Direction GetDirection(KeyCode key)
        {
            if (!DirectionDic.ContainsKey(key))
            {
                throw new ArgumentException("不正なコンソールキーです。コンソールキー：" + key);
            }

            return DirectionDic[key];
        }

        /// <summary>
        /// プレイヤ辞書を作成します。
        /// </summary>
        /// <returns></returns>
        private static Dictionary<KeyCode, Player.Index> builtPlayerDic()
        {
            var dic = new Dictionary<KeyCode, Player.Index>();
            dic.Add(KeyCode.UpArrow, Player.Index.First);
            dic.Add(KeyCode.RightArrow, Player.Index.First);
            dic.Add(KeyCode.DownArrow, Player.Index.First);
            dic.Add(KeyCode.LeftArrow, Player.Index.First);

            dic.Add(KeyCode.Keypad8, Player.Index.Second);
            dic.Add(KeyCode.Keypad6, Player.Index.Second);
            dic.Add(KeyCode.Keypad2, Player.Index.Second);
            dic.Add(KeyCode.Keypad4, Player.Index.Second);

            return dic;
        }

        /// <summary>
        /// 方向辞書を作成します。
        /// </summary>
        /// <returns>方向辞書</returns>
        private static Dictionary<KeyCode, Direction> builtDirectionDic()
        {
            var dic = new Dictionary<KeyCode, Direction>();
            dic.Add(KeyCode.UpArrow, Direction.Up);
            dic.Add(KeyCode.RightArrow, Direction.Right);
            dic.Add(KeyCode.DownArrow, Direction.Down);
            dic.Add(KeyCode.LeftArrow, Direction.Left);

            dic.Add(KeyCode.Keypad4, Direction.Left);
            dic.Add(KeyCode.Keypad2, Direction.Down);
            dic.Add(KeyCode.Keypad8, Direction.Up);
            dic.Add(KeyCode.Keypad6, Direction.Right);

            return dic;
        }
    }
}
