@rem UnityEXを使用し、日本語化対象のファイルをMonoBehaviour形式でエクスポートする。
@SET PATH="OxfJpModKit\tools";%PATH%

copy /y OxfJpModKit\bkup\resources.assets .\resources.assets

UnityEX.exe export resources.assets -mb_new -t -9
UnityEX.exe export resources.assets -mb_new -t -2

@pause
@exit /b
