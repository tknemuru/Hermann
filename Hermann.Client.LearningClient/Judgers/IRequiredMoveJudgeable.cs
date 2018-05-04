using System;
namespace Hermann.Client.LearningClient.Judgers
{
    /// <summary>
    /// スライムを動かす必要があるかどうかの判定機能を提供します。
    /// </summary>
    public interface IRequiredMoveJudgeable<T> : IJudgeable<T>
    {
    }
}
