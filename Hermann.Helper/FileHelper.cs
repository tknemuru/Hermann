using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hermann.Helpers
{
    /// <summary>
    /// ファイル操作に関する機能を提供します。
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// <para>ファイルから文字列のリストを取得します。</para>
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>文字列のリスト</returns>
        public static IEnumerable<string> ReadTextLines(string filePath, Encoding encoding = null)
        {
            if (encoding == null) { encoding = Encoding.GetEncoding("Shift_JIS"); }

            string line;
            using (StreamReader sr = new StreamReader(filePath, encoding))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}
