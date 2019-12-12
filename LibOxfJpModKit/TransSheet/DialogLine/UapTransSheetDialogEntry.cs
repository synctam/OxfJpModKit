namespace LibOxfJpModKit.TransSheet.DialogLine
{
    /// <summary>
    /// 翻訳シートエントリー(Dialog)
    /// </summary>
    public class UapTransSheetDialogEntry
    {
        /// <summary>
        /// FileID
        /// </summary>
        public string FileID { get; set; }

        /// <summary>
        /// LineID
        /// </summary>
        public string LineID { get; set; }

        /// <summary>
        /// キャラクター名
        /// </summary>
        public string Character { get; set; }

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
