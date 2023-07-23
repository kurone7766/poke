using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

//===キャラが使用する技の情報の元となるデータを管理するスクリプト===//

[CreateAssetMenu]
public class MoveBase : ScriptableObject
{
    //技のマスターデータ

    //名前、詳細、タイプ、威力、正確性、PP(技を使うときに消費するポイント)

    [SerializeField] new string name;           //技の名前

    [TextArea]                                  //複数行説明が入力できる
    [SerializeField] string description;        //技の詳細欄

    [SerializeField] BattleCharaType type;      //技のタイプ設定
    [SerializeField] int power;                 //威力
    [SerializeField] int accuracy;              //命中率
    [SerializeField] int pp;                    //使用回数

    //技カテゴリーの読み出し
    [SerializeField] MoveCategory category;

    //ターゲットの読み出し
    [SerializeField] MoveTarget target;

    //ステータス変化のリスト：どのステータスをどの程度変化させるのかリスト
    [SerializeField] MoveEffects effects;

    //他のファイル(Move.cs)から参照するためのプロパティ
    public string Name { get => name; }
    public string Description { get => description; }
    public BattleCharaType Type { get => type; }
    public int Power { get => power; }
    public int Accuracy { get => accuracy; }
    public int PP { get => pp; }

    public MoveCategory Category { get => category; set => category = value; }
    public MoveTarget Target { get => target; }

    ////特殊技
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



//技カテゴリーの枠を列挙
public enum MoveCategory
{
    Phyical,            //物理技
    Specual,            //特殊技(未実装) 
    Stat,               //能力変化技
}

//技のターゲットの枠を列挙
public enum MoveTarget
{
    Foe,                //相手
    Self,               //自分
}

//ステータスの変化リスト元
[System.Serializable]                                   //Unityの枠登録
public class MoveEffects
{
    [SerializeField] List<StatBoost> boosts;

    public List<StatBoost> Boosts { get => boosts; }
}

//どのステータスをどの程度変化させるのか               //Unityの枠登録
[System.Serializable]
public class StatBoost
{
    public Stat stat;
    public int boost;
}