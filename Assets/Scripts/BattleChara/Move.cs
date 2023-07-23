using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===キャラが使用する技の情報を管理するスクリプト===//

public class Move
{
    //BattleCharaが実際に使うときの技データ

    //技のマスターデータを持つ
    //使いやすいようにするためにPPももつ

    //BattleChara.csが参照するため、public(公開)にしておく
    public MoveBase Base { get; set; }  //Movebase.csから値を参照
    public int PP { get; set; }

    //初期設定
    public Move(MoveBase pBase)
    {
        Base = pBase;
        PP = pBase.PP;
    }

}