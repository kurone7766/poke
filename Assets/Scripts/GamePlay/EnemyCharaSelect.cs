using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharaSelect : MonoBehaviour
{
    //エネミーのキャラを管理する
    [SerializeField] List<BattleChara> battleCharas;

    //ランダムで選択する
    public BattleChara GetRandomChoiceBattleChara()
    {
        int r = Random.Range(0, battleCharas.Count);
        BattleChara battleChara = battleCharas[r];

        //戦闘が始まる度に初期化を行う
        battleChara.Init(); 
        return battleCharas[r];
    }

}
