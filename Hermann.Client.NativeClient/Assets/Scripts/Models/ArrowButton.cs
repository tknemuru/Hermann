using Assets.Scripts.Di;
using Hermann.Di;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Models
{
    /// <summary>
    /// 矢印ボタン
    /// </summary>
    public class ArrowButton : MonoBehaviour
    {
        /// <summary>
        /// プレイヤ
        /// </summary>
        public Player.Index Player;

        /// <summary>
        /// 方向
        /// </summary>
        public Direction Direction;

        /// <summary>
        /// マウス押下時のコールバック関数
        /// </summary>
        void OnMouseDown()
        {
            DiProvider.GetContainer().GetInstance<InputManager>()
                .AddKeyCode(Player, Direction);
        }
    }
}
