namespace LibOxfJpModKit.DialogLine
{
    /// <summary>
    /// Dialog言語エントリー
    /// </summary>
    public class UapDialogLangEntry
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="langNo">言語番号</param>
        /// <param name="text">テキスト</param>
        /// <param name="clip">Clip</param>
        /// <param name="vol">Vol</param>
        public UapDialogLangEntry(int langNo, string text, string clip, float vol)
        {
            this.LangCode = langNo;
            this.Text = text;
            this.Clip = clip;
            this.Vol = vol;
        }

        /// <summary>
        /// 言語番号
        /// </summary>
        public int LangCode { get; } = 0;

        /// <summary>
        /// テキスト
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Clip
        /// </summary>
        public string Clip { get; } = string.Empty;

        /// <summary>
        /// Vol
        /// </summary>
        public float Vol { get; } = 0f;
    }
}
