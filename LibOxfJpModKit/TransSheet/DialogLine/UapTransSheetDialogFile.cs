namespace LibOxfJpModKit.TransSheet.DialogLine
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 翻訳シートファイル(Dialog)
    /// </summary>
    public class UapTransSheetDialogFile
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileID">FileID</param>
        public UapTransSheetDialogFile(string fileID)
        {
            this.FileID = fileID;
        }

        /// <summary>
        /// FileID
        /// </summary>
        public string FileID { get; } = string.Empty;

        /// <summary>
        /// 翻訳シートエントリー(Dialog)の辞書。
        /// キーは LineID(大文字/小文字を区別する)。
        /// </summary>
        public Dictionary<string, UapTransSheetDialogEntry> Items { get; } =
             new Dictionary<string, UapTransSheetDialogEntry>();

        /// <summary>
        /// EntryIDから翻訳シートエントリー(Dialog)を返す。
        /// </summary>
        /// <param name="entryID">EntryID</param>
        /// <returns>翻訳シートエントリー(Dialog)</returns>
        public UapTransSheetDialogEntry GetEntry(string entryID)
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
        /// 翻訳シートエントリー(Dialog)を追加する。
        /// </summary>
        /// <param name="dialogEntry">翻訳シートエントリー(Dialog)</param>
        public void AddEntry(UapTransSheetDialogEntry dialogEntry)
        {
            if (this.Items.ContainsKey(dialogEntry.LineID))
            {
                throw new Exception($"Duplicate LineID({dialogEntry.LineID}). FileID({dialogEntry.FileID})");
            }
            else
            {
                this.Items.Add(dialogEntry.LineID, dialogEntry);
            }
        }
    }
}
