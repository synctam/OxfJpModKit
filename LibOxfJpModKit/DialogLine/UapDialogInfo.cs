namespace LibOxfJpModKit.DialogLine
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Dialog情報
    /// </summary>
    public class UapDialogInfo
    {
        /// <summary>
        /// Dialogファイル辞書。
        /// キーはファイル名。
        /// </summary>
        public Dictionary<string, UapDialogFile> Items { get; } =
            new Dictionary<string, UapDialogFile>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Dialogファイルを追加する。
        /// </summary>
        /// <param name="dialogFile">Dialogファイル</param>
        public void AddFile(UapDialogFile dialogFile)
        {
            if (this.Items.ContainsKey(dialogFile.FileID))
            {
                throw new Exception($"Duplicate file id({dialogFile.FileID}).");
            }
            else
            {
                this.Items.Add(dialogFile.FileID, dialogFile);
            }
        }
    }
}
