using Hermann.Contexts;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    /// <summary>
    /// UIフィールドに関する補助機能を提供します。
    /// </summary>
    public static class UiFieldHelper
    {
        /// <summary>
        /// 指定したプレイヤのフィールドを取得します。
        /// </summary>
        /// <param name="player">プレイヤ</param>
        /// <returns>指定したプレイヤのフィールド</returns>
        public static GameObject GetPlayerField(Player.Index player)
        {
            return GameObject.Find(GetFieldName(player));
        }

        /// <summary>
        /// 指定したプレイヤのNextスライムフィールドを取得します。
        /// </summary>
        /// <param name="player">プレイヤ</param>
        /// <returns>指定したプレイヤのNextスライムフィールド</returns>
        public static GameObject GetPlayerNextSlimeField(Player.Index player)
        {
            return GameObject.Find(GetNextSlimeFieldName(player));
        }

        /// <summary>
        /// 1列の横幅を取得します。
        /// </summary>
        /// <returns>1列の横幅</returns>
        public static float GetOneColumnWidth()
        {
            return TransformHelper.GetSize(GetPlayerField(Player.Index.First).GetComponent<RectTransform>()).x / FieldContextConfig.VerticalLineLength;
        }

        /// <summary>
        /// フィールド名を取得します。
        /// </summary>
        /// <param name="player">プレイヤ</param>
        /// <returns>フィールド名</returns>
        private static string GetFieldName(Player.Index player)
        {
            return player.GetName() + "Field";
        }

        /// <summary>
        /// Nextスライムフィールド名を取得します。
        /// </summary>
        /// <param name="player">プレイヤ</param>
        /// <returns>Nextスライムフィールド名</returns>
        private static string GetNextSlimeFieldName(Player.Index player)
        {
            return player.GetName() + "NextSlimeField";
        }
    }
}
