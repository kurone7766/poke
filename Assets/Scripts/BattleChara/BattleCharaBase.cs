using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===戦闘するキャラの情報の元となるデータを管理するスクリプト===//

//バトルキャラのマスターデータ：外部から変更しない（インスペクターだけ変更可能）
[CreateAssetMenu]
public class BattleCharaBase : ScriptableObject
{
    //各種詳細設定(名前、説明、画像、タイプ、ステータス)
    //名前
    [SerializeField] new string name;       //キャラの名前
    [SerializeField] string hudname;        //キャラの名前(HUD上の名前)
    [TextArea]
    [SerializeField] string description;    //キャラの説明

    //画像
    [SerializeField] Sprite leftSprite;     //キャラ像(プレイヤー側)
    [SerializeField] Sprite rightSprite;    //キャラ像(エネミー側)

    //タイプ
    [SerializeField] BattleCharaType type1;
    [SerializeField] BattleCharaType type2;

    //ステータス [HP,ATK,DEF,SPE]　           //public intにすると外部から変更できてしまう。
    [SerializeField] int maxHP;
    [SerializeField] int attack;    
    [SerializeField] int defense;
    [SerializeField] int speed;

    //覚える技一覧
    [SerializeField] List<LearnableMove> learnableMoves;

    //プロパティ設定                           //他ファイルからattackの値は取得できるが変更はできない。
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

//覚える技クラス；どのレベルで何を覚えるのか
[Serializable]
public class LearnableMove
{
    //ヒエラルキーで設定する
    [SerializeField] MoveBase _base;
    [SerializeField] int level;

    public MoveBase Base { get => _base; }
    public int Level { get => level; }
}

//バトルキャラのタイプ設定
public enum BattleCharaType
{
    None,                   //設定無し
    Normal,                 //無属性
    Red,                    //赤属性
    Blue,                   //青属性
    Green,                  //緑属性
    Yellow,                 //黄属性
}

//戦闘中の能力変化技を使用するとき用
public enum Stat
{
    Attack,
    Defense,
    Speed,
}


//わざの相性計算   (どこからでも参照可能）→BattleCharaのTakeDamegeで反映
public class TypeChart
{
    //タイプの相対図（参照値）
    static float[][] chart =
    {
            //攻撃＼防御（左上から右下の斜線がひかれた相対図をイメージ）
            //                     nor   red   blu   gre   yel   
            /*nor*/new float[]{    1f,   1f,   1f,   1f,   1f},  //normal(ノーマル)
            /*red*/new float[]{    1f, 0.5f, 0.5f,   2f,   1f},  //red(赤)
            /*blu*/new float[]{    1f,   2f, 0.5f, 0.5f,   1f},  //blue(青)
            /*gre*/new float[]{    1f, 0.5f,   2f, 0.5f,   1f},  //green(緑)
            /*yel*/new float[]{    1f,   1f,   1f,   1f,   1f},  //yellow(黄)
        };

    //攻撃の際、上の相対図を参照して値を返す
    public static float GetEffectivenss(BattleCharaType attackType, BattleCharaType defenseType)
    {
        //もし攻撃or防御タイプが無しの場合、等倍とする。
        if (attackType == BattleCharaType.None || defenseType == BattleCharaType.None)
        {
            return 1f;
        }
        //配列の行と列を選択して中の値を参照する。
        int row = (int)attackType-1;  //(row＝行番号)　noneの分を含まないため-1しておく
        int col = (int)defenseType-1; //(col＝列番号)　noneの分を含まないため-1しておく
        return chart[row][col];
    }
}