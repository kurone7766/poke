using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===�퓬����L�������Ǘ�����X�N���v�g===//

//���x���ɉ������X�e�[�^�X�̈Ⴄ�����X�^�[�𐶐�����N���X
//���ӁF�f�[�^�݈̂���������C#�̃N���X
[System.Serializable]
public class BattleChara
{
    //�f�[�^�̏��������s���Ă��Ȃ�


    //�C���X�y�N�^�[����f�[�^��ݒ�ł���悤�ɂ���
    [SerializeField] BattleCharaBase _base;
    [SerializeField] int level;

    //�x�[�X�ƂȂ�f�[�^
    public BattleCharaBase Base { get => _base; }
    public int Level { get => level; }

    public int HP { get; set; }

    //�g����Z
    public List<Move> Moves { get; set; }   //Move.cs����Q��

    //�R���X�g���N�^�[�F�������̏����ݒ� => Init�֐��ɕύX
    public void Init()
    {
        HP = MaxHP;     //pBase.MaxHP

        Moves = new List<Move>(); //�Z�̏�����

        //�g����Z�̐ݒ�F�o����Z�̃��x���ȏ�Ȃ�AMoves�ɒǉ�
        foreach (LearnableMove learnableMove in Base.LearnableMoves)
        {
            if (Level >= learnableMove.Level)               //���ȏ�̃��x���ɂȂ����ꍇ
            {
                //�Z���o����
                Moves.Add(new Move(learnableMove.Base));    //learnableMove.Base���Q�Ƃ��ċZ��ǉ�����
            }

            //4�ȏ�̋Z�͎g���Ȃ�
            if (Moves.Count >= 4)
            {
                break;
            }
        }

    }

    //level�ɉ������X�e�[�^�X��Ԃ����́F�v���p�e�B�i+�����������邱�Ƃ��ł���j
    public int MaxHP
    {
        get { return Mathf.FloorToInt((Base.MaxHP * Level) / 100f) + 5; }   //�L�����x�[�X�̍U���͂��Q�Ƃ��A���x�������������l��100�Ŋ�����5�𑫂�
    }
    public int Attack
    {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
    }
    public int Defense
    {
        get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5; }
    }
    public int Speed
    {
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 10; }
    }

    //�퓬�Ń_���[�W���󂯂��Ƃ�(�_���[�W�֐�)
    public DamageDetails TakeDamage(Move move, BattleChara attacker)
    {
        //�N���e�B�J��
        //float critical = 1f;                //�ʏ펞�͓��{

        //if(Random.value * 100 <= 6.25f)     //6.25%�̊m���ŃN���e�B�J�������i�Q�{�j
        //{
        //    critical = 2f;
        //}

        // �����{���p�̕ϐ��쐬 (�U����(move)�̋Z�̃^�C�v�Ɩh�䑤�̃^�C�v�P�ƂQ���Q�Ɓj
        float type = TypeChart.GetEffectivenss(move.Base.Type, Base.Type1) * TypeChart.GetEffectivenss(move.Base.Type, Base.Type2);
        DamageDetails damageDetails = new DamageDetails
        {
            Fainted = false,
            //Critical = critical,
            TypeEffectiveness = type
        };


        //�X�e�[�^�X���Q�Ƃ��Čv�Z���ꂽ�l�ɗ������^�C�v�������N���e�B�J���̔{���������邽�߂̕ϐ�
        float modifiers = Random.Range(0.85f, 1f) * type;   // * critical �N���e�B�J���͖���

        //�v�Z���{��
        float a = (2 * attacker.Level + 10) / 250f;                                       //���x���␳
        float d = (a * move.Base.Power * ((float)attacker.Attack / Defense) + 2);         //a�~�Z�̈З́~(�U�����̍U����/�h�䑤�̖h���)�{�Q
        int damage = Mathf.FloorToInt(d * modifiers);                                     //�_���[�W�v�Z�@�����_�ȉ���؂�̂�

        //HP�փ_���[�W�̔��f
        HP -= damage;                                                                     //HP����_���[�W��������

        if (HP <= 0)                                                                      //HP��0�ȉ��ɂȂ����Ƃ�
        {
            HP = 0;                                                                       //HP��0�ɂ���B
            damageDetails.Fainted = true;
        }

        return damageDetails;
    }

    //20�~90[�ق̂��p���`]�~(65[red�̍U����]/75[blue�̖h���])/50 =31.2
    //110[blue�̗̑�]-31=79

    //�{���̎�
    //float b = (20f * move.Base.Power * ((float)attacker.Attack / Defense) / 50f);     //20�~�Z�̈З́~�U�����̍U����/�h�䑤�̖h���/50
    //int damage = Mathf.FloorToInt(b * modifiers);

    ////�퓬�Ń_���[�W���󂯂��Ƃ�(�Q�l��)
    //public bool TakeDamege(Move move, BattleChara attacker)
    //{
    //    float modifiers = Random.Range(0.85f, 1f);                                  //����(85%�`100%)�̗p��
    //    float a = (2 * attacker.Level + 10) / 250f;                                 //���x���␳
    //    float d = a * move.Base.Power * ((float)attacker.Attack / Defense) + 2;     //a�~�Z�̈З́~(�U�����̍U����/�h�䑤�̖h���)�{�Q
    //    int damege = Mathf.FloorToInt(d * modifiers);                               //�_���[�W�v�Z�@�����_�ȉ���؂�̂�
    //    HP -= damage;                                                               //HP����_���[�W��������

    //    if (HP <= 0)                                                                //HP��0�ȉ��ɂȂ����Ƃ�
    //    {
    //        HP = 0;                                                                 //HP��0�ɂ���B
    //        return true;
    //    }

    //    retuen false;
    //}

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);   //Moves���烉���_���őI��
        return Moves[r];
    }

}

//�퓬�s�\�E�N���e�B�J�������E���������̔���
public class DamageDetails
{
     public bool Fainted { get; set; }               //�퓬�s�\���𔻒�
     //public float Critical { get; set; }             //�N���e�B�J�����𔻒�
     public float TypeEffectiveness { get; set; }    //�����������𔻒�
}
