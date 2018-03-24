using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hermann.Models;

namespace Assets.Scripts.Models
{
    /// <summary>
    /// 効果音
    /// </summary>
    public enum SoundEffect
    {
        /// <summary>
        /// 攻撃レベル1
        /// </summary>
        Attack1,

        /// <summary>
        /// 攻撃レベル2
        /// </summary>
        Attack2,

        /// <summary>
        /// 攻撃レベル3
        /// </summary>
        Attack3,

        /// <summary>
        /// 相殺
        /// </summary>
        Offset,

        /// <summary>
        /// 削除
        /// </summary>
        Erase,

        /// <summary>
        /// 接地
        /// </summary>
        Ground,

        /// <summary>
        /// 移動
        /// </summary>
        Move,

        /// <summary>
        /// おじゃまスライム落下
        /// </summary>
        Obstructions,

        /// <summary>
        /// 単発おじゃまスライム落下
        /// </summary>
        SingleObstruction,
    }

    /// <summary>
    /// 効果音拡張
    /// </summary>
    public static class ExtensionSoundEffect
    {
        /// <summary>
        /// 効果音リスト
        /// </summary>
        public static readonly IEnumerable<SoundEffect> SoundEffects;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static ExtensionSoundEffect()
        {
            SoundEffects = ((IEnumerable<SoundEffect>)Enum.GetValues(typeof(SoundEffect)));
        }

        /// <summary>
        /// 効果音の名称を取得します。
        /// </summary>
        /// <param name="se">効果音</param>
        /// <returns>効果音の名称</returns>
        public static string GetName(this SoundEffect se, Player.Index player)
        {
            var name = string.Empty;
            switch (se)
            {
                case SoundEffect.Attack1:
                    name = player.GetName() + "_Attack1";
                    break;
                case SoundEffect.Attack2:
                    name = player.GetName() + "_Attack2";
                    break;
                case SoundEffect.Attack3:
                    name = player.GetName() + "_Attack3";
                    break;
                case SoundEffect.Offset:
                    name = "Offset";
                    break;
                case SoundEffect.Erase:
                    name = "Erase";
                    break;
                case SoundEffect.Ground:
                    name = "Ground";
                    break;
                case SoundEffect.Move:
                    name = "Move";
                    break;
                case SoundEffect.Obstructions:
                    name = "Obstructions";
                    break;
                case SoundEffect.SingleObstruction:
                    name = "Single_Obstruction";
                    break;
                default:
                    throw new ArgumentException("SoundEffectが不正です");
            }
            return name;
        }
    }
}
