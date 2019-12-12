# OxfJpModKit

Oxenfree ���{�ꉻMOD�쐬�x���c�[��


## �|��Ώۃt�@�C���� Unpack

UnityEX���g�p���AMonoBehaviour�`���� export ���܂��B�Ώۂ̃^�C�v�́ADialog�� "-2"�ALocalization�� "-9" �ł��B
    
    UnityEX.exe export resources.assets -mb_new -t -9
    UnityEX.exe export resources.assets -mb_new -t -2


## �|��ς݃t�@�C���� Repack

UnityEX���g�p���AMonoBehaviour�`���� import ���܂��B
    
    UnityEX.exe import resources.assets -mb_new


## OxfSheetMaker

    ����t�H���_�[����|��V�[�g���쐬����B
      usage: OxfSheetMaker.exe -d|-L -i <lang folder path> -s <trans sheet path> [-n <lang no>] [-r]
    OPTIONS:
      -d, --dialog               [�R�}���h]�|��V�[�g�̍쐬(Dialog)
      -l, -L, --localization     [�R�}���h]�|��V�[�g�̍쐬(Localization)
      -i, --in=VALUE             �I���W�i���ł̌���t�H���_�[�̃p�X���w�肷��B
      -s, --sheet=VALUE          CSV�`���̖|��V�[�g�̃p�X���B
      -n, --no=VALUE             ����ԍ��B�ȗ����͉p��(10)
                                   10:English, 14:French, 15:German, 21:Italian, 30:
                                   Russian, 34:Spanish, 40:Chinese(Simplified)
      -r                         �|��V�[�g�����ɑ��݂���ꍇ�͂��㏑������B
      -h, --help                 �w���v
    Example:
      (-i)����t�H���_�[����(-s)�|��V�[�g(Dialog)���쐬����B
        OxfSheetMaker.exe -d -i data\en -s data\csv\TransSheet.csv
    �I���R�[�h:
     0  ����I��
     1  �ُ�I��


## OxfJpModMaker

    ���{�ꉻMOD���쐬����B
      usage: OxfJpModMaker.exe -d|-L -i <original lang folder path> -o <japanized lang folder path> -s <Trans Sheet path> [-n <lang no>] [-m] [-r]
    OPTIONS:
      -d, --dialog               [�R�}���h]���{�ꉻMOD�쐬(Dialog)
      -l, -L, --localization     [�R�}���h]���{�ꉻMOD�쐬(Localization)
      -i, --in=VALUE             �I���W�i���ł̌���t�H���_�[�̃p�X���w�肷��B
      -o, --out=VALUE            ���{�ꉻ���ꂽ����t�H���_�[�̃p�X���w�肷��B
      -s, --sheet=VALUE          CSV�`���̖|��V�[�g�̃p�X���B
      -n, --no=VALUE             ����ԍ��B�ȗ����͉p��(10)
                                   10:English, 14:French, 15:German, 21:Italian, 30:
                                   Russian, 34:Spanish, 40:Chinese(Simplified)
      -m                         �L�u�|�󂪂Ȃ��ꍇ�͋@�B�|����g�p����B
      -r                         �o�͗p����t�@�C�������ɑ��݂���ꍇ�͂��㏑������B
      -h, --help                 �w���v
    Example:
      �|��V�[�g(-s)�ƃI���W�i���̌���t�H���_�[(-i)������{��̌���t�H���_�[��Dialog�̓��{�ꉻMOD(-o)���쐬����B
        OxfJpModMaker.exe -d -i data\EN -o data\JP -s transSheet.csv
    �I���R�[�h:
     0  ����I��
     1  �ُ�I��

