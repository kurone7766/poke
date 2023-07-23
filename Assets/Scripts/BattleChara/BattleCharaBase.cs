using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===�퓬����L�����̏��̌��ƂȂ�f�[�^���Ǘ�����X�N���v�g===//

//�o�g���L�����̃}�X�^�[�f�[�^�F�O������ύX���Ȃ��i�C���X�y�N�^�[�����ύX�\�j
[CreateAssetMenu]
public class BattleCharaBase : ScriptableObject
{
    //�e��ڍאݒ�(���O�A�����A�摜�A�^�C�v�A�X�e�[�^�X)
    //���O
    [SerializeField] new string name;       //�L�����̖��O
    [SerializeField] string hudname;        //�L�����̖��O(HUD��̖��O)
    [TextArea]
    [SerializeField] string description;    //�L�����̐���

    //�摜
    [SerializeField] Sprite leftSprite;     //�L������(�v���C���[��)
    [SerializeField] Sprite rightSprite;    //�L������(�G�l�~�[��)

    //�^�C�v
    [SerializeField] BattleCharaType type1;
    [SerializeField] BattleCharaType type2;

    //�X�e�[�^�X [HP,ATK,DEF,SPE]�@           //public int�ɂ���ƊO������ύX�ł��Ă��܂��B
    [SerializeField] int maxHP;
    [SerializeField] int attack;    
    [SerializeField] int defense;
    [SerializeField] int speed;

    //�o����Z�ꗗ
    [SerializeField] List<LearnableMove> learnableMoves;

    //�v���p�e�B�ݒ�                           //���t�@�C������attack�̒l�͎擾�ł��邪�ύX�͂ł��Ȃ��B
    public int MaxHP    { get => maxHP;     }
    public int Attack   { get => attack;    }
    public int Defense  { get => defense;   }
    public int Speed    { get => speed;     }

    public List<LearnableMove> LearnableMoves { get => learnableMoves; }
    public string Name              { get => name; }
    public string Hudname           { get => hudname; }
    public string Description       { get => description; }
    public Sprite RightSprite       { get => rightSprite; }
    public Sprite LeftSprite        { get => leftSprite; }
    public BattleCharaType Type1    { get => type1; }
    public BattleCharaType Type2    { get => type2; }
}

//�o����Z�N���X�G�ǂ̃��x���ŉ����o����̂�
[Serializable]
public class LearnableMove
{
    //�q�G�����L�[�Őݒ肷��
    [SerializeField] MoveBase _base;
    [SerializeField] int level;

    public MoveBase Base { get => _base; }
    public int Level { get => level; }
}

//�o�g���L�����̃^�C�v�ݒ�
public enum BattleCharaType
{
    None,                   //�ݒ薳��
    Normal,                 //������
    Red,                    //�ԑ���
    Blue,                   //����
    Green,                  //�Α���
    Yellow,                 //������
}

//�퓬���̔\�͕ω��Z���g�p����Ƃ��p
public enum Stat
{
    Attack,
    Defense,
    Speed,
}


//�킴�̑����v�Z   (�ǂ�����ł��Q�Ɖ\�j��BattleChara��TakeDamege�Ŕ��f
public class TypeChart
{
    //�^�C�v�̑��ΐ}�i�Q�ƒl�j
    static float[][] chart =
    {
            //�U���_�h��i���ォ��E���̎ΐ����Ђ��ꂽ���ΐ}���C���[�W�j
            //                     nor   red   blu   gre   yel   
            /*nor*/new float[]{    1f,   1f,   1f,   1f,   1f},  //normal(�m�[�}��)
            /*red*/new float[]{    1f, 0.5f, 0.5f,   2f,   1f},  //red(��)
            /*blu*/new float[]{    1f,   2f, 0.5f, 0.5f,   1f},  //blue(��)
            /*gre*/new float[]{    1f, 0.5f,   2f, 0.5f,   1f},  //green(��)
            /*yel*/new float[]{    1f,   1f,   1f,   1f,   1f},  //yellow(��)
        };

    //�U���̍ہA��̑��ΐ}���Q�Ƃ��Ēl��Ԃ�
    public static float GetEffectivenss(BattleCharaType attackType, BattleCharaType defenseType)
    {
        //�����U��or�h��^�C�v�������̏ꍇ�A���{�Ƃ���B
        if (attackType == BattleCharaType.None || defenseType == BattleCharaType.None)
        {
            return 1f;
        }
        //�z��̍s�Ɨ��I�����Ē��̒l���Q�Ƃ���B
        int row = (int)attackType-1;  //(row���s�ԍ�)�@none�̕����܂܂Ȃ�����-1���Ă���
        int col = (int)defenseType-1; //(col����ԍ�)�@none�̕����܂܂Ȃ�����-1���Ă���
        return chart[row][col];
    }
}