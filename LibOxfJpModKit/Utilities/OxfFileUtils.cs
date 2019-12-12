namespace LibOxfJpModKit.Utilities
{
    using System.IO;

    /// <summary>
    /// ファイル関連のユーティリティ
    /// </summary>
    public class OxfFileUtils
    {
        /// <summary>
        /// 指定したパスにディレクトリが存在しない場合
        /// すべてのディレクトリとサブディレクトリを作成します
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>DirectoryInfo</returns>
        public static DirectoryInfo SafeCreateDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                return null;
            }

            return Directory.CreateDirectory(path);
        }
    }
}
