// ******************************************************************************
// Copyright (c) 2015-2019 synctam
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of
// this software and associated documentation files (the "Software"), to deal in
// the Software without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
// of the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
// FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
// IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

namespace MonoOptions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Mono.Options;

    /// <summary>
    /// コマンドライン オプション
    /// </summary>
    public class TOptions
    {
        //// ******************************************************************************
        //// Property fields
        //// ******************************************************************************
        private TArgs args;
        private bool isError = false;
        private StringWriter errorMessage = new StringWriter();
        private OptionSet optionSet;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="arges">コマンドライン引数</param>
        public TOptions(string[] arges)
        {
            this.args = new TArgs();
            this.Settings(arges);
            if (this.IsError)
            {
                this.ShowErrorMessage();
                this.ShowUsage();
            }
            else
            {
                this.CheckOption();
                if (this.IsError)
                {
                    this.ShowErrorMessage();
                    this.ShowUsage();
                }
                else
                {
                    // skip
                }
            }
        }

        //// ******************************************************************************
        //// Property
        //// ******************************************************************************

        /// <summary>
        /// コマンドライン オプション
        /// </summary>
        public TArgs Arges { get { return this.args; } }

        /// <summary>
        /// コマンドライン オプションのエラー有無
        /// </summary>
        public bool IsError { get { return this.isError; } }

        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public string ErrorMessage { get { return this.errorMessage.ToString(); } }

        /// <summary>
        /// Uasgeを表示する
        /// </summary>
        public void ShowUsage()
        {
            TextWriter writer = Console.Error;
            this.ShowUsage(writer);
        }

        /// <summary>
        /// Uasgeを表示する
        /// </summary>
        /// <param name="textWriter">出力先</param>
        public void ShowUsage(TextWriter textWriter)
        {
            var msg = new StringWriter();

            string exeName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
            msg.WriteLine(string.Empty);
            msg.WriteLine($@"使い方：");
            msg.WriteLine($@"言語フォルダーから翻訳シートを作成する。");
            msg.WriteLine($@"  usage: {exeName} -d|-L -i <lang folder path> -s <trans sheet path> [-n <lang no>] [-r]");
            msg.WriteLine($@"OPTIONS:");
            this.optionSet.WriteOptionDescriptions(msg);
            msg.WriteLine($@"Example:");
            msg.WriteLine($@"  (-i)言語フォルダーから(-s)翻訳シート(Dialog)を作成する。");
            msg.WriteLine($@"    {exeName} -d -i data\en -s data\csv\TransSheet.csv");
            msg.WriteLine($@"終了コード:");
            msg.WriteLine($@" 0  正常終了");
            msg.WriteLine($@" 1  異常終了");
            msg.WriteLine();

            if (textWriter == null)
            {
                textWriter = Console.Error;
            }

            textWriter.Write(msg.ToString());
        }

        /// <summary>
        /// エラーメッセージ表示
        /// </summary>
        public void ShowErrorMessage()
        {
            TextWriter writer = Console.Error;
            this.ShowErrorMessage(writer);
        }

        /// <summary>
        /// エラーメッセージ表示
        /// </summary>
        /// <param name="textWriter">出力先</param>
        public void ShowErrorMessage(TextWriter textWriter)
        {
            if (textWriter == null)
            {
                textWriter = Console.Error;
            }

            textWriter.Write(this.ErrorMessage);
        }

        /// <summary>
        /// オプション文字の設定
        /// </summary>
        /// <param name="args">args</param>
        private void Settings(string[] args)
        {
            this.optionSet = new OptionSet()
            {
                { "d|dialog"         , this.args.CommandDialogText       , v => this.args.CommandDialog       = v != null},
                { "l|L|localization" , this.args.CommandLocalizationText , v => this.args.CommandLocalization = v != null},
                { "i|in="     , this.args.FolderNameLangInputText , v => this.args.FolderNameLangInput = v},
                { "s|sheet="  , this.args.FileNameSheetText       , v => this.args.FileNameSheet       = v},
                { "n|no="     , this.args.LanguageNoText          , v => this.args.StrLanguageNo          = v},
                { "r"         , this.args.UseReplaceText          , v => this.args.UseReplace      = v != null},
                { "h|help"    , "ヘルプ"                          , v => this.args.Help            = v != null},
            };

            List<string> extra;
            try
            {
                extra = this.optionSet.Parse(args);
                if (extra.Count > 0)
                {
                    // 指定されたオプション以外のオプションが指定されていた場合、
                    // extra に格納される。
                    // 不明なオプションが指定された。
                    this.SetErrorMessage($"{Environment.NewLine}エラー：不明なオプションが指定されました。");
                    extra.ForEach(t => this.SetErrorMessage(t));
                    this.isError = true;
                }
            }
            catch (OptionException e)
            {
                ////パースに失敗した場合OptionExceptionを発生させる
                this.SetErrorMessage(e.Message);
                this.isError = true;
            }
        }

        /// <summary>
        /// オプションのチェック
        /// </summary>
        private void CheckOption()
        {
            //// -h
            if (this.Arges.Help)
            {
                this.SetErrorMessage();
                this.isError = false;
                return;
            }

            if (this.CheckCommand())
            {
                return;
            }

            if (this.IsErrorLangInputFile())
            {
                return;
            }

            if (this.IsErrorTransSheetFile())
            {
                return;
            }

            if (this.CheckLanguageNo())
            {
                return;
            }

            this.isError = false;
            return;
        }

        private bool CheckCommand()
        {
            if (this.args.CommandDialog && this.args.CommandLocalization)
            {
                //// エラー：コマンドが複数指定された。
                this.SetErrorMessage(
                    $@"{Environment.NewLine}エラー：(-d/-l)複数のコマンドが存在します。{Environment.NewLine}コマンドはどちらか一方を指定してください。");
                this.isError = true;

                return true;
            }
            else if (!this.args.CommandDialog && !this.args.CommandLocalization)
            {
                //// エラー：コマンドが指定されていない。
                this.SetErrorMessage(
                    $@"{Environment.NewLine}エラー：コマンドがありません。{Environment.NewLine}-d または -l コマンドを指定してください。");
                this.isError = true;

                return true;
            }

            return false;
        }

        private bool IsErrorLangInputFile()
        {
            if (string.IsNullOrWhiteSpace(this.Arges.FolderNameLangInput))
            {
                this.SetErrorMessage($@"{Environment.NewLine}エラー：(-i)オリジナル版の言語フォルダーのパスを指定してください。");
                this.isError = true;

                return true;
            }
            else
            {
                if (!Directory.Exists(this.Arges.FolderNameLangInput))
                {
                    this.SetErrorMessage($@"{Environment.NewLine}エラー：(-i)オリジナル版の言語フォルダーがみつかりません。{Environment.NewLine}({Path.GetFullPath(this.Arges.FolderNameLangInput)})");
                    this.isError = true;

                    return true;
                }
            }

            return false;
        }

        private bool IsErrorTransSheetFile()
        {
            if (string.IsNullOrWhiteSpace(this.Arges.FileNameSheet))
            {
                this.SetErrorMessage($@"{Environment.NewLine}エラー：(-s)翻訳シートファイルのパスを指定してください。");
                this.isError = true;

                return true;
            }

            if (File.Exists(this.Arges.FileNameSheet) && !this.args.UseReplace)
            {
                this.SetErrorMessage(
                    $"{Environment.NewLine}" +
                    $@"エラー：(-s)翻訳シートファイルが既に存在します。{Environment.NewLine}" +
                    $@"({Path.GetFullPath(this.Arges.FileNameSheet)}){Environment.NewLine}" +
                    $@"上書きする場合は '-r' オプションを指定してください。");
                this.isError = true;

                return true;
            }

            return false;
        }

        private bool CheckLanguageNo()
        {
            int value = 0;
            int.TryParse(this.args.StrLanguageNo, out value);
            if (value == 0)
            {
                this.args.LanguageNo = 10;
            }
            else
            {
                this.args.LanguageNo = value;
            }

            return false;
        }

        private void SetErrorMessage(string errorMessage = null)
        {
            if (errorMessage != null)
            {
                this.errorMessage.WriteLine(errorMessage);
            }
        }

        /// <summary>
        /// オプション項目
        /// </summary>
        public class TArgs
        {
            public bool CommandDialog { get; internal set; }

            public string CommandDialogText { get; internal set; } =
                $"[コマンド]翻訳シートの作成(Dialog)";

            public bool CommandLocalization { get; internal set; }

            public string CommandLocalizationText { get; internal set; } =
                $"[コマンド]翻訳シートの作成(Localization)";

            public string FolderNameLangInput { get; internal set; }

            public string FolderNameLangInputText { get; internal set; } =
                "オリジナル版の言語フォルダーのパスを指定する。";

            public string FileNameSheet { get; set; }

            public string FileNameSheetText { get; set; } =
                "CSV形式の翻訳シートのパス名。";

            public bool UseReplace { get; internal set; }

            public string UseReplaceText { get; internal set; } = $"翻訳シートが既に存在する場合はを上書きする。";

            public string StrLanguageNo { get; internal set; }

            public int LanguageNo { get; set; }

            public string LanguageNoText { get; internal set; } =
                $"言語番号。省略時は英語(10)" +
                $"{Environment.NewLine}" +
                $"10:English, 14:French, 15:German, 21:Italian, 30:Russian, 34:Spanish, 40:Chinese(Simplified)";

            public bool Help { get; set; }
        }
    }
}
