namespace LibOxfJpModKit.TransSheet.Localization
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 翻訳シート情報(Localization)
    /// </summary>
    public class UapTransSheetLocalizationInfo
    {
        /// <summary>
        /// 翻訳シートファイルの辞書。
        /// キーは FileID。
        /// </summary>
        public Dictionary<string, UapTransSheetLocalizationFile> Items { get; } =
            new Dictionary<string, UapTransSheetLocalizationFile>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// FileIDに該当する翻訳シートファイル(Localization)を返す。
        /// </summary>
        /// <param name="fileID">FileID</param>
        /// <returns>翻訳シートファイル(Localization)</returns>
        public UapTransSheetLocalizationFile GetFile(string fileID)
        {
            if (this.Items.ContainsKey(fileID))
            {
                return this.Items[fileID];
            }
            else
            {
                //// 存在しない場合は新規に作成する。
                var localizationFile = new UapTransSheetLocalizationFile(fileID);
                this.Items.Add(localizationFile.FileID, localizationFile);

                return localizationFile;
            }
        }

        /// <summary>
        /// 翻訳シートエントリー(Localization)を追加する。
        /// </summary>
        /// <param name="sheetEntry">翻訳シートエントリー(Localization)</param>
        public void AddEntry(UapTransSheetLocalizationEntry sheetEntry)
        {
            if (this.Items.ContainsKey(sheetEntry.FileID))
            {
                var sheetFile = this.Items[sheetEntry.FileID];
                sheetFile.AddEntry(sheetEntry);
            }
            else
            {
                //// 翻訳シートファイルが存在しない時は新たに作成する。
                var sheetFile = new UapTransSheetLocalizationFile(sheetEntry.FileID);
                sheetFile.AddEntry(sheetEntry);

                this.Items.Add(sheetFile.FileID, sheetFile);
            }
        }

        /// <summary>
        /// 指定した FileID, ID の翻訳シートエントリー(Localization)を返す。
        /// </summary>
        /// <param name="fileID">FileID</param>
        /// <param name="id">id</param>
        /// <returns>翻訳シートエントリー(Localization)</returns>
        public UapTransSheetLocalizationEntry GetEntry(string fileID, string id)
        {
            if (this.Items.ContainsKey(fileID))
            {
                var locFile = this.Items[fileID];
                if (locFile.Items.ContainsKey(id))
                {
                    var locEntry = locFile.Items[id];
                    return locEntry;
                }
            }

            //// 存在しない場合はnullオブジェクトを返す。
            var nullObject = new UapTransSheetLocalizationEntry();
            return nullObject;
        }
    }
}
