using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hermann.Helpers
{
    /// <summary>
    /// ファイル操作に関する機能を提供します。
    /// </summary>
    public static class FileHelper
    {
        /// <summary>
        /// ファイルパスの初期値
        /// </summary>
        private static readonly string DefaultFilePath;

        /// <summary>
        /// エンコードの初期値
        /// UTF-8, Shift_JIS
        /// </summary>
        private const string DefaultEncoding = "UTF-8";

        /// <summary>
        /// コンストラクタ
        /// </summary>
        static FileHelper()
        {
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            DefaultFilePath = string.Format(@"./log/{0}.txt", DateTime.Now.ToString("yyyyMMddhhmmss"));
        }

        /// <summary>
        /// <para>ファイルから文字列のリストを取得します。</para>
        /// </summary>
        /// <param name="filePath">ファイルパス</param>
        /// <returns>文字列のリスト</returns>
        public static IEnumerable<string> ReadTextLines(string filePath, Encoding encoding = null)
        {
            if (encoding == null) { encoding = Encoding.GetEncoding(DefaultEncoding); }

            string line;
            using (StreamReader sr = new StreamReader(filePath, encoding))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        /// <summary>
        /// 文字列に改行コードを付与して出力します。
        /// </summary>
        /// <param name="line"></param>
        public static void WriteLine(string line)
        {
            WriteLine(line, DefaultFilePath);
        }

        /// <summary>
        /// <para>文字列に改行コードを付与して出力します。</para>
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static void WriteLine(string line, string filePath, bool append = true)
        {
            CreateDirectory(GetFileDirectory(filePath));

            using (System.IO.StreamWriter sr = new System.IO.StreamWriter(filePath, append, System.Text.Encoding.GetEncoding(DefaultEncoding)))
            {
                sr.WriteLine(line);
            }
        }

        /// <summary>
        /// <para>文字列をファイルに出力する</para>
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static void Write(string str)
        {
            Write(str, DefaultFilePath);
        }

        /// <summary>
        /// <para>文字列をファイルに出力する</para>
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static void Write(string str, string filePath, bool append = true)
        {
            CreateDirectory(GetFileDirectory(filePath));

            using (System.IO.StreamWriter sr = new System.IO.StreamWriter(filePath, append, System.Text.Encoding.GetEncoding(DefaultEncoding)))
            {
                sr.Write(str);
            }
        }

        /// <summary>
        /// ディレクトリを作成する
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectory(string path)
        {
            if (File.Exists(path)) { return; }
            Directory.CreateDirectory(path);
        }

        /// <summary>
        /// ファイル名を含めたフルパスからファイル名を除いたパスを返す
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string GetFileDirectory(string fullPath, char delimiter = '/')
        {
            List<string> directorys = fullPath.Replace('\\', delimiter).Split(delimiter).ToList();
            if (directorys.Count == 1)
            {
                return fullPath;
            }

            string retPath = "";
            for (int i = 0; i < (directorys.Count - 1); i++)
            {
                retPath += directorys[i] + delimiter;
            }

            return retPath.Substring(0, (retPath.Length - 1));
        }

        /// <summary>
        /// ファイル名を含めたフルパスからファイル名のみを返す
        /// </summary>
        /// <param name="fullPath">フルパス</param>
        /// <returns>ファイル名</returns>
        public static string GetFileName(string fullPath)
        {
            return Path.GetFileName(fullPath);
        }
    }
}
