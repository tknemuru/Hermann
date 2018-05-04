using System;
namespace Hermann.Client.LearningClient.Judgers
{
    /// <summary>
    /// 判定機能を提供します。
    /// </summary>
    public interface IJudgeable<T>
    {
        /// <summary>
        /// 判定を実施します。
        /// </summary>
        /// <returns>判定結果</returns>
        /// <param name="input">入力情報</param>
        bool Judge(T input);
    }
}
