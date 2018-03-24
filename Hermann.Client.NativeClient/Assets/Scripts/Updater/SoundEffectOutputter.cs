using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Containers;
using Hermann.Models;
using Assets.Scripts.Models;

namespace Assets.Scripts.Updater
{
    /// <summary>
    /// 効果音の出力機能を提供します。
    /// </summary>
    public class SoundEffectOutputter
    {
        /// <summary>
        /// 音管理機能
        /// </summary>
        private AudioManager AudioManager { get; set; }

        /// <summary>
        /// 初期化処理を行います。
        /// </summary>
        /// <param name="manager">音管理機能</param>
        public void Initialize(AudioManager manager)
        {
            this.AudioManager = manager;
        }

        /// <summary>
        /// 効果音を出力します。
        /// </summary>
        /// <param name="container">効果音に関する情報</param>
        public void Output(SoundEffectDecorationContainer container, Player.Index player)
        {
            foreach(var required in container.Required)
            {
                if (required.Value)
                {
                    if (required.Key == SoundEffect.Attack1 ||
                        required.Key == SoundEffect.Attack2 ||
                        required.Key == SoundEffect.Attack3)
                    {
                        AudioManager.ChangeSeVolume(0.8f);
                    }
                    else
                    {
                        AudioManager.ChangeSeVolume(0.08f);
                    }
                    AudioManager.PlaySE(required.Key.GetName(player).ToLower());
                }
            }
        }
    }
}
