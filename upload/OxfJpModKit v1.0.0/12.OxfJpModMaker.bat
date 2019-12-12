@rem Oxenfree ÇÃì˙ñ{åÍâªMODÇÃçÏê¨ÅB
@SET PATH="OxfJpModKit\tools";%PATH%

@rem Dialog
OxfJpModMaker.exe ^
  -d ^
  -i "Unity_Assets_Files\resources\Mono\Assembly-CSharp" ^
  -o "OxfJpModKit\JpMod" ^
  -s "OxfJpModKit\csv\OxenfreeTransSheetDialogSample.csv" ^
  -m ^
  -r

@rem Localization
OxfJpModMaker.exe ^
  -L ^
  -i "Unity_Assets_Files\resources\Mono\Assembly-CSharp" ^
  -o "OxfJpModKit\JpMod" ^
  -s "OxfJpModKit\csv\OxenfreeTransSheetLocalizationSample.csv" ^
  -m ^
  -r



@pause
@exit /b
