namespace LibOxfJpModKit.TransSheet.Localization
{
    /// <summary>
    /// 翻訳シートエントリー(Localization)
    /// </summary>
    public class UapTransSheetLocalizationEntry
    {
        /// <summary>
        /// FileID
        /// </summary>
        public string FileID { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// テキスト(原文)
        /// </summary>
        public string English { get; set; }

        /// <summary>
        /// テキスト(日本語)
        /// </summary>
        public string Japanese { get; set; }

        /// <summary>
        /// テキスト(機械翻訳)
        /// </summary>
        public string MTrans { get; set; }

        /// <summary>
        /// シーケンス番号
        /// </summary>
        public int Sequence { get; set; }
    }
}
