using Hermann.Contexts;
using Hermann.Models;
using Hermann.Api.Receivers;
using Hermann.Api.Senders;
using Hermann.Tests.Di;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using Hermann.Updaters;

namespace Hermann.Tests.TestHelpers
{
    /// <summary>
    /// テストで使用する補助機能を提供します。
    /// </summary>
    public static class TestHelper
    {
        /// <summary>
        /// DIコンテナ
        /// </summary>
        public static Container Container { get; private set; }

        /// <summary>
        /// 受信機能
        /// </summary>
        public static SimpleTextReceiver Receiver { get; private set; }

        /// <summary>
        /// 送信機能
        /// </summary>
        public static SimpleTextSender Sender { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static TestHelper()
        {
            Container = TestDiProvider.GetContainer();
            Receiver = Container.GetInstance<SimpleTextReceiver>();
            Sender = Container.GetInstance<SimpleTextSender>();
        }

        /// <summary>
        /// 指定した行数のシフト量を取得します。
        /// </summary>
        /// <param name="lineIndex">何行目か</param>
        /// <returns>シフト量</returns>
        public static int GetShift(int lineIndex)
        {
            return ((lineIndex + (FieldContextConfig.FieldUnitLineCount - 1)) % FieldContextConfig.FieldUnitLineCount) * FieldContextConfig.OneLineBitCount;
        }

        /// <summary>
        /// 指定した行・列のシフト量を取得します。
        /// </summary>
        /// <param name="lineIndex">何行目か</param>
        /// <param name="columnIndex">何列目か</param>
        /// <returns>指定した行・列のシフト量</returns>
        public static int GetShift(int lineIndex, int columnIndex)
        {
            return GetShift(lineIndex) + (FieldContextConfig.OneLineBitCount - columnIndex);
        }

        /// <summary>
        /// 指定した行が何番目のフィールド単位に属しているかを取得します。
        /// </summary>
        /// <param name="lineIndex">何行目か</param>
        /// <returns>指定した行が何番目のフィールド単位に属しているか</returns>
        public static int GetFieldUnitIndex(int lineIndex)
        {
            return ((lineIndex - 1) / FieldContextConfig.FieldUnitLineCount);
        }

        /// <summary>
        /// 指定した行・列にスライムが存在するフィールド状態を取得します。
        /// </summary>
        /// <param name="lineIndex">何行目か</param>
        /// <param name="columnIndex">何列目か</param>
        /// <returns></returns>
        public static uint GetField(int lineIndex, int columnIndex)
        {
            return 1u << GetShift(lineIndex, columnIndex);
        }

        /// <summary>
        /// uintの検証を行います。
        /// </summary>
        /// <param name="expected">期待値</param>
        /// <param name="actual">実際値</param>
        public static void AssertAreEqualUint(uint expected, uint actual)
        {
            Assert.AreEqual(expected, actual,
                Environment.NewLine + "[expected] : {0}" + Environment.NewLine + " [actual] : {1}",
                ConvertUintToDebugFieldStr(expected), ConvertUintToDebugFieldStr(actual));
        }

        /// <summary>
        /// 更新後のフィールドの状態が期待通りであることを検証します。
        /// </summary>
        /// <param name="expectedFilePath">期待するフィールド状態を示すテキストファイルパス</param>
        /// <param name="actualFilePath">検証対象のフィールド状態を示すテキストファイルパス</param>
        /// <param name="updater">フィールド更新機能</param>
        public static void AssertEqualsFieldContext(string expectedFilePath, string actualFilePath, Action<FieldContext> updater)
        {
            var expected = TestHelper.Receiver.Receive(expectedFilePath);
            var actual = TestHelper.Receiver.Receive(actualFilePath);
            updater(actual);
            AssertEqualsFieldContext(expected, actual);
        }

        /// <summary>
        /// 更新後のフィールドの状態が期待通りであることを検証します。
        /// </summary>
        /// <param name="expectedFilePath">期待するフィールド状態を示すテキストファイルパス</param>
        /// <param name="actualFilePath">検証対象のフィールド状態を示すテキストファイルパス</param>
        /// <param name="updater">フィールド更新機能</param>
        public static void AssertEqualsFieldContext(string expectedFilePath, string actualFilePath, Action<FieldContext, Player.Index> updater)
        {
            var expected = TestHelper.Receiver.Receive(expectedFilePath);
            var actual = TestHelper.Receiver.Receive(actualFilePath);
            updater(actual, actual.OperationPlayer);
            AssertEqualsFieldContext(expected, actual);
        }

        /// <summary>
        /// 更新後のフィールドの状態が期待通りであることを検証します。
        /// </summary>
        /// <param name="expectedFilePath">期待するフィールド状態を示すテキストファイルパス</param>
        /// <param name="actualFilePath">検証対象のフィールド状態を示すテキストファイルパス</param>
        /// <param name="updater">フィールド更新機能</param>
        /// <param name="player">プレイヤ</param>
        public static void AssertEqualsFieldContext(string expectedFilePath, string actualFilePath, Action<FieldContext, Player.Index> updater, Player.Index player)
        {
            var expected = TestHelper.Receiver.Receive(expectedFilePath);
            var actual = TestHelper.Receiver.Receive(actualFilePath);
            updater(actual, player);
            AssertEqualsFieldContext(expected, actual);
        }

        /// <summary>
        /// フィールドの状態が同一であることを検証します。
        /// </summary>
        /// <param name="expected">期待値</param>
        /// <param name="actual">実際値</param>
        public static void AssertEqualsFieldContext(FieldContext expected, FieldContext actual)
        {
            Assert.IsTrue(expected.Equals(actual),
                Environment.NewLine + "[expected] :" + Environment.NewLine + "{0}" + Environment.NewLine + " [actual] :" + Environment.NewLine + "{1}",
                Sender.Send(expected), Sender.Send(actual));
        }

        /// <summary>
        /// uintの値をフィールドデバッグ出力用の文字列に変換します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string ConvertUintToDebugFieldStr(uint value)
        {
            var field = Convert.ToString(value, 2);
            field = field.PadLeft(FieldContextConfig.FieldUnitBitCount, '0');
            var fieldStates = field.ToCharArray().Reverse().ToArray();
            var result = new StringBuilder();

            for (var line = 1; line < 5; line++)
            {
                result.Append(Environment.NewLine);
                for (var i = ((line * 8) - 1); i > (((line - 1) * 8) - 1); i--)
                {
                    if ((i % 8) == 1)
                    {
                        result.Append("|");
                    }
                    result.Append(fieldStates[i]);
                }
            }

            return result.ToString();
        }
    }
}
