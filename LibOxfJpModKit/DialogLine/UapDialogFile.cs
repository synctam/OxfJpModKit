namespace LibOxfJpModKit.DialogLine
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Dialogファイル
    /// </summary>
    public class UapDialogFile
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileID">FileID</param>
        public UapDialogFile(string fileID)
        {
            this.FileID = fileID;
        }

        /// <summary>
        /// Dialogエントリーの辞書。
        /// キーはLineID。
        /// LineIDは大文字/小文字を区別する。
        /// </summary>
        public Dictionary<string, UapDialogEntry> Items { get; } =
            new Dictionary<string, UapDialogEntry>();

        /// <summary>
        /// FileID
        /// </summary>
        public string FileID { get; } = string.Empty;

        /// <summary>
        /// Dialogエントリーを追加し、そのエントリーを返す。
        /// </summary>
        /// <param name="entry">Dialogエントリー</param>
        /// <returns>追加されたDialogエントリー</returns>
        public UapDialogEntry AddEntry(UapDialogEntry entry)
        {
            if (this.Items.ContainsKey(entry.LineID))
            {
                throw new Exception($"Duplicate dialog entry({entry.LineID}). FileName({this.FileID})");
            }
            else
            {
                this.Items.Add(entry.LineID, entry);
                return entry;
            }
        }

        /// <summary>
        /// 自分自身のクローンを返す。
        /// </summary>
        /// <returns>Dialogファイル</returns>
        public UapDialogFile Clone()
        {
            var uapDialogFile = new UapDialogFile(this.FileID);
            foreach (var entry in this.Items.Values)
            {
                uapDialogFile.AddEntry(entry.Clone());
            }

            return uapDialogFile;
        }
    }
}
