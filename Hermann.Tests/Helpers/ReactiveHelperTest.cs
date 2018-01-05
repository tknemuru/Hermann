using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reactive.Bindings;
using Reactive.Bindings.Notifiers;
using Hermann.Helpers;

namespace Hermann.Tests.Helpers
{
    /// <summary>
    /// Reactiveのテスト実行機能を提供します。
    /// </summary>
    [TestClass]
    public class ReactiveHelperTest
    {
        /// <summary>
        /// 001:GetValuesテスト
        /// </summary>
        [TestMethod]
        public void GetValuesテスト()
        {
            var r = new ReactiveProperty<bool>[2];
            r[0] = new ReactiveProperty<bool>(false);
            r[1] = new ReactiveProperty<bool>(true);
            var vals = ReactiveHelper.GetValues(r);
            CollectionAssert.AreEqual(new[] { false, true }, vals);
        }
    }
}
