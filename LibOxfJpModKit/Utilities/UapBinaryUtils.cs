namespace LibOxfJpModKit.Utilities
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// バイナリーファイル操作関連ユーティリティ
    /// </summary>
    public class UapBinaryUtils
    {
        /// <summary>
        /// Streamを読み込み、文字列を返す。
        /// 必要に応じpaddingを読み飛ばす。
        /// </summary>
        /// <param name="br">Stream</param>
        /// <returns>文字列</returns>
        public static string ReadString(BinaryReader br)
        {
            var result = string.Empty;

            var length = br.ReadInt32();
            if (length > 0)
            {
                var scriptNameArray = br.ReadBytes(length);
                result = Encoding.UTF8.GetString(scriptNameArray);
                //// Padding
                ReadPadding(br);
            }

            return result;
        }

        /// <summary>
        /// Streamにバイナリー形式で文字列を書き込む。
        /// 必要に応じpaddingを書き込む。
        /// </summary>
        /// <param name="bw">Stream</param>
        /// <param name="text">文字列</param>
        public static void WriteString(BinaryWriter bw, string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                bw.Write((int)0);
            }
            else
            {
                //// 文字列のバイト数を書き込む
                var length = Encoding.UTF8.GetByteCount(text);
                bw.Write(length);
                //// 文字列を書き込む
                byte[] arrayOfScriptName = Encoding.UTF8.GetBytes(text);
                bw.Write(arrayOfScriptName);
                //// padding処理
                WritePadding(bw);
            }
        }

        /// <summary>
        /// Streamを読み込み、Bool値を返す。
        /// 必要に応じpaddingを読み飛ばす。
        /// </summary>
        /// <param name="br">Stream</param>
        /// <returns>Bool値</returns>
        public static bool ReadBoolean(BinaryReader br)
        {
            var result = br.ReadBoolean();
            //// padding処理
            ReadPadding(br);

            return result;
        }

        /// <summary>
        /// Streamにバイナリー形式でbool値を書き込む。
        /// 必要に応じpaddingを書き込む。
        /// </summary>
        /// <param name="bw">Stream</param>
        /// <param name="value">bool値</param>
        public static void WriteBoolean(BinaryWriter bw, bool value)
        {
            bw.Write(value);
            //// padding処理
            WritePadding(bw);
        }

        /// <summary>
        /// StreamからPaddingのデータを読み飛ばす。
        /// </summary>
        /// <param name="br">Stream</param>
        private static void ReadPadding(BinaryReader br)
        {
            var padmod = (int)(br.BaseStream.Position % 4);
            if (padmod != 0)
            {
                //// paddingの読み飛ばし
                br.ReadBytes(4 - padmod);
            }
        }

        /// <summary>
        /// Streamにpaddingを書き込む。
        /// </summary>
        /// <param name="bw">Stream</param>
        private static void WritePadding(BinaryWriter bw)
        {
            //// ファイルのオフセット値からpaddingデータを書き込む。
            var amari = bw.BaseStream.Position % 4;
            if (amari > 0)
            {
                var paddingCount = 4 - amari;
                for (int i = 0; i < paddingCount; i++)
                {
                    byte dummy = 0;
                    bw.Write(dummy);
                }
            }
        }
    }
}
