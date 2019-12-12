namespace OxfJpModMaker
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using LibOxfJpModKit.DialogLine;
    using LibOxfJpModKit.Localization;
    using LibOxfJpModKit.Translation;
    using LibOxfJpModKit.TransSheet.DialogLine;
    using LibOxfJpModKit.TransSheet.Localization;
    using MonoOptions;
    using S5mDebugTools;

    /// <summary>
    /// 日本語化MODを作成する。
    /// </summary>
    internal class Program
    {
        private static int Main(string[] args)
        {
            var opt = new TOptions(args);
            if (opt.IsError)
            {
                TDebugUtils.Pause();
                return 1;
            }

            if (opt.Arges.Help)
            {
                opt.ShowUsage();

                TDebugUtils.Pause();
                return 1;
            }

            try
            {
                if (opt.Arges.CommandDialog)
                {
                    SaveDialog(opt.Arges);
                }
                else if (opt.Arges.CommandLocalization)
                {
                    SaveLocalize(opt.Arges);
                }
                else
                {
                    throw new Exception($"Unknown error.");
                }

                TDebugUtils.Pause();
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

                TDebugUtils.Pause();
                return 1;
            }
        }

        private static void SaveDialog(TOptions.TArgs opt)
        {
            //// 翻訳シートの読み込み
            var sheetDialogInfo = new UapTransSheetDialogInfo();
            string dialogPath = opt.FileNameSheet;
            UapTransSheetDialogDao.LoadFromCsv(sheetDialogInfo, dialogPath);

            //// 言語情報(原文)の読み込み
            var langDialogInfoEN = new UapDialogInfo();
            UapDialogDao.LoadFromFolder(
                langDialogInfoEN,
                opt.FolderNameInput,
                "*.DialogPackage");

            //// 翻訳済み言語情報の作成
            var langDialogInfoJP =
                UapTranslation.TranslateDialog(sheetDialogInfo, langDialogInfoEN, opt.LanguageNo, opt.UseMachineTrans);

            UapDialogDao.SaveToFolder(opt.FolderNameOutput, langDialogInfoJP, opt.UseReplace);
        }

        private static void SaveLocalize(TOptions.TArgs opt)
        {
            //// 翻訳シートの読み込み
            var sheetLocalizationInfo = new UapTransSheetLocalizationInfo();
            string locPath = opt.FileNameSheet;
            UapTransSheetLocalizationDao.LoadFromCsv(sheetLocalizationInfo, locPath);

            //// 言語情報(原文)の読み込み
            var localizationInfoEN = new UapLocalizationInfo();
            UapLocalizationDao.LoadFromFolder(
                localizationInfoEN,
                opt.FolderNameInput,
                "*.LocalizationReference");

            //// 翻訳済み言語情報の作成
            var localizationInfoJP =
                UapTranslation.TranslateLocalization(sheetLocalizationInfo, localizationInfoEN, opt.LanguageNo, opt.UseMachineTrans);

            UapLocalizationDao.SaveToFolder(opt.FolderNameOutput, localizationInfoJP, opt.UseReplace);
        }
    }
}
