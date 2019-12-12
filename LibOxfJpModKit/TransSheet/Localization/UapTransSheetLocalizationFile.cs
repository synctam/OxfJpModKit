namespace LibOxfJpModKit.TransSheet.Localization
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 翻訳シートファイル(Localization)
    /// </summary>
    public class UapTransSheetLocalizationFile
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileID">FileID</param>
        public UapTransSheetLocalizationFile(string fileID)
        {
            this.FileID = fileID;
        }

        /// <summary>
        /// 翻訳シートエントリー(Localization)の辞書。
        /// キーは、ID。
        /// </summary>
        public Dictionary<string, UapTransSheetLocalizationEntry> Items { get; } =
            new Dictionary<string, UapTransSheetLocalizationEntry>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// FileID
        /// </summary>
        public string FileID { get; } = string.Empty;

        /// <summary>
        /// EntryIDから翻訳シートエントリー(Localization)を返す。
        /// </summary>
        /// <param name="entryID">EntryID</param>
        /// <returns>翻訳シートエントリー(Localization)</returns>
        public UapTransSheetLocalizationEntry GetEntry(string entryID)
        {
            if (this.Items.ContainsKey(entryID))
            {
                return this.Items[entryID];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 翻訳シートエントリー(Localization)を追加する。
        /// </summary>
        /// <param name="localizationEntry">翻訳シートエントリー(Dialog)</param>
        public void AddEntry(UapTransSheetLocalizationEntry localizationEntry)
        {
            if (this.Items.ContainsKey(localizationEntry.ID))
            {
                throw new Exception($"Duplicate ID({localizationEntry.ID}). FileID({localizationEntry.FileID})");
            }
            else
            {
                this.Items.Add(localizationEntry.ID, localizationEntry);
            }
        }
    }
}
