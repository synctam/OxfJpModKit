namespace LibOxfJpModKit.DialogLine
{
    using System;
    using System.IO;
    using System.Text;
    using LibOxfJpModKit.Utilities;

    /// <summary>
    /// UnityExでExportしたMonoBehaviour(Dialog)ファイルの入出力
    /// 抽出方法: UnityEX.exe export resources.assets -mb_new -t -2
    /// 拡張子: ".DialogPackage"
    /// </summary>
    public class UapDialogDao
    {
        /// <summary>
        /// 指定したフォルダー内のMB(Dialog)を読み込む。
        /// </summary>
        /// <param name="dialogInfo">Dialog情報</param>
        /// <param name="folderPath">フォルダーのパス</param>
        /// <param name="pattern">ファイル名のパターン</param>
        public static void LoadFromFolder(
            UapDialogInfo dialogInfo, string folderPath, string pattern)
        {
            string[] files = Directory.GetFiles(
                folderPath, pattern, SearchOption.AllDirectories);
            foreach (var locFile in files)
            {
                LoadFromFile(dialogInfo, locFile);
            }
        }

        /// <summary>
        /// 指定したフォルダーにMB(Dialog)を保存する。
        /// </summary>
        /// <param name="folderPath">フォルダーのパス</param>
        /// <param name="langDialogInfoJP">Dialog情報</param>
        /// <param name="useReplace">上書きの有無</param>
        public static void SaveToFolder(string folderPath, UapDialogInfo langDialogInfoJP, bool useReplace)
        {
            foreach (var dialogFile in langDialogInfoJP.Items.Values)
            {
                var path = Path.Combine(folderPath, dialogFile.FileID + ".DialogPackage");
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
                    using (var sw = new StreamWriter(path, false, Encoding.ASCII))
                    {
                        var bw = new BinaryWriter(sw.BaseStream);
                        WriteEntries(dialogFile, bw);
                    }
                }
            }
        }

        /// <summary>
        /// 指定したパスのMB(Dialog)を読み込み、Dialog情報に追加する。
        /// </summary>
        /// <param name="dialogInfo">Dialog情報</param>
        /// <param name="path">MB(Dialog)のパス</param>
        private static void LoadFromFile(UapDialogInfo dialogInfo, string path)
        {
            using (var sr = new StreamReader(path))
            {
                using (var br = new BinaryReader(sr.BaseStream, Encoding.UTF8))
                {
                    var fileID = Path.GetFileNameWithoutExtension(path);
                    var dialogFile = new UapDialogFile(fileID);
                    ReadEntries(dialogFile, br);
                    dialogInfo.AddFile(dialogFile);
                }
            }
        }

        /// <summary>
        /// StreamからDialogエントリー読み込み、Dialogファイルに追加する。
        /// </summary>
        /// <param name="dialogFile">Dialogファイル</param>
        /// <param name="br">Stream</param>
        private static void ReadEntries(
            UapDialogFile dialogFile, BinaryReader br)
        {
            var entries = br.ReadInt32();
            for (int i = 0; i < entries; i++)
            {
                var entry = ReadEntry(br);
                dialogFile.AddEntry(entry);
            }
        }

        /// <summary>
        /// Streamからデータを読み込み、Dialogエントリー返す。
        /// </summary>
        /// <param name="br">Stream</param>
        /// <returns>Dialogエントリー</returns>
        private static UapDialogEntry ReadEntry(BinaryReader br)
        {
            var lineID = UapBinaryUtils.ReadString(br);
            var character = UapBinaryUtils.ReadString(br);
            var result = new UapDialogEntry(lineID, character);

            var size = br.ReadInt32();
            for (int i = 0; i < size; i++)
            {
                var langCode = br.ReadInt32();
                var text = UapBinaryUtils.ReadString(br);
                var clip = UapBinaryUtils.ReadString(br);
                float vol = br.ReadSingle();
                result.AddEntry(langCode, text, clip, vol);
            }

            return result;
        }

        /// <summary>
        /// DialogファイルをStreamに書き出す。
        /// </summary>
        /// <param name="dialogFile">Dialogファイル</param>
        /// <param name="bw">Stream</param>
        private static void WriteEntries(UapDialogFile dialogFile, BinaryWriter bw)
        {
            bw.Write(dialogFile.Items.Count);
            foreach (var entry in dialogFile.Items.Values)
            {
                WriteEntry(entry, bw);
            }
        }

        /// <summary>
        /// DialogエントリーをStreamに書き出す。
        /// </summary>
        /// <param name="entry">Dialogエントリー</param>
        /// <param name="bw">Stream</param>
        private static void WriteEntry(UapDialogEntry entry, BinaryWriter bw)
        {
            UapBinaryUtils.WriteString(bw, entry.LineID);
            UapBinaryUtils.WriteString(bw, entry.Character);

            bw.Write(entry.LanguageEntries.Count);
            foreach (var lang in entry.LanguageEntries.Values)
            {
                bw.Write(lang.LangCode);
                UapBinaryUtils.WriteString(bw, lang.Text);
                UapBinaryUtils.WriteString(bw, lang.Clip);
                bw.Write(lang.Vol);
            }
        }
    }
}
