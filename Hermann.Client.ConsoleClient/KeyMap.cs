﻿using Hermann.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private static Dictionary<ConsoleKey, int> PlayerDic = builtPlayerDic();

        /// <summary>
        /// 方向辞書
        /// </summary>
        private static Dictionary<ConsoleKey, Direction> DirectionDic = builtDirectionDic();

        /// <summary>
        /// 指定したコンソールキーが操作対象のキーに含まれているかどうかを判定します。
        /// </summary>
        /// <param name="key">コンソールキー</param>
        /// <returns>指定したコンソールキーが操作対象のキーに含まれているかどうか</returns>
        public static bool ContainsKey(ConsoleKey key)
        {
            return PlayerDic.ContainsKey(key);
        }

        /// <summary>
        /// 指定したコンソールキーに対応したプレイヤを取得します。
        /// </summary>
        /// <param name="key">コンソールキー</param>
        /// <returns>プレイヤ</returns>
        public static int GetPlayer(ConsoleKey key)
        {
            if (!PlayerDic.ContainsKey(key))
            {
                throw new ArgumentException("不正なコンソールキーです。コンソールキー：" + key);
            }

            return PlayerDic[key];
        }

        /// <summary>
        /// 指定したコンソールキーに対応した方向を取得します。
        /// </summary>
        /// <param name="key">コンソールキー</param>
        /// <returns>方向</returns>
        public static Direction GetDirection(ConsoleKey key)
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
        private static Dictionary<ConsoleKey, int> builtPlayerDic()
        {
            var dic = new Dictionary<ConsoleKey, int>();
            dic.Add(ConsoleKey.UpArrow, Player.First);
            dic.Add(ConsoleKey.RightArrow, Player.First);
            dic.Add(ConsoleKey.DownArrow, Player.First);
            dic.Add(ConsoleKey.LeftArrow, Player.First);

            dic.Add(ConsoleKey.H, Player.Second);
            dic.Add(ConsoleKey.J, Player.Second);
            dic.Add(ConsoleKey.K, Player.Second);
            dic.Add(ConsoleKey.L, Player.Second);

            return dic;
        }

        /// <summary>
        /// 方向辞書を作成します。
        /// </summary>
        /// <returns>方向辞書</returns>
        private static Dictionary<ConsoleKey, Direction> builtDirectionDic()
        {
            var dic = new Dictionary<ConsoleKey, Direction>();
            dic.Add(ConsoleKey.UpArrow, Direction.Up);
            dic.Add(ConsoleKey.RightArrow, Direction.Right);
            dic.Add(ConsoleKey.DownArrow, Direction.Down);
            dic.Add(ConsoleKey.LeftArrow, Direction.Left);

            dic.Add(ConsoleKey.H, Direction.Left);
            dic.Add(ConsoleKey.J, Direction.Down);
            dic.Add(ConsoleKey.K, Direction.Up);
            dic.Add(ConsoleKey.L, Direction.Right);

            return dic;
        }
    }
}