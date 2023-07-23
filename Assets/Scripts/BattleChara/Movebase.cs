using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

//===�L�������g�p����Z�̏��̌��ƂȂ�f�[�^���Ǘ�����X�N���v�g===//

[CreateAssetMenu]
public class MoveBase : ScriptableObject
{
    //�Z�̃}�X�^�[�f�[�^

    //���O�A�ڍׁA�^�C�v�A�З́A���m���APP(�Z���g���Ƃ��ɏ����|�C���g)

    [SerializeField] new string name;           //�Z�̖��O

    [TextArea]                                  //�����s���������͂ł���
    [SerializeField] string description;        //�Z�̏ڍח�

    [SerializeField] BattleCharaType type;      //�Z�̃^�C�v�ݒ�
    [SerializeField] int power;                 //�З�
    [SerializeField] int accuracy;              //������
    [SerializeField] int pp;                    //�g�p��

    //�Z�J�e�S���[�̓ǂݏo��
    [SerializeField] MoveCategory category;

    //�^�[�Q�b�g�̓ǂݏo��
    [SerializeField] MoveTarget target;

    //�X�e�[�^�X�ω��̃��X�g�F�ǂ̃X�e�[�^�X���ǂ̒��x�ω�������̂����X�g
    [SerializeField] MoveEffects effects;

    //���̃t�@�C��(Move.cs)����Q�Ƃ��邽�߂̃v���p�e�B
    public string Name { get => name; }
    public string Description { get => description; }
    public BattleCharaType Type { get => type; }
    public int Power { get => power; }
    public int Accuracy { get => accuracy; }
    public int PP { get => pp; }

    public MoveCategory Category { get => category; set => category = value; }
    public MoveTarget Target { get => target; }

    ////����Z
    //public bool IsSpecial
    //{
    //    get
    //    {
    //        if (type == BattleCharaType.Red || BattleCharaType.Blue || BattleCharaType.Green || BattleCharaType.Yellow)
    //        {
    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //}

}



//�Z�J�e�S���[�̘g���
public enum MoveCategory
{
    Phyical,            //�����Z
    Specual,            //����Z(������) 
    Stat,               //�\�͕ω��Z
}

//�Z�̃^�[�Q�b�g�̘g���
public enum MoveTarget
{
    Foe,                //����
    Self,               //����
}

//�X�e�[�^�X�̕ω����X�g��
[System.Serializable]                                   //Unity�̘g�o�^
public class MoveEffects
{
    [SerializeField] List<StatBoost> boosts;

    public List<StatBoost> Boosts { get => boosts; }
}

//�ǂ̃X�e�[�^�X���ǂ̒��x�ω�������̂�               //Unity�̘g�o�^
[System.Serializable]
public class StatBoost
{
    public Stat stat;
    public int boost;
}