using Assets.Scripts.Helpers;
using Hermann.Api.Containers;
using Hermann.Contexts;
using Hermann.Helpers;
using Hermann.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Updater
{
    /// <summary>
    /// フィールド情報をUIフィールドへ反映する機能を提供します。
    /// </summary>
    public class FieldContextReflector : ScriptableObject, IUiPlayerFieldParameterizedUpdatable<List<GameObject>, UiDecorationContainer>
    {
        /// <summary>
        /// オブジェクトの基底Z値
        /// </summary>
        private const float BaseZ = -1;

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
        /// <param name="container">フィールド情報コンテナ</param>
        public List<GameObject> Update(Player.Index player, UiDecorationContainer container)
        {
            var slimes = new List<GameObject>();
            this.ReflectSlimeField(player, container, slimes);
            this.ReflectNextSlimeField(player, container, slimes);
            return slimes;
        }

        /// <summary>
        /// スライム状態の反映を行います。
        /// </summary>
        /// <param name="player">プレイヤ</param>
        /// <param name="context">フィールド状態</param>
        /// <param name="container">フィールド情報コンテナ</param>
        private void ReflectSlimeField(Player.Index player, UiDecorationContainer container, List<GameObject> slimes)
        {
            for (var unit = 0; unit < FieldContextConfig.FieldUnitCount; unit++)
            {
                if(unit <= FieldContextConfig.MaxHiddenUnitIndex)
                {
                    continue;
                }

                for (var index = 0; index < FieldContextConfig.FieldUnitBitCount; index++)
                {
                    if(index % FieldContextConfig.OneLineBitCount < FieldContextConfig.OneLineBitCount - FieldContextConfig.VerticalLineLength)
                    {
                        continue;
                    }

                    // スライム色を取得
                    var color = FieldContextHelper.GetSlime(container.FieldContext, player, unit, index);
                    if(color == Slime.None)
                    {
                        continue;
                    }

                    var field = UiFieldHelper.GetPlayerField(player);
                    var slime = Instantiate(this.SlimeObject);
                    slimes.Add(slime);

                    // フィールドを親にセット
                    slime.transform.SetParent(field.transform);

                    // スライム初期化のために結合状態を取得
                    var joinStateIndex = ((unit - (FieldContextConfig.MaxHiddenUnitIndex + 1)) * FieldContextConfig.FieldUnitBitCount) + index;
                    var joinState = container.SlimeJoinStatus[(int)player][joinStateIndex];

                    // 初期化
                    var uiSlime = slime.GetComponent<UiSlime>();
                    uiSlime.Initialize(slime, player, color, joinState);

                    // サイズ・位置から座標を取得し、セット
                    var position = GetSlimeFieldPosition(TransformHelper.GetScaledSize(field.GetComponent<RectTransform>()).x,
                        TransformHelper.GetScaledSize(slime.GetComponent<Image>()).x,
                        TransformHelper.GetScaledSize(field.GetComponent<RectTransform>()).y,
                        TransformHelper.GetScaledSize(slime.GetComponent<Image>()).y,
                        unit,
                        index);
                    TransformHelper.SetPosition(slime.GetComponent<RectTransform>(), position);
                }
            }
        }

        /// <summary>
        /// Nextスライムの反映を行います。
        /// </summary>
        /// <param name="player">プレイヤ</param>
        /// <param name="context">フィールド状態</param>
        /// <param name="container">フィールド情報コンテナ</param>
        private void ReflectNextSlimeField(Player.Index player, UiDecorationContainer container, List<GameObject> slimes)
        {
            var nextField = UiFieldHelper.GetPlayerNextSlimeField(player);

            NextSlime.ForEach(next =>
            {
                MovableSlime.ForEach(movable =>
                {
                    var color = container.FieldContext.NextSlimes[(int)player][(int)next][(int)movable];
                    var field = UiFieldHelper.GetPlayerField(player);
                    var slime = Instantiate(this.SlimeObject);
                    slimes.Add(slime);

                    // フィールド上のスライムと同じサイズ調整を行うために、一度フィールドを親にセットする
                    slime.transform.SetParent(field.transform);

                    // 初期化
                    var uiSlime = slime.GetComponent<UiSlime>();
                    uiSlime.Initialize(slime, player, color, SlimeJoinState.Default);

                    // 初期化完了後にNextスライムフィールドを親にセットし直す
                    slime.transform.SetParent(nextField.transform);

                    // サイズ・位置から座標を取得し、セット
                    var position = GetNextSlimeFieldPosition(TransformHelper.GetScaledSize(nextField.GetComponent<RectTransform>()).y,
                        TransformHelper.GetScaledSize(slime.GetComponent<Image>()).y,
                        next,
                        movable);
                    TransformHelper.SetPosition(slime.GetComponent<RectTransform>(), position);
                });
            });
        }

        /// <summary>
        /// 指定したユニット・ユニット内のインデックスに対応したフィールド座標を取得します。
        /// </summary>
        /// <param name="fieldWidth">フィールドの横幅</param>
        /// <param name="slimeWidth">スライムの横幅</param>
        /// <param name="fieldHeight">フィールドの縦幅</param>
        /// <param name="slimeHeight">フィールドの縦幅</param>
        /// <param name="unit">フィールドユニット</param>
        /// <param name="index">フィールドインデックス</param>
        /// <returns>フィールド座標</returns>
        private static Vector3 GetSlimeFieldPosition(float fieldWidth, float slimeWidth, float fieldHeight, float slimeHeight, int unit, int index)
        {
            var x = (fieldWidth / 2f) - (slimeWidth / 2f);
            var y = (fieldHeight / 2f) - (slimeHeight / 2f);

            var line = FieldContextHelper.GetLineIndex(unit, index);
            var column = FieldContextHelper.GetColumnIndex(index);

            return new Vector3((x - (slimeWidth * column)), (y - (slimeHeight * line)), BaseZ);
        }

        /// <summary>
        /// 指定したNextスライムの座標を取得します。
        /// </summary>
        /// <param name="nextFieldHeight">Nextスライムフィールドの高さ</param>
        /// <param name="slimeHeight">スライムの高さ</param>
        /// <param name="nextIndex">Nextスライムのインデックス</param>
        /// <param name="movableIndex">移動可能スライムのインデックス</param>
        /// <returns>Nextスライムの座標</returns>
        private static Vector3 GetNextSlimeFieldPosition(float nextFieldHeight, float slimeHeight, NextSlime.Index nextIndex, MovableSlime.UnitIndex movableIndex)
        {
            var y = (nextFieldHeight / 2f) - (slimeHeight / 2f);

            var index = (int)nextIndex * NextSlime.Length + (int)movableIndex;
            var padding = index >= NextSlime.Length ? nextFieldHeight / ((MovableSlime.Length * NextSlime.Length) + 1) : 0f;

            return new Vector3(0f, y - ((slimeHeight * index) + padding));
        }
    }
}
