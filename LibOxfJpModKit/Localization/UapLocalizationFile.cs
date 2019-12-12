namespace LibOxfJpModKit.Localization
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// ローカライズファイル
    /// </summary>
    public class UapLocalizationFile
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        public UapLocalizationFile(string fileName)
        {
            this.FileID = fileName;
        }

        /// <summary>
        /// ローカライズエントリーの辞書。キーはID。
        /// Nameは大文字/小文字を区別しない。
        /// </summary>
        public Dictionary<string, UapLocalizationEntry> Items { get; } =
            new Dictionary<string, UapLocalizationEntry>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// ファイルID
        /// </summary>
        public string FileID { get; } = string.Empty;

        /// <summary>
        /// ローカライズエントリーを追加する。
        /// </summary>
        /// <param name="entry">ローカライズエントリー</param>
        public void AddEntry(UapLocalizationEntry entry)
        {
            if (this.Items.ContainsKey(entry.ID))
            {
                throw new Exception($"Duplicate dialog entry. ID({entry.ID}) Name({entry.Name}) FileID({this.FileID})");
            }
            else
            {
                this.Items.Add(entry.ID, entry);
            }
        }

        /// <summary>
        /// 自分自身のクローンを返す。
        /// </summary>
        /// <returns>ローカライズファイル</returns>
        public UapLocalizationFile Clone()
        {
            UapLocalizationFile uapLocalizationFile = new UapLocalizationFile(this.FileID);
            foreach (var entry in this.Items.Values)
            {
                uapLocalizationFile.AddEntry(entry.Clone());
            }

            return uapLocalizationFile;
        }
    }
}
