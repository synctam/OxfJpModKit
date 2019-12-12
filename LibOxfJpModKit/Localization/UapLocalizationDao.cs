namespace LibOxfJpModKit.Localization
{
    using System;
    using System.IO;
    using System.Text;
    using LibOxfJpModKit.Utilities;

    /// <summary>
    /// UnityExでExportしたMonoBehaviour(Localization)ファイルの入出力
    /// 抽出方法: UnityEX.exe export resources.assets -mb_new -t -9
    /// 拡張子: ".LocalizationReference"
    /// </summary>
    public class UapLocalizationDao
    {
        /// <summary>
        /// 指定したフォルダー内のMB(Localization)を読み込む
        /// </summary>
        /// <param name="localizationInfo">Localization情報</param>
        /// <param name="folderPath">フォルダーのパス</param>
        /// <param name="pattern">ファイル名のパターン</param>
        public static void LoadFromFolder(
            UapLocalizationInfo localizationInfo, string folderPath, string pattern)
        {
            string[] files = Directory.GetFiles(folderPath, pattern, SearchOption.AllDirectories);
            foreach (var locFile in files)
            {
                LoadFromFile(localizationInfo, locFile);
            }
        }

        /// <summary>
        /// 指定したフォルダーにMB(Localization)を保存する。
        /// </summary>
        /// <param name="folderPath">フォルダーのパス</param>
        /// <param name="localizeInfoJP">ローカライズ情報</param>
        /// <param name="useReplace">上書きの有無</param>
        public static void SaveToFolder(string folderPath, UapLocalizationInfo localizeInfoJP, bool useReplace)
        {
            foreach (var localizeFile in localizeInfoJP.Items.Values)
            {
                var path = Path.Combine(folderPath, localizeFile.FileID + ".LocalizationReference");
                //// exists | useReplace |
                ////    y   |     y      | run
                ////    y   |     n      | cancel
                ////    n   |     y      | run
                ////    n   |     n      | run
                if (File.Exists(path) && !useReplace)
                {
                    //// 上書きしない
                    var fileName = Path.GetFileName(path);
                    Console.WriteLine($"ファイルが既に存在します。File({fileName}){Environment.NewLine}上書きする場合は -r オプションを指定してください。");
                }
                else
                {
                    OxfFileUtils.SafeCreateDirectory(folderPath);
                    using (StreamWriter sw = new StreamWriter(path, false, Encoding.ASCII))
                    {
                        BinaryWriter bw = new BinaryWriter(sw.BaseStream);
                        WriteEntries(localizeFile, bw);
                    }
                }
            }
        }

        /// <summary>
        /// 指定したパスのMB(Localization)を読み込み、ローカライズ情報に追加する。
        /// </summary>
        /// <param name="localizationInfo">ローカライズ情報</param>
        /// <param name="path">ファイルのパス</param>
        private static void LoadFromFile(
            UapLocalizationInfo localizationInfo, string path)
        {
            using (var sr = new StreamReader(path))
            {
                using (var br = new BinaryReader(
                    sr.BaseStream, Encoding.UTF8))
                {
                    var fileID = Path.GetFileNameWithoutExtension(path);
                    var localizationFile = new UapLocalizationFile(fileID);
                    ReadEntries(localizationFile, br);
                    localizationInfo.AddFile(localizationFile);
                }
            }
        }

        /// <summary>
        /// Streamからローカライズエントリー読み込み、ローカライズファイルに追加する。
        /// </summary>
        /// <param name="localizationFile">ローカライズファイル</param>
        /// <param name="sr">Stream</param>
        private static void ReadEntries(
            UapLocalizationFile localizationFile, BinaryReader sr)
        {
            var entries = sr.ReadInt32();
            for (int i = 0; i < entries; i++)
            {
                var entry = ReadEntry(sr, localizationFile.FileID);
                localizationFile.AddEntry(entry);
            }
        }

        /// <summary>
        /// Streamからデータを読み込みローカライズエントリー返す。
        /// </summary>
        /// <param name="br">Stream</param>
        /// <param name="fileID">fileID</param>
        /// <returns>ローカライズエントリー</returns>
        private static UapLocalizationEntry ReadEntry(
            BinaryReader br, string fileID)
        {
            var name = UapBinaryUtils.ReadString(br);
            var id = UapBinaryUtils.ReadString(br);
            var result = new UapLocalizationEntry(name, id);

            var size = br.ReadInt32();
            for (int i = 0; i < size; i++)
            {
                var langCode = br.ReadInt32();
                var text = UapBinaryUtils.ReadString(br);

                if (!result.AddEntry(langCode, text))
                {
                    //// GOG版(v2.7.1)のデータバグ対策。
                    Console.WriteLine($"Warning: Duplicate language FileID({fileID}) Name({name}) ID({id}) LangNo({langCode})");
                }
            }

            return result;
        }

        /// <summary>
        /// ローカライズファイルをStreamに書き出す。
        /// </summary>
        /// <param name="localizeFile">ローカライズファイル</param>
        /// <param name="bw">Stream</param>
        private static void WriteEntries(UapLocalizationFile localizeFile, BinaryWriter bw)
        {
            bw.Write(localizeFile.Items.Count);
            foreach (var entry in localizeFile.Items.Values)
            {
                WriteEntry(entry, bw);
            }
        }

        /// <summary>
        /// ローカライズエントリーをStreamに書き出す。
        /// </summary>
        /// <param name="entry">ローカライズエントリー</param>
        /// <param name="bw">Stream</param>
        private static void WriteEntry(UapLocalizationEntry entry, BinaryWriter bw)
        {
            UapBinaryUtils.WriteString(bw, entry.Name);
            UapBinaryUtils.WriteString(bw, entry.ID);
            bw.Write(entry.LanguageEntries.Count);
            foreach (var langEntryPair in entry.LanguageEntries)
            {
                bw.Write(langEntryPair.Key);
                UapBinaryUtils.WriteString(bw, langEntryPair.Value);
            }
        }
    }
}
