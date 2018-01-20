using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Initializers
{
    /// <summary>
    /// 初期化機能を提供します。
    /// </summary>
    public interface IInitializable<TIn>
    {
        /// <summary>
        /// 渡されたオブジェクトに対する初期化処理を行います。
        /// </summary>
        /// <param name="input">初期化対象のオブジェクト</param>
        void Initialize(TIn input);
    }
}
