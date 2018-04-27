using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Learning.LearningClient.Excecuter
{
    /// <summary>
    /// 処理の実行機能を提供します。
    /// </summary>
    public interface IExecutable
    {
        /// <summary>
        /// 処理を実行します。
        /// </summary>
        /// <param name="args">パラメータ</param>
        void Execute(string[] args);
    }
}
