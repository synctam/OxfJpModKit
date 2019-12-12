# OxfJpModKit

Oxenfree 日本語化MOD作成支援ツール


## 翻訳対象ファイルの Unpack

UnityEXを使用し、MonoBehaviour形式で export します。対象のタイプは、Dialogが "-2"、Localizationは "-9" です。
    
    UnityEX.exe export resources.assets -mb_new -t -9
    UnityEX.exe export resources.assets -mb_new -t -2


## 翻訳済みファイルの Repack

UnityEXを使用し、MonoBehaviour形式で import します。
    
    UnityEX.exe import resources.assets -mb_new


## OxfSheetMaker

    言語フォルダーから翻訳シートを作成する。
      usage: OxfSheetMaker.exe -d|-L -i <lang folder path> -s <trans sheet path> [-n <lang no>] [-r]
    OPTIONS:
      -d, --dialog               [コマンド]翻訳シートの作成(Dialog)
      -l, -L, --localization     [コマンド]翻訳シートの作成(Localization)
      -i, --in=VALUE             オリジナル版の言語フォルダーのパスを指定する。
      -s, --sheet=VALUE          CSV形式の翻訳シートのパス名。
      -n, --no=VALUE             言語番号。省略時は英語(10)
                                   10:English, 14:French, 15:German, 21:Italian, 30:
                                   Russian, 34:Spanish, 40:Chinese(Simplified)
      -r                         翻訳シートが既に存在する場合はを上書きする。
      -h, --help                 ヘルプ
    Example:
      (-i)言語フォルダーから(-s)翻訳シート(Dialog)を作成する。
        OxfSheetMaker.exe -d -i data\en -s data\csv\TransSheet.csv
    終了コード:
     0  正常終了
     1  異常終了


## OxfJpModMaker

    日本語化MODを作成する。
      usage: OxfJpModMaker.exe -d|-L -i <original lang folder path> -o <japanized lang folder path> -s <Trans Sheet path> [-n <lang no>] [-m] [-r]
    OPTIONS:
      -d, --dialog               [コマンド]日本語化MOD作成(Dialog)
      -l, -L, --localization     [コマンド]日本語化MOD作成(Localization)
      -i, --in=VALUE             オリジナル版の言語フォルダーのパスを指定する。
      -o, --out=VALUE            日本語化された言語フォルダーのパスを指定する。
      -s, --sheet=VALUE          CSV形式の翻訳シートのパス名。
      -n, --no=VALUE             言語番号。省略時は英語(10)
                                   10:English, 14:French, 15:German, 21:Italian, 30:
                                   Russian, 34:Spanish, 40:Chinese(Simplified)
      -m                         有志翻訳がない場合は機械翻訳を使用する。
      -r                         出力用言語ファイルが既に存在する場合はを上書きする。
      -h, --help                 ヘルプ
    Example:
      翻訳シート(-s)とオリジナルの言語フォルダー(-i)から日本語の言語フォルダーにDialogの日本語化MOD(-o)を作成する。
        OxfJpModMaker.exe -d -i data\EN -o data\JP -s transSheet.csv
    終了コード:
     0  正常終了
     1  異常終了

