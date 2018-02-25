using Hermann.Contexts;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Updater
{
    /// <summary>
    /// フィールド情報のUIフィールドへの反映機能を提供します。
    /// </summary>
    public class FieldContextReflector : ScriptableObject, IUiPlayerFieldParameterizedUpdatable<FieldContext>
    {
        /// <summary>
        /// スライムオブジェクト
        /// </summary>
        private GameObject SlimeObject { get; set; }

        /// <summary>
        /// 初期化処理
        /// </summary>
        /// <param name="slime">スライム</param>
        public void Initialize(GameObject slime)
        {
            this.SlimeObject = slime;
        }

        public void Update()
        {
        }

        /// <summary>
        /// 指定したプレイヤのUIフィールド状態を更新します。
        /// </summary>
        /// <param name="player">プレイヤ</param>
        /// <param name="param">パラメータ</param>
        public void Update(Player.Index player, FieldContext param)
        {
            for (var unit = 0; unit < FieldContextConfig.FieldUnitCount; unit++)
            {
                var slime = Slime.None;
                if (unit == 2)
                {
                    slime = Slime.Blue;
                }
                else if (unit == 3)
                {
                    slime = Slime.Green;
                }
                else if (unit == 4)
                {
                    slime = Slime.Yellow;
                }
                else
                {
                    slime = Slime.Red;
                }

                for (var index = 0; index < FieldContextConfig.FieldUnitBitCount; index++)
                {
                    var slimeObj = Instantiate(this.SlimeObject);
                    var uiSlime = slimeObj.GetComponent<UiSlime>();
                    uiSlime.Initialize(slimeObj, player, unit, index, slime);
                }
            }
        }
    }
}
