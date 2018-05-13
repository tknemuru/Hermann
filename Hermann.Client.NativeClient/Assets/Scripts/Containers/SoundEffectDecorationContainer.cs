using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hermann.Contexts;
using Assets.Scripts.Models;
using Hermann.Models;

namespace Assets.Scripts.Containers
{
    /// <summary>
    /// 音の演出に関する情報を格納します。
    /// </summary>
    public class SoundEffectDecorationContainer
    {
        /// <summary>
        /// 前回のフィールド状態
        /// </summary>
        public FieldContext LastFieldContext { get; set; }

        /// <summary>
        /// おじゃまスライム数
        /// </summary>
        public int ObstructionSlimeCount { get; set; }

        /// <summary>
        /// 効果音の出力要求情報
        /// </summary>
        public Dictionary<SoundEffect, bool> Required { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SoundEffectDecorationContainer()
        {
            this.ClearRequired();
        }

        /// <summary>
        /// 効果音要求情報をクリアします。
        /// </summary>
        public void ClearRequired()
        {
            this.Required = ExtensionSoundEffect.SoundEffects.ToDictionary(e => e, e => false);
        }

        /// <summary>
        /// オブジェクトを文字列化します。
        /// </summary>
        /// <returns>文字列化したオブジェクト</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            if (this.LastFieldContext != null)
            {
                sb.AppendLine(string.Format("LastFieldContext[FieldEvent]{0}|{1}",
                    this.LastFieldContext.FieldEvent[(int)Player.Index.First],
                    this.LastFieldContext.FieldEvent[(int)Player.Index.Second]));
                sb.AppendLine(string.Format("LastFieldContext[Ground]{0}|{1}",
                    this.LastFieldContext.Ground[(int)Player.Index.First],
                    this.LastFieldContext.Ground[(int)Player.Index.Second]));
            }

            foreach (var r in this.Required)
            {
                sb.AppendLine(string.Format("[{0}]{1}", r.Key, r.Value));
            }
            sb.AppendLine(string.Format("ObstructionSlimeCount:{0}", this.ObstructionSlimeCount.ToString()));
            return sb.ToString();
        }
    }
}
