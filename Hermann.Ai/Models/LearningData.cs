using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Models
{
    /// <summary>
    /// 学習データ
    /// </summary>
    public class LearningData
    {
        /// <summary>
        /// 入力データ
        /// </summary>
        public double[][] Inputs { get; set; }

        /// <summary>
        /// 出力データ
        /// </summary>
        public double[][] Outputs { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public LearningData()
        {
            this.Inputs = new double[0][];
            this.Outputs = new double[0][];
        }
    }
}
