@rem UnityEX���g�p���A���{�ꉻ�Ώۂ̃t�@�C����MonoBehaviour�`���ŃG�N�X�|�[�g����B
@SET PATH="OxfJpModKit\tools";%PATH%

copy /y OxfJpModKit\bkup\resources.assets .\resources.assets

UnityEX.exe export resources.assets -mb_new -t -9
UnityEX.exe export resources.assets -mb_new -t -2

@pause
@exit /b
