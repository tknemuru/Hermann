using Hermann.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Helper
{
    /// <summary>
    /// 時間計測と計測結果の出力処理を提供します。
    /// </summary>
    public static class StopWatchLogger
    {
        /// <summary>
        /// <para>デバッグモードの場合はTrue</para>
        /// </summary>
        public const bool IsDebug = true;

        /// <summary>
        /// <para>コンソールに出力する場合はTrue</para>
        /// </summary>
        private const bool IsConsole = false;

        /// <summary>
        /// <para>詳細な計測時間を出力する場合はTrue</para>
        /// </summary>
        private const bool IsDetailTimeDisplay = true;

        /// <summary>
        /// <para>ストップウォッチディクショナリ</para>
        /// </summary>
        private static Dictionary<string, Stopwatch> StopWatchDic { get; set; }

        /// <summary>
        /// <para>時間記録ディクショナリ</para>
        /// </summary>
        private static Dictionary<string, long> Times { get; set; }

        /// <summary>
        /// <para>詳細な時間記録ディクショナリ</para>
        /// </summary>
        private static Dictionary<string, double> DetailTimes { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static StopWatchLogger()
        {
            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            ClearAllEventTimes();
        }

        /// <summary>
        /// <para>文字列をコンソールに出力する</para>
        /// </summary>
        /// <param name="str"></param>
        public static void OutputStringToConsole(string str)
        {
            if (!IsDebug) { return; }
            Debug.WriteLine(str);
        }

        /// <summary>
        /// <para>イベント時間の計測を開始する</para>
        /// </summary>
        /// <param name="eventName"></param>
        public static void StartEventWatch(string eventName)
        {
            if (!IsDebug) { return; }
            if (!StopWatchDic.ContainsKey(eventName))
            {
                StopWatchDic.Add(eventName, new Stopwatch());
                StopWatchDic[eventName].Start();
            }
            else
            {
                StopWatchDic[eventName].Start();
            }
        }

        /// <summary>
        /// <para>イベント時間の計測を終了する</para>
        /// </summary>
        /// <param name="eventName"></param>
        public static void StopEventWatch(string eventName)
        {
            if (!IsDebug) { return; }
            Debug.Assert(StopWatchDic.ContainsKey(eventName), "計測が開始されていないイベントの計測を終了しようとしました。");
            StopWatchDic[eventName].Stop();
            AddEventTimes(eventName);
        }

        /// <summary>
        /// <para>イベントの時間を記録する</para>
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="ms"></param>
        private static void AddEventTimes(string eventName)
        {
            if (!IsDebug) { return; }
            Debug.Assert(StopWatchDic.ContainsKey(eventName) || !(StopWatchDic[eventName].IsRunning), "計測が完了していないイベントの計測を記録しようとしました。");
            if (!Times.ContainsKey(eventName))
            {
                Times.Add(eventName, StopWatchDic[eventName].ElapsedMilliseconds);
                DetailTimes.Add(eventName, (double)StopWatchDic[eventName].ElapsedTicks / (double)Stopwatch.Frequency);
            }
            else
            {
                Times[eventName] = Times[eventName] + StopWatchDic[eventName].ElapsedMilliseconds;
                DetailTimes[eventName] = DetailTimes[eventName] + ((double)StopWatchDic[eventName].ElapsedTicks / (double)Stopwatch.Frequency);
            }
            StopWatchDic[eventName].Reset();
        }

        /// <summary>
        /// <para>記録したすべてのイベントの時間を出力する</para>
        /// </summary>
        public static void DisplayAllEventTimes()
        {
            if (!IsDebug) { return; }

            if (IsDetailTimeDisplay)
            {
                foreach (KeyValuePair<string, double> dTime in DetailTimes)
                {
                    Debug.WriteLine(dTime.Key + "：" + dTime.Value.ToString());
                }
            }
        }

        /// <summary>
        /// <para>記録したすべてのイベントの時間を出力する</para>
        /// </summary>
        public static void WriteAllEventTimes(string file)
        {
            foreach (KeyValuePair<string, double> dTime in DetailTimes)
            {
                FileHelper.WriteLine((dTime.Key + "：" + dTime.Value.ToString()), file);
            }
        }

        /// <summary>
        /// <para>イベント時間記録ディクショナリをクリアする</para>
        /// </summary>
        public static void ClearAllEventTimes()
        {
            if (!IsDebug) { return; }
            Times = new Dictionary<string, long>();
            StopWatchDic = new Dictionary<string, Stopwatch>();
            DetailTimes = new Dictionary<string, double>();
        }
    }
}
