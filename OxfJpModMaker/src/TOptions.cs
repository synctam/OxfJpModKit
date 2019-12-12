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
            msg.WriteLine($@"日本語化MODを作成する。");
            msg.WriteLine(
                $@"  usage: {exeName} -d|-L -i <original lang folder path> -o <japanized lang folder path>" +
                $@" -s <Trans Sheet path> [-n <lang no>] [-m] [-r]");
            msg.WriteLine($@"OPTIONS:");
            this.optionSet.WriteOptionDescriptions(msg);
            msg.WriteLine($@"Example:");
            msg.WriteLine($@"  翻訳シート(-s)とオリジナルの言語フォルダー(-i)から日本語の言語フォルダーにDialogの日本語化MOD(-o)を作成する。");
            msg.WriteLine(
                $@"    {exeName} -d -i data\EN -o data\JP" +
                $@" -s transSheet.csv");
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
                { "i|in="            , this.args.FolderNameInputText     , v => this.args.FolderNameInput     = v},
                { "o|out="           , this.args.FolderNameOutputText    , v => this.args.FolderNameOutput    = v},
                { "s|sheet="         , this.args.FileNameSheetText       , v => this.args.FileNameSheet       = v},
                { "n|no="            , this.args.LanguageNoText          , v => this.args.StrLanguageNo          = v},
                { "m"                , this.args.UseMachineTransText     , v => this.args.UseMachineTrans     = v != null},
                { "r"                , this.args.UseReplaceText          , v => this.args.UseReplace          = v != null},
                { "h|help"           , "ヘルプ"                          , v => this.args.Help                = v != null},
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

            if (this.IsErrorSheetSystemFile())
            {
                return;
            }

            if (this.IsErrorInputFile())
            {
                return;
            }

            if (this.IsErrorOutputFile())
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

        /// <summary>
        /// コマンドの確認
        /// </summary>
        /// <returns>コマンドエラーの有無。true: エラーあり</returns>
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

        /// <summary>
        /// 翻訳シートの有無を確認
        /// </summary>
        /// <returns>翻訳シートの存在有無</returns>
        private bool IsErrorSheetSystemFile()
        {
            if (string.IsNullOrWhiteSpace(this.Arges.FileNameSheet))
            {
                this.SetErrorMessage(
                    $@"{Environment.NewLine}エラー：(-s)翻訳シートのパスを指定してください。");
                this.isError = true;

                return true;
            }
            else
            {
                if (!File.Exists(this.Arges.FileNameSheet))
                {
                    this.SetErrorMessage(
                        $"{Environment.NewLine}エラー：(-s)翻訳シートが見つかりません。" +
                        $"{Environment.NewLine}({Path.GetFullPath(this.Arges.FileNameSheet)})");
                    this.isError = true;

                    return true;
                }
            }

            return false;
        }

        private bool IsErrorInputFile()
        {
            if (string.IsNullOrWhiteSpace(this.Arges.FolderNameInput))
            {
                this.SetErrorMessage(
                    $"{Environment.NewLine}エラー：" +
                    $"(-i)オリジナル版の言語フォルダーのパスを指定してください。");
                this.isError = true;

                return true;
            }
            else
            {
                if (!Directory.Exists(this.Arges.FolderNameInput))
                {
                    this.SetErrorMessage(
                        $"{Environment.NewLine}エラー：" +
                        $"(-i)オリジナル版の言語フォルダーがありません。" +
                        $"{Environment.NewLine}({Path.GetFullPath(this.Arges.FolderNameInput)})");
                    this.isError = true;

                    return true;
                }
            }

            return false;
        }

        private bool IsErrorOutputFile()
        {
            if (string.IsNullOrWhiteSpace(this.Arges.FolderNameOutput))
            {
                this.SetErrorMessage(
                    $"{Environment.NewLine}エラー：" +
                    $"(-o)日本語化された言語ファイルを格納するフォルダーのパスを指定してください。");
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
                $"[コマンド]日本語化MOD作成(Dialog)";

            public bool CommandLocalization { get; internal set; }

            public string CommandLocalizationText { get; internal set; } =
                $"[コマンド]日本語化MOD作成(Localization)";

            public string FolderNameInput { get; internal set; }

            public string FolderNameInputText { get; internal set; } =
                "オリジナル版の言語フォルダーのパスを指定する。";

            public string FolderNameOutput { get; internal set; }

            public string FolderNameOutputText { get; internal set; } =
                "日本語化された言語フォルダーのパスを指定する。";

            public string FileNameSheet { get; set; }

            public string FileNameSheetText { get; set; } =
                "CSV形式の翻訳シートのパス名。";

            public bool UseMachineTrans { get; internal set; }

            public string UseMachineTransText { get; internal set; } =
                $"有志翻訳がない場合は機械翻訳を使用する。";

            public bool UseReplace { get; internal set; }

            public string UseReplaceText { get; internal set; } =
                $"出力用言語ファイルが既に存在する場合はを上書きする。";

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
