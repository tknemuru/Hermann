using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Ai.Models
{
    /// <summary>
    /// 2次元学習データ
    /// </summary>
    public class Learning2DData
    {
        /// <summary>
        /// 入力データ
        /// </summary>
        public double[][] Inputs { get; set; }

        /// <summary>
        /// 出力データ
        /// </summary>
        public double[] Outputs { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Learning2DData()
        {
            this.Inputs = new double[0][];
            this.Outputs = new double[0];
        }
    }
}
