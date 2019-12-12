namespace LibOxfJpModKit.Localization
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// ローカライズ情報
    /// </summary>
    public class UapLocalizationInfo
    {
        /// <summary>
        /// ローカライズファイル辞書。
        /// キーはFileID。
        /// </summary>
        public Dictionary<string, UapLocalizationFile> Items { get; } =
            new Dictionary<string, UapLocalizationFile>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// ローカライズファイルを追加する。
        /// </summary>
        /// <param name="localizationFile">ローカライズファイル</param>
        public void AddFile(UapLocalizationFile localizationFile)
        {
            if (this.Items.ContainsKey(localizationFile.FileID))
            {
                throw new Exception($"Duplicate file id({localizationFile.FileID}).");
            }
            else
            {
                this.Items.Add(localizationFile.FileID, localizationFile);
            }
        }
    }
}
