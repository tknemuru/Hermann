using System;
using System.IO;

namespace Hermann.Environments
{
    /// <summary>
    /// 環境設定情報を管理します。
    /// </summary>
    public static class EnvConfig
    {
        /// <summary>
        /// MacOSでのルートディレクトリ
        /// </summary>
        private static readonly string MacOsRootDir = @"/Users/Shared/Hermann";

        /// <summary>
        /// Windowsでのルートディレクトリ
        /// </summary>
        private static readonly string WinRootDir = @"C:/work/visualstudio/Hermann";

        /// <summary>
        /// 環境に応じたルートディレクトリを取得します。
        /// </summary>
        /// <returns>The root dir.</returns>
        public static string GetRootDir()
        {
            var env = GetPlatform();
            var root = string.Empty;
            switch(env)
            {
                case PlatformID.MacOSX:
                case PlatformID.Unix:
                    root = MacOsRootDir;
                    break;
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.WinCE:
                    root = WinRootDir;
                    break;
                default:
                    throw new PlatformNotSupportedException("サポート外のプラットフォームです");
            }
            return root;
        }

        /// <summary>
        /// プラットフォームを取得します。
        /// </summary>
        /// <returns>プラットフォーム</returns>
        public static PlatformID GetPlatform()
        {
            return Environment.OSVersion.Platform;
        }
    }
}
