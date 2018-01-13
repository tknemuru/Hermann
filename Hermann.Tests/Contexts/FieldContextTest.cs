using Hermann.Collections;
using Hermann.Contexts;
using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reactive.Linq;
using Reactive.Bindings;
using Reactive.Bindings.Notifiers;

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

            // プレイヤが等しくない
            y = CreateEqualsTestDefaultFieldContext();
            y.OperationPlayer = Player.Index.First;
            Assert.IsFalse(x.Equals(y));

            // 方向が等しくない
            y = CreateEqualsTestDefaultFieldContext();
            y.OperationDirection = Direction.Left;
            Assert.IsFalse(x.Equals(y));

            // 1Pの移動可能なスライムの配置状態が等しくない
            y = CreateEqualsTestDefaultFieldContext();
            y.MovableSlimes[0][1] = new MovableSlime(Slime.Purple, 2, 5);
            Assert.IsFalse(x.Equals(y));

            // 2Pの移動可能なスライムの配置状態が等しくない
            y = CreateEqualsTestDefaultFieldContext();
            y.MovableSlimes[1][1] = new MovableSlime(Slime.Purple, 2, 5);
            Assert.IsFalse(x.Equals(y));

            // 1Pのスライムごとの配置状態が等しくない
            y = CreateEqualsTestDefaultFieldContext();
            y.SlimeFields[0][Slime.Red] = new uint[] { 2u, 4u, 6u };
            Assert.IsFalse(x.Equals(y));

            // 2Pのスライムごとの配置状態が等しくない
            y = CreateEqualsTestDefaultFieldContext();
            y.SlimeFields[1][Slime.Red] = new uint[] { 2u, 4u, 6u };
            Assert.IsFalse(x.Equals(y));
        }

        /// <summary>
        /// Equalsテスト向けのデフォルトFieldContextインスタンスを作成します。
        /// </summary>
        /// <returns>Equalsテスト向けのデフォルトFieldContextインスタンス</returns>
        private static FieldContext CreateEqualsTestDefaultFieldContext()
        {
            var context = new FieldContext();

            context.OperationPlayer = Player.Index.Second;
            context.OperationDirection = Direction.Right;

            context.MovableSlimes[0] = new MovableSlime[2];
            context.MovableSlimes[0][0] = new MovableSlime(Slime.Red, 1, 3);
            context.MovableSlimes[0][1] = new MovableSlime(Slime.Blue, 2, 5);
            context.MovableSlimes[1] = new MovableSlime[2];
            context.MovableSlimes[1][0] = new MovableSlime(Slime.Green, 0, 4);
            context.MovableSlimes[1][1] = new MovableSlime(Slime.Purple, 1, 6);

            var fieldsFirst = new Dictionary<Slime, uint[]>();
            fieldsFirst.Add(Slime.Red, new uint[] {2u, 3u, 6u});
            fieldsFirst.Add(Slime.Blue, new uint[] { 3u, 4u, 7u });
            context.SlimeFields = new Dictionary<Slime, uint[]>[Player.Length];
            context.SlimeFields[0] = fieldsFirst;

            var fieldsSecond = new Dictionary<Slime, uint[]>();
            fieldsSecond.Add(Slime.Red, new uint[] { 3u, 4u, 7u });
            fieldsSecond.Add(Slime.Blue, new uint[] { 2u, 3u, 6u });
            context.SlimeFields[1] = fieldsSecond;

            return context;
        }
    }
}
