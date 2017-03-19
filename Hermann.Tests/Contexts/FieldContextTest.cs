using Hermann.Collections;
using Hermann.Contexts;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hermann.Tests.Contexts
{
    /// <summary>
    /// FieldContextクラスのテスト機能を提供します。
    /// </summary>
    [TestClass]
    public class FieldContextTest
    {
        /// <summary>
        /// 001:Equalsテスト
        /// </summary>
        [TestMethod]
        public void Equalsテスト()
        {
            // 等しい
            var x = CreateEqualsTestDefaultFieldContext();
            var y = CreateEqualsTestDefaultFieldContext();
            Assert.IsTrue(x.Equals(y));

            // コマンドが等しくない
            y = CreateEqualsTestDefaultFieldContext();
            y.Command = 5u;
            Assert.IsFalse(x.Equals(y));

            // 移動可能なスライムの配置状態が等しくない
            y = CreateEqualsTestDefaultFieldContext();
            y.MovableInfos[1] = new MovableInfo(Slime.Purple, 2, 5);
            Assert.IsFalse(x.Equals(y));

            // スライムごとの配置状態が等しくない
            y = CreateEqualsTestDefaultFieldContext();
            y.SlimeFields[Slime.Red] = new uint[] { 2u, 4u, 6u };
            Assert.IsFalse(x.Equals(y));
        }

        /// <summary>
        /// Equalsテスト向けのデフォルトFieldContextインスタンスを作成します。
        /// </summary>
        /// <returns>Equalsテスト向けのデフォルトFieldContextインスタンス</returns>
        private static FieldContext CreateEqualsTestDefaultFieldContext()
        {
            var context = new FieldContext();
            
            context.Command = 3u;
            
            context.MovableInfos = new MovableInfo[2];
            var first = new MovableInfo(Slime.Red, 1, 3);
            var second = new MovableInfo(Slime.Blue, 2, 5);
            context.MovableInfos[0] = first;
            context.MovableInfos[1] = second;

            var fields = new Dictionary<Slime, uint[]>();
            fields.Add(Slime.Red, new uint[] {2u, 3u, 6u});
            fields.Add(Slime.Blue, new uint[] { 3u, 4u, 7u });
            context.SlimeFields = fields;

            return context;
        }
    }
}
