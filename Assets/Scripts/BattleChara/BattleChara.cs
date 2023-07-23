using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===戦闘するキャラを管理するスクリプト===//

//レベルに応じたステータスの違うモンスターを生成するクラス
//注意：データのみ扱う＝純粋C#のクラス
[System.Serializable]
public class BattleChara
{
    //データの初期化が行われていない


    //インスペクターからデータを設定できるようにする
    [SerializeField] BattleCharaBase _base;
    [SerializeField] int level;

    //ベースとなるデータ
    public BattleCharaBase Base { get => _base; }
    public int Level { get => level; }

    public int HP { get; set; }

    //使える技
    public List<Move> Moves { get; set; }   //Move.csから参照

    //コンストラクター：生成時の初期設定 => Init関数に変更
    public void Init()
    {
        HP = MaxHP;     //pBase.MaxHP

        Moves = new List<Move>(); //技の初期化

        //使える技の設定：覚える技のレベル以上なら、Movesに追加
        foreach (LearnableMove learnableMove in Base.LearnableMoves)
        {
            if (Level >= learnableMove.Level)               //一定以上のレベルになった場合
            {
                //技を覚える
                Moves.Add(new Move(learnableMove.Base));    //learnableMove.Baseを参照して技を追加する
            }

            //4つ以上の技は使えない
            if (Moves.Count >= 4)
            {
                break;
            }
        }

    }

    //levelに応じたステータスを返すもの：プロパティ（+処理を加えることができる）
    public int MaxHP
    {
        get { return Mathf.FloorToInt((Base.MaxHP * Level) / 100f) + 5; }   //キャラベースの攻撃力を参照し、レベル分をかけた値を100で割って5を足す
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

    //戦闘でダメージを受けたとき(ダメージ関数)
    public DamageDetails TakeDamage(Move move, BattleChara attacker)
    {
        //クリティカル
        //float critical = 1f;                //通常時は等倍

        //if(Random.value * 100 <= 6.25f)     //6.25%の確率でクリティカル発生（２倍）
        //{
        //    critical = 2f;
        //}

        // 相性倍率用の変数作成 (攻撃側(move)の技のタイプと防御側のタイプ１と２を参照）
        float type = TypeChart.GetEffectivenss(move.Base.Type, Base.Type1) * TypeChart.GetEffectivenss(move.Base.Type, Base.Type2);
        DamageDetails damageDetails = new DamageDetails
        {
            Fainted = false,
            //Critical = critical,
            TypeEffectiveness = type
        };


        //ステータスを参照して計算された値に乱数＆タイプ相性＆クリティカルの倍率をかけるための変数
        float modifiers = Random.Range(0.85f, 1f) * type;   // * critical クリティカルは無効

        //計算式本体
        float a = (2 * attacker.Level + 10) / 250f;                                       //レベル補正
        float d = (a * move.Base.Power * ((float)attacker.Attack / Defense) + 2);         //a×技の威力×(攻撃側の攻撃力/防御側の防御力)＋２
        int damage = Mathf.FloorToInt(d * modifiers);                                     //ダメージ計算　小数点以下を切り捨て

        //HPへダメージの反映
        HP -= damage;                                                                     //HPからダメージ分を引く

        if (HP <= 0)                                                                      //HPが0以下になったとき
        {
            HP = 0;                                                                       //HPを0にする。
            damageDetails.Fainted = true;
        }

        return damageDetails;
    }

    //20×90[ほのおパンチ]×(65[redの攻撃力]/75[blueの防御力])/50 =31.2
    //110[blueの体力]-31=79

    //本来の式
    //float b = (20f * move.Base.Power * ((float)attacker.Attack / Defense) / 50f);     //20×技の威力×攻撃側の攻撃力/防御側の防御力/50
    //int damage = Mathf.FloorToInt(b * modifiers);

    ////戦闘でダメージを受けたとき(参考元)
    //public bool TakeDamege(Move move, BattleChara attacker)
    //{
    //    float modifiers = Random.Range(0.85f, 1f);                                  //乱数(85%～100%)の用意
    //    float a = (2 * attacker.Level + 10) / 250f;                                 //レベル補正
    //    float d = a * move.Base.Power * ((float)attacker.Attack / Defense) + 2;     //a×技の威力×(攻撃側の攻撃力/防御側の防御力)＋２
    //    int damege = Mathf.FloorToInt(d * modifiers);                               //ダメージ計算　小数点以下を切り捨て
    //    HP -= damage;                                                               //HPからダメージ分を引く

    //    if (HP <= 0)                                                                //HPが0以下になったとき
    //    {
    //        HP = 0;                                                                 //HPを0にする。
    //        return true;
    //    }

    //    retuen false;
    //}

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);   //Movesからランダムで選ぶ
        return Moves[r];
    }

}

//戦闘不能・クリティカル発生・相性発生の判別
public class DamageDetails
{
     public bool Fainted { get; set; }               //戦闘不能かを判定
     //public float Critical { get; set; }             //クリティカルかを判定
     public float TypeEffectiveness { get; set; }    //相性発生かを判定
}
