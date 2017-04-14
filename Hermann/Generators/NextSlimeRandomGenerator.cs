using Hermann.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Generators
{
    /// <summary>
    /// NEXTスライムのランダムな生成機能を提供します。
    /// </summary>
    public class NextSlimeRandomGenerator : NextSlimeGenerator
    {
        /// <summary>
        /// 乱数生成機能
        /// </summary>
        private Random RandomGen { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NextSlimeRandomGenerator()
        {
            this.RandomGen = new Random();
        }

        /// <summary>
        /// NEXTスライムを生成します。
        /// </summary>
        /// <returns>NEXTスライム</returns>
        protected override Slime[] GetNextSlime()
        {
            var slimes = new Slime[MovableSlime.Length];
            for (var i = 0; i < MovableSlime.Length; i++)
            {
                var index = this.RandomGen.Next(this.UsingSlime.Count() - 1);
                slimes[i] = this.UsingSlime[index];
            }

            return slimes;
        }
    }
}
