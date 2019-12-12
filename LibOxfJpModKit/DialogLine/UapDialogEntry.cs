namespace LibOxfJpModKit.DialogLine
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Dialogエントリー
    /// </summary>
    public class UapDialogEntry
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="lineID">lineID</param>
        /// <param name="character">キャラクター名</param>
        public UapDialogEntry(string lineID, string character)
        {
            this.LineID = lineID;
            this.Character = character;
        }

        /// <summary>
        /// 言語エントリの辞書。
        /// キーは言語番号(lng)。
        /// </summary>
        public Dictionary<int, UapDialogLangEntry> LanguageEntries { get; } =
            new Dictionary<int, UapDialogLangEntry>();

        /// <summary>
        /// LineID
        /// </summary>
        public string LineID { get; } = string.Empty;

        /// <summary>
        /// キャラクター名
        /// </summary>
        public string Character { get; } = string.Empty;

        /// <summary>
        /// 言語エントリを追加する。
        /// </summary>
        /// <param name="langNo">言語番号</param>
        /// <param name="text">テキスト</param>
        /// <param name="clip">clip</param>
        /// <param name="vol">vol</param>
        public void AddEntry(int langNo, string text, string clip, float vol)
        {
            if (this.LanguageEntries.ContainsKey(langNo))
            {
                throw new Exception($"Error: Duplicate language no({langNo})");
            }
            else
            {
                UapDialogLangEntry langEntry = new UapDialogLangEntry(langNo, text, clip, vol);
                this.LanguageEntries.Add(langNo, langEntry);
            }
        }

        /// <summary>
        /// 指定した言語コードの言語エントリを返す。
        /// </summary>
        /// <param name="langCode">言語番号</param>
        /// <returns>言語エントリ</returns>
        public UapDialogLangEntry GetEntry(int langCode)
        {
            if (this.LanguageEntries.ContainsKey(langCode))
            {
                return this.LanguageEntries[langCode];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 自分自身のクローンを作成しDialogエントリーを返す。
        /// </summary>
        /// <returns>Dialogエントリー</returns>
        public UapDialogEntry Clone()
        {
            var uapDialogEntry = new UapDialogEntry(this.LineID, this.Character);
            foreach (var data in this.LanguageEntries.Values)
            {
                uapDialogEntry.LanguageEntries.Add(data.LangCode, data);
            }

            return uapDialogEntry;
        }
    }
}
