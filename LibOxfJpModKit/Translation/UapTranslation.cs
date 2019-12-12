namespace LibOxfJpModKit.Translation
{
    using System;
    using LibOxfJpModKit.DialogLine;
    using LibOxfJpModKit.Localization;
    using LibOxfJpModKit.TransSheet.DialogLine;
    using LibOxfJpModKit.TransSheet.Localization;

    /// <summary>
    /// 翻訳
    /// </summary>
    public class UapTranslation
    {
        /// <summary>
        /// Dialog情報(英語版)と翻訳シート情報(Dialog)を使用し、翻訳を行い、
        /// 翻訳済みDialog情報を返す。
        /// </summary>
        /// <param name="transSheetDialogInfo">翻訳シート情報(Dialog)</param>
        /// <param name="langDialogInfoEN">Dialog情報(英語版)</param>
        /// <param name="langNo">言語番号</param>
        /// <param name="useMT">機械翻訳の使用有無</param>
        /// <returns>翻訳済みDialog情報</returns>
        public static UapDialogInfo TranslateDialog(
            UapTransSheetDialogInfo transSheetDialogInfo,
            UapDialogInfo langDialogInfoEN,
            int langNo,
            bool useMT)
        {
            var langDialogInfoJP = new UapDialogInfo();

            //// Dialogファイル(英語版)を元に翻訳を行い、Dialogファイル(日本語版)を作成する。
            foreach (var langDialogFileEN in langDialogInfoEN.Items.Values)
            {
                //// Dialogファイル(英語版)のクローンを作成し、Dialogファイル(日本語版)の雛形とする。
                var langDialogFileJP = langDialogFileEN.Clone();
                langDialogInfoJP.AddFile(langDialogFileJP);

                foreach (var langEntryJP in langDialogFileJP.Items.Values)
                {
                    //// 該当エントリの翻訳シートエントリーを取得。
                    UapTransSheetDialogEntry sheetEntry =
                        transSheetDialogInfo.GetEntry(
                            langDialogFileEN.FileID, langEntryJP.LineID);
                    //// Dialogエントリーを翻訳する。
                    langEntryJP.LanguageEntries[langNo].Text = GetTranslatedText(
                        langEntryJP.LanguageEntries[langNo].Text,
                        sheetEntry.Japanese,
                        sheetEntry.MTrans,
                        useMT);
                }
            }

            return langDialogInfoJP;
        }

        /// <summary>
        /// ローカライズ情報(英語版)と翻訳シート情報(Localization)を使用し、翻訳を行い、
        /// 翻訳済みDialog情報を返す。
        /// </summary>
        /// <param name="transSheetLocalizationInfo">翻訳シート情報(Localization)</param>
        /// <param name="localizationInfoEN">ローカライズ情報(英語版)</param>
        /// <param name="langNo">言語番号</param>
        /// <param name="useMT">機械翻訳の使用有無</param>
        /// <returns>翻訳済みローカライズ情報</returns>
        public static UapLocalizationInfo TranslateLocalization(
            UapTransSheetLocalizationInfo transSheetLocalizationInfo,
            UapLocalizationInfo localizationInfoEN,
            int langNo,
            bool useMT)
        {
            var localizationInfoJP = new UapLocalizationInfo();

            //// ローカライズファイル(英語版)を元に翻訳を行い、ローカライズファイル(日本語版)を作成する。
            foreach (var localizationFileEN in localizationInfoEN.Items.Values)
            {
                //// ローカライズファイル(英語版)のクローンを作成し、ローカライズファイル(日本語版)の雛形とする。
                var localizationFileJP = localizationFileEN.Clone();
                localizationInfoJP.AddFile(localizationFileJP);

                foreach (var localizationEntryJP in localizationFileJP.Items.Values)
                {
                    //// 該当エントリの翻訳シートエントリーを取得。
                    UapTransSheetLocalizationEntry sheetEntry =
                        transSheetLocalizationInfo.GetEntry(
                            localizationFileEN.FileID, localizationEntryJP.ID);
                    //// 言語エントリーを翻訳
                    localizationEntryJP.LanguageEntries[langNo] = GetTranslatedText(
                        localizationEntryJP.LanguageEntries[langNo],
                        sheetEntry.Japanese,
                        sheetEntry.MTrans,
                        useMT);
                }
            }

            return localizationInfoJP;
        }

        /// <summary>
        /// 翻訳済みのテキストを返す。
        /// </summary>
        /// <param name="textEN">原文</param>
        /// <param name="textJP">翻訳文</param>
        /// <param name="textMT">機械翻訳文</param>
        /// <param name="useMT">機械翻訳の使用有無</param>
        /// <returns>翻訳済みのテキスト</returns>
        private static string GetTranslatedText(string textEN, string textJP, string textMT, bool useMT)
        {
            //// EN | JP | MT | result
            //// ---+----+----+---------
            ////  o |  o |  o | JP
            ////  o |  o |  x | JP
            ////  o |  x |  o | MT <-> EN
            ////  o |  x |  x | EN
            ////  x |  o |  o | EN
            ////  x |  o |  x | EN
            ////  x |  x |  o | EN
            ////  x |  x |  x | EN
            bool en = !string.IsNullOrWhiteSpace(textEN);
            bool jp = !string.IsNullOrWhiteSpace(textJP);
            bool mt = !string.IsNullOrWhiteSpace(textMT);
            if      (en && jp && mt)
            {
                return textJP;
            }
            else if (en && jp && !mt)
            {
                return textJP;
            }
            else if (en && !jp && mt)
            {
                if (useMT)
                {
                    return textMT;
                }
                else
                {
                    return textEN;
                }
            }
            else if (en && !jp && !mt)
            {
                return textEN;
            }
            else if (!en && jp && mt)
            {
                return textEN;
            }
            else if (!en && jp && !mt)
            {
                return textEN;
            }
            else if (!en && !jp && mt)
            {
                return textEN;
            }
            else if (!en && !jp && !mt)
            {
                return textEN;
            }
            else
            {
                throw new Exception($"Logic error.");
            }
        }
    }
}
