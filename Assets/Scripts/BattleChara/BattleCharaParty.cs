using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class BattleCharaParty : MonoBehaviour
{
    //プレイヤーの選択したキャラを管理する
    [SerializeField] List<BattleChara> battleCharas;

    //ゲーム開始時にキャラ状況を初期化
    private void Start()
    {
        foreach (BattleChara battleChara in battleCharas)
        {
            battleChara.Init();
        }

    }

    //戦えるキャラを渡す(HP>0のキャラを返す)
    public BattleChara GetHealthyBattleChara()
    {
        //最初に見つけたHPが0以上のキャラを探して返す
        return battleCharas.Where(chara => chara.HP > 0).FirstOrDefault();
    }

}
