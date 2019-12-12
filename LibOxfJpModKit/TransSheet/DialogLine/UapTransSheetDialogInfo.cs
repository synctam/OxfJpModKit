namespace LibOxfJpModKit.TransSheet.DialogLine
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// 翻訳シート情報(Dialog)
    /// </summary>
    public class UapTransSheetDialogInfo
    {
        /// <summary>
        /// 翻訳シートファイル(Dialog)の辞書。
        /// キーは FileID。
        /// </summary>
        public Dictionary<string, UapTransSheetDialogFile> Items { get; } =
            new Dictionary<string, UapTransSheetDialogFile>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// FileIDに該当する翻訳シートファイル(Dialog)を返す。
        /// </summary>
        /// <param name="fileID">FileID</param>
        /// <returns>翻訳シートファイル(Dialog)</returns>
        public UapTransSheetDialogFile GetFile(string fileID)
        {
            if (this.Items.ContainsKey(fileID))
            {
                return this.Items[fileID];
            }
            else
            {
                //// 存在しない場合は新規に作成する。
                var dialogFile = new UapTransSheetDialogFile(fileID);
                this.Items.Add(dialogFile.FileID, dialogFile);

                return dialogFile;
            }
        }

        /// <summary>
        /// 指定した FileID, LineID の翻訳シートエントリー(Dialog)を返す。
        /// </summary>
        /// <param name="fileID">FileID</param>
        /// <param name="lineID">LineID</param>
        /// <returns>翻訳シートエントリー(Dialog)</returns>
        public UapTransSheetDialogEntry GetEntry(string fileID, string lineID)
        {
            if (this.Items.ContainsKey(fileID))
            {
                var sheetFile = this.Items[fileID];
                if (sheetFile.Items.ContainsKey(lineID))
                {
                    return sheetFile.Items[lineID];
                }
            }

            //// 存在しない場合はnullオブジェクトを返す。
            var nullEntry = new UapTransSheetDialogEntry();
            return nullEntry;
        }

        /// <summary>
        /// 翻訳シートエントリー(Dialog)を追加する。
        /// </summary>
        /// <param name="sheetEntry">翻訳シートエントリー(Dialog)</param>
        public void AddEntry(UapTransSheetDialogEntry sheetEntry)
        {
            if (this.Items.ContainsKey(sheetEntry.FileID))
            {
                var sheetFile = this.Items[sheetEntry.FileID];
                sheetFile.AddEntry(sheetEntry);
            }
            else
            {
                //// 翻訳シートファイルが存在しない時は新たに作成する。
                var sheetFile = new UapTransSheetDialogFile(sheetEntry.FileID);
                sheetFile.AddEntry(sheetEntry);

                this.Items.Add(sheetFile.FileID, sheetFile);
            }
        }
    }
}
