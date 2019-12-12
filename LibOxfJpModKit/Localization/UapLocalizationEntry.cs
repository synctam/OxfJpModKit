namespace LibOxfJpModKit.Localization
{
    using System.Collections.Generic;

    /// <summary>
    /// ローカライズエントリー
    /// </summary>
    public class UapLocalizationEntry
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="id">ID</param>
        public UapLocalizationEntry(string name, string id)
        {
            this.Name = name;
            this.ID = id;
        }

        /// <summary>
        /// テキストエントリの辞書。
        /// キーは言語番号。
        /// </summary>
        public Dictionary<int, string> LanguageEntries { get; } =
            new Dictionary<int, string>();

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; } = string.Empty;

        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; } = string.Empty;

        /// <summary>
        /// 言語エントリを追加する。
        /// </summary>
        /// <param name="langNo">言語番号</param>
        /// <param name="text">テキスト</param>
        /// <returns>成否</returns>
        public bool AddEntry(int langNo, string text)
        {
            if (this.LanguageEntries.ContainsKey(langNo))
            {
                return false;
            }
            else
            {
                this.LanguageEntries.Add(langNo, text);
                return true;
            }
        }

        /// <summary>
        /// 指定した言語番号のテキストを返す。
        /// </summary>
        /// <param name="langCode">言語番号</param>
        /// <returns>テキスト</returns>
        public string GetEntry(int langCode)
        {
            if (this.LanguageEntries.ContainsKey(langCode))
            {
                return this.LanguageEntries[langCode];
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 自分自身のクローンを作成しローカライズエントリーを返す。
        /// </summary>
        /// <returns>ローカライズエントリー</returns>
        public UapLocalizationEntry Clone()
        {
            var entry = new UapLocalizationEntry(this.Name, this.ID);
            foreach (var langPair in this.LanguageEntries)
            {
                entry.LanguageEntries.Add(langPair.Key, langPair.Value);
            }

            return entry;
        }
    }
}
