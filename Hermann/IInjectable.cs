using System;
namespace Hermann
{
    /// <summary>
    /// 依存する機能や情報の注入機能を提供します。
    /// </summary>
    public interface IInjectable<T>
    {
        /// <summary>
        /// 依存する機能や情報を注入します。
        /// </summary>
        /// <param name="input">依存機能・情報</param>
        void Inject(T input);

        /// <summary>
        /// 注入が完了したかどうかを判定します。
        /// </summary>
        /// <returns><c>true</c>, if injected was hased, <c>false</c> otherwise.</returns>
        bool HasInjected { get; }
    }
}
