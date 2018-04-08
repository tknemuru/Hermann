using Assets.Scripts.Helpers;
using Hermann.Client.ConsoleClient;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    /// <summary>
    /// 入力に関する管理機能を提供します。
    /// </summary>
    public class InputManager
    {
        /// <summary>
        /// キーコードのリスト
        /// </summary>
        private List<KeyCode>[] KeyCodes { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public InputManager()
        {
            this.KeyCodes = new List<KeyCode>[Player.Length];
            Player.ForEach(player =>
            {
                this.KeyCodes[(int)player] = new List<KeyCode>();
            });
        }

        /// <summary>
        /// キーコードを追加します。
        /// </summary>
        /// <param name="player">プレイヤ</param>
        /// <param name="direction">方向</param>
        public void AddKeyCode(Player.Index player, Direction direction)
        {
            this.KeyCodes[(int)player].Add(KeyMap.GetKeyCode(player, direction));
        }

        /// <summary>
        /// キーコードのリストを取得し、蓄積したキーコードをクリアします。
        /// </summary>
        /// <param name="player">プレイヤ</param>
        /// <returns>キーコードのリスト</returns>
        public KeyCode[] PullKeyCodes(Player.Index player)
        {
            var keyCodes = this.KeyCodes[(int)player].Concat(KeyMap.GetKeys().Select(k => k).
                    Where(k => Input.GetKeyDown(k) && KeyMap.GetPlayer(k) == player)).
                    ToArray();
            this.KeyCodes[(int)player].Clear();
            return keyCodes;
        }
    }
}
