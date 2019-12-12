namespace LibOxfJpModKit.TransSheet.Localization
{
    using System.IO;
    using System.Text;
    using CsvHelper;
    using LibOxfJpModKit.Localization;

    /// <summary>
    /// 翻訳シート(Localization)入出力
    /// 正規のFileIDの形式は "LocalizationReference_xxxxxx_SELFNAME"
    /// 簡略化された FileID は xxxxxx とする。
    /// </summary>
    public class UapTransSheetLocalizationDao
    {
        /// <summary>
        /// ローカライズ情報をCSV形式で保存する。
        /// </summary>
        /// <param name="uapLocalizationInfo">ローカライズ情報</param>
        /// <param name="path">CSV形式のファイルのパス</param>
        /// <param name="langCode">言語番号</param>
        public static void SaveToCsv(
            UapLocalizationInfo uapLocalizationInfo, string path, int langCode)
        {
            using (var writer = new CsvWriter(
                new StreamWriter(path, false, Encoding.UTF8)))
            {
                writer.Configuration.RegisterClassMap<CsvMapperLocalization>();
                writer.WriteHeader<UapTransSheetLocalizationEntry>();
                writer.NextRecord();

                foreach (var localizationFile in uapLocalizationInfo.Items.Values)
                {
                    var seq = 1;
                    foreach (var entry in localizationFile.Items.Values)
                    {
                        var data = new UapTransSheetLocalizationEntry();
                        //// FileIDを簡略形式に変換する。
                        data.FileID = ConvertFileIDToSheetFileID(localizationFile.FileID);
                        data.Name = entry.Name;
                        data.ID = $"#{entry.ID}";
                        //// 英語のエントリを出力する。
                        data.English = entry.GetEntry(langCode);
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
        /// CSVファイルから翻訳シート情報(Localization)を作成する。
        /// </summary>
        /// <param name="sheetInfo">翻訳シート情報(Localization)</param>
        /// <param name="path">CSVファイルのパス</param>
        /// <param name="enc">CSVファイルの文字コード</param>
        public static void LoadFromCsv(
            UapTransSheetLocalizationInfo sheetInfo, string path, Encoding enc = null)
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
                csv.Configuration.RegisterClassMap<CsvMapperLocalization>();

                //// データを読み出し
                var records = csv.GetRecords<UapTransSheetLocalizationEntry>();
                foreach (var record in records)
                {
                    var sheetEntry = new UapTransSheetLocalizationEntry();
                    sheetEntry.English = record.English;
                    //// 簡略形式のFileIDを正規のFileIDに変換する。
                    sheetEntry.FileID = ConverSheetFileIdToFileID(record.FileID);
                    sheetEntry.ID = record.ID.Replace("#", string.Empty);
                    sheetEntry.Japanese = record.Japanese;
                    sheetEntry.MTrans = record.MTrans;
                    sheetEntry.Name = record.Name;
                    sheetEntry.Sequence = record.Sequence;

                    sheetInfo.AddEntry(sheetEntry);
                }
            }
        }

        /// <summary>
        /// 簡略形式のFileIDを返す。
        /// </summary>
        /// <param name="fileID">正規のFileID</param>
        /// <returns>簡略形式のFileID</returns>
        private static string ConvertFileIDToSheetFileID(string fileID)
        {
            var buff = new StringBuilder(fileID);
            buff.Replace("LocalizationReference_", string.Empty);
            buff.Replace("_SELFNAME", string.Empty);
            return buff.ToString();
        }

        /// <summary>
        /// 正規の形式のFileIDを返す。
        /// </summary>
        /// <param name="shortFileID">簡略形式のFaieID</param>
        /// <returns>正規の形式のFileID</returns>
        private static string ConverSheetFileIdToFileID(string shortFileID)
        {
            var result = $"LocalizationReference_{shortFileID}_SELFNAME";
            return result;
        }

        /// <summary>
        /// 格納ルール ：マッピングルール(一行目を列名とした場合は列名で定義することができる。)
        /// </summary>
        public class CsvMapperLocalization :
            CsvHelper.Configuration.ClassMap<UapTransSheetLocalizationEntry>
        {
            public CsvMapperLocalization()
            {
                // 出力時の列の順番は指定した順となる。
                this.Map(x => x.FileID).Name("[[FileID]]");
                this.Map(x => x.Name).Name("[[Name]]");
                this.Map(x => x.ID).Name("[[ID]]");
                this.Map(x => x.English).Name("[[English]]");
                this.Map(x => x.Japanese).Name("[[Japanese]]");
                this.Map(x => x.MTrans).Name("[[MTrans]]");
                this.Map(x => x.Sequence).Name("[[Sequence]]");
            }
        }
    }
}
