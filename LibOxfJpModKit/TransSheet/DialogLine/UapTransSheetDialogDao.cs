namespace LibOxfJpModKit.TransSheet.DialogLine
{
    using System.IO;
    using System.Text;
    using CsvHelper;
    using LibOxfJpModKit.DialogLine;

    /// <summary>
    /// 翻訳シート(Dialog)入出力
    /// 正規のFileIDの形式は "xxxx_SELFNAME"
    /// 簡略化された FileIDは xxxx とする。
    /// </summary>
    public class UapTransSheetDialogDao
    {
        /// <summary>
        /// 言語情報から翻訳シートをCSV形式で保存する。
        /// </summary>
        /// <param name="uapDialogInfo">言語情報</param>
        /// <param name="path">CSVファイルのパス</param>
        /// <param name="langCode">言語コード</param>
        public static void SaveToCsv(
            UapDialogInfo uapDialogInfo, string path, int langCode)
        {
            //// ToDo: 言語コードを enum 化する。
            using (var writer = new CsvWriter(
                new StreamWriter(path, false, Encoding.UTF8)))
            {
                writer.Configuration.RegisterClassMap<CsvMapperDialog>();
                writer.WriteHeader<UapTransSheetDialogEntry>();
                writer.NextRecord();

                foreach (var dialogFile in uapDialogInfo.Items.Values)
                {
                    var seq = 1;
                    foreach (var entry in dialogFile.Items.Values)
                    {
                        var data = new UapTransSheetDialogEntry();
                        //// FileIDを簡略形式に変換する。
                        data.FileID = ConvertFileIDToSheetFileID(dialogFile.FileID);
                        data.LineID = entry.LineID;
                        data.Character = entry.Character;
                        data.English = entry.GetEntry(langCode).Text;
                        data.Japanese = string.Empty;
                        data.MTrans = string.Empty;
                        data.Sequence = seq;
                        seq++;
                        writer.WriteRecord(data);

                        writer.NextRecord();
                    }
                }
            }
        }

        /// <summary>
        /// CSVファイルから翻訳シート情報を作成する。
        /// </summary>
        /// <param name="sheetInfo">翻訳シート情報</param>
        /// <param name="path">CSVファイルのパス</param>
        /// <param name="enc">CSVファイルの文字コード</param>
        public static void LoadFromCsv(
            UapTransSheetDialogInfo sheetInfo, string path, Encoding enc = null)
        {
            if (enc == null)
            {
                enc = Encoding.UTF8;
            }

            using (var reader = new StreamReader(path, enc))
            using (var csv = new CsvReader(reader))
            {
                //// 区切り文字
                csv.Configuration.Delimiter = ",";
                csv.Configuration.HasHeaderRecord = true;
                csv.Configuration.RegisterClassMap<CsvMapperDialog>();

                //// データを読み出し
                var records = csv.GetRecords<UapTransSheetDialogEntry>();
                foreach (var record in records)
                {
                    var sheetEntry = new UapTransSheetDialogEntry();
                    sheetEntry.Character = record.Character;
                    sheetEntry.English = record.English;
                    //// 簡略形式のFileIDを正規のFileIDに変換する。
                    sheetEntry.FileID = ConverSheetFileIdToFileID(record.FileID);
                    sheetEntry.Japanese = record.Japanese;
                    sheetEntry.LineID = record.LineID;
                    sheetEntry.MTrans = record.MTrans;
                    sheetEntry.Sequence = record.Sequence;

                    sheetInfo.AddEntry(sheetEntry);
                }
            }
        }

        /// <summary>
        /// 正規のFileIDを簡略化されたFileIDに変換し返す。
        /// </summary>
        /// <param name="fileID">正規のFileID</param>
        /// <returns>簡略化されたFileID</returns>
        private static string ConvertFileIDToSheetFileID(string fileID)
        {
            var sheetFileID = fileID.Replace("_SELFNAME", string.Empty);
            return sheetFileID;
        }

        /// <summary>
        /// 簡略化されたFileIDを正規のFileIDに変換し返す。
        /// </summary>
        /// <param name="sheetFileID">簡略化されたFileID</param>
        /// <returns>正規のFileID</returns>
        private static string ConverSheetFileIdToFileID(string sheetFileID)
        {
            var fileID = sheetFileID + "_SELFNAME";
            return fileID;
        }

        /// <summary>
        /// 格納ルール ：マッピングルール(一行目を列名とした場合は列名で定義することができる。)
        /// </summary>
        public class CsvMapperDialog :
            CsvHelper.Configuration.ClassMap<UapTransSheetDialogEntry>
        {
            public CsvMapperDialog()
            {
                // 出力時の列の順番は指定した順となる。
                this.Map(x => x.FileID).Name("[[FileID]]");
                this.Map(x => x.LineID).Name("[[LineID]]");
                this.Map(x => x.Character).Name("[[Character]]");
                this.Map(x => x.English).Name("[[English]]");
                this.Map(x => x.Japanese).Name("[[Japanese]]");
                this.Map(x => x.MTrans).Name("[[MTrans]]");
                this.Map(x => x.Sequence).Name("[[Sequence]]");
            }
        }
    }
}
