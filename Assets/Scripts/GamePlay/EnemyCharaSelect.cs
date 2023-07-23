using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharaSelect : MonoBehaviour
{
    //�G�l�~�[�̃L�������Ǘ�����
    [SerializeField] List<BattleChara> battleCharas;

    //�����_���őI������
    public BattleChara GetRandomChoiceBattleChara()
    {
        int r = Random.Range(0, battleCharas.Count);
        BattleChara battleChara = battleCharas[r];

        //�퓬���n�܂�x�ɏ��������s��
        battleChara.Init(); 
        return battleCharas[r];
    }

}
