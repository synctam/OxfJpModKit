@rem Oxenfree �|��V�[�g���쐬����B
@SET PATH="OxfJpModKit\tools";%PATH%

OxfSheetMaker.exe -d -i "Unity_Assets_Files\resources\Mono\Assembly-CSharp" -s "OxfJpModKit\csv\tranOxfSheetDialog.csv" -r
OxfSheetMaker.exe -L -i "Unity_Assets_Files\resources\Mono\Assembly-CSharp" -s "OxfJpModKit\csv\tranOxfSheetLocalization.csv" -r


@pause
@exit /b
