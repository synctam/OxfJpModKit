namespace OxfSheetMaker
{
    using System;
    using LibOxfJpModKit.DialogLine;
    using LibOxfJpModKit.Localization;
    using LibOxfJpModKit.TransSheet.DialogLine;
    using LibOxfJpModKit.TransSheet.Localization;
    using MonoOptions;
    using S5mDebugTools;

    /// <summary>
    /// 翻訳シートを作成する。
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
                    SheetDialog(opt.Arges);
                }
                else if (opt.Arges.CommandLocalization)
                {
                    SheetLocalization(opt.Arges);
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

        private static void SheetDialog(TOptions.TArgs opt)
        {
            var dialogInfo = new UapDialogInfo();
            UapDialogDao.LoadFromFolder(
                dialogInfo,
                opt.FolderNameLangInput,
                "*.DialogPackage");
            UapTransSheetDialogDao.SaveToCsv(
                dialogInfo, opt.FileNameSheet, opt.LanguageNo);
        }

        private static void SheetLocalization(TOptions.TArgs opt)
        {
            //// 【注意事項】
            //// GOG版(v2.7.1)のデータ "resources_00006.-9" にはバグが有る。
            //// FileID(LocalizationReference_Map_SELFNAME)
            //// ID(1190585287)のLangID(10)が重複し項目数も一つ多い。
            //// LoadFromFolder()時にエラーメッセージが表示されるが無視して良い。
            var localizeInfo = new UapLocalizationInfo();
            UapLocalizationDao.LoadFromFolder(
                localizeInfo,
                opt.FolderNameLangInput,
                "*.LocalizationReference");
            UapTransSheetLocalizationDao.SaveToCsv(
                localizeInfo, opt.FileNameSheet, opt.LanguageNo);
        }
    }
}
