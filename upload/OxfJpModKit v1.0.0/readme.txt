Oxenfree日本語化MOD作成支援ツール

このツールは Oxenfree を日本語化を支援するためのツールをまとめたものです。
このツールには日本語データは含まれないため、各自で翻訳をお願いします。

■使い方
1.準備
1.1.UnityEXのダウンロードと移動
このリンクからUnityEXをダウンロードし、解凍します。
「UnityEX - Вскрытие игровых ресурсов - Zone of Games Forum」
https://forum.zoneofgames.ru/topic/36240-unityex/

"OxfJpModKit\tools" フォルダーに UnityEX.exe を移動します。

1.2.ゲームフォルダーへ日本語化支援ツールを移動する。
以下のフォルダーとバッチファイルを OxenfreeのOxenfree_Dataフォルダーに移動します。
・OxfJpModKitフォルダー
・00.backup.bat
・10.unpacMB.bat
・11.OxfSheetMaker.bat
・12.OxfJpModMaker.bat
・13.OxfJpModApply.bat
・20.RepacMB.bat

2.バックアップの取得
オリジナルの "resources.assets" をバックアップします。
バッチファイル "00.backup.bat" をダブルクリックしバックアップを実行します。
これで、"OxfJpModKit\bkup" フォルダーにオリジナルの "resources.assets" がコピーされました。
この処理は最初に一度だけ実行してください。
なお、ゲームのアップデートがあった場合は再度実行する必要があります。

3.アンパック
UnityEXを使用し日本語化対象のファイルをアンパックします。
バッチファイル "10.unpacMB.bat" をダブルクリックしアンパックを実行します。
これで、Unity_Assets_Files フォルダーが作成され、日本語化対象のファイルがアンパックされました。

4.翻訳シートの作成
アンパックされたファイルから翻訳シートを作成します。
バッチファイル "11.OxfSheetMaker.bat" をダブルクリックし翻訳シート作成を実行します。
これで、"OxfJpModKit\csv" フォルダーに以下の翻訳シートが作成されました。
・tranOxfSheetDialog.csv
・tranOxfSheetLocalization.csv

5.翻訳
作成された翻訳シートを Googleスプレッドシートなどを使用し、翻訳します。
なお、"OxfJpModKit\csv" フォルダーにはサンプルの翻訳シートが入っていますので参考にしてください。

6.日本語化MODの作成
翻訳シートを使用し、日本語化MODを作成します。
バッチファイル "12.OxfJpModMaker.bat" をダブルクリックし日本語化MOD作成を実行します。
このまま実行するとサンプルの翻訳シートを使用し日本語化MODを作成します。
必要に応じバッチファイルの翻訳シートのファイル名を変更してください。

7.日本語化MODのコピー
UnityEXでインポートできるよう、作成した日本語化MODを所定の位置にコピーします。
バッチファイル "13.OxfJpModApply.bat" をダブルクリックし日本語化MODのコピーを実行します。
これで、Unity_Assets_Files フォルダー内の所定に位置に日本語化MODがコピーされました。

8.リパック
UnityEXを使用し日本語化対象のファイルをリパックします。
バッチファイル "20.RepacMB.bat" をダブルクリックしリパックを実行します。
これで、日本語化された "resources.assets" が作成されました。

9.日本語表示の確認
ゲームを起動し、日本語が表示されていることを確認してください。

■英語版への戻し方
「2.バックアップの取得」で取得した "resources.assets" をゲームフォルダーの "Oxenfree_Data" にコピーしてください。
なお、Steamであれば、Oxenfree のプロパティを開き、"ローカルファイル"タブ の "ゲームファイルの整合性を確認..." ボタンを押すことで英語版に戻せます。

■免責
日本語化MOD作成支援ツール、バッチファイル、データおよびこの手順を使用した事によるいかなる損害も作者は一切の責任を負いません。
自己の責任で使用して下さい。

■ライセンス
このツールは MIT ライセンスで公開します。

■ライブラリー
このツールでは以下のライブラリーを使用しています。各ライブラリーのライセンスはURLを参照願いします。

・CsvHelper.12.2.2
「CsvHelper」
https://joshclose.github.io/CsvHelper/

・Mono.Options.5.3.0.1
「XamarinComponents/XPlat/Mono.Options at master · xamarin/XamarinComponents」
https://github.com/xamarin/XamarinComponents/tree/master/XPlat/Mono.Options

■連絡先
synctam@gmail.com

■変更履歴
2019.12.12  初版公開

以上
