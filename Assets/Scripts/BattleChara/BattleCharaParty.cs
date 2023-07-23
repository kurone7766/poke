using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class BattleCharaParty : MonoBehaviour
{
    //�v���C���[�̑I�������L�������Ǘ�����
    [SerializeField] List<BattleChara> battleCharas;

    //�Q�[���J�n���ɃL�����󋵂�������
    private void Start()
    {
        foreach (BattleChara battleChara in battleCharas)
        {
            battleChara.Init();
        }

    }

    //�킦��L������n��(HP>0�̃L������Ԃ�)
    public BattleChara GetHealthyBattleChara()
    {
        //�ŏ��Ɍ�����HP��0�ȏ�̃L������T���ĕԂ�
        return battleCharas.Where(chara => chara.HP > 0).FirstOrDefault();
    }

}
