using System;
namespace Hermann.Client.LearningClient.Judgers
{
    /// <summary>
    /// フレーム数を用いてスライムを動かす必要があるかどうかを判定する機能を提供します。
    /// </summary>
    public class RequiredMoveFrameCountJudger : IRequiredMoveJudgeable<int>
    {
        /// <summary>
        /// スライムの移動を要求するフレーム数
        /// </summary>
        private const int RequiredMoveFrameCount = 16;

        /// <summary>
        /// フレーム数からスライムを動かす必要があるかどうかを判定します。
        /// </summary>
        /// <returns>スライムを動かす必要があるかどうか</returns>
        /// <param name="frameCount">フレーム数</param>
        public bool Judge(int frameCount)
        {
            return frameCount % RequiredMoveFrameCount == 0 || frameCount % (RequiredMoveFrameCount - 1) == 0;
        }
    }
}
