using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===ゲームの状態を管理するスクリプト===//

public enum GameState   //列挙型でゲーム状況を把握
{
    FreeRoam,   //マップ移動(未使用)    ←合わせのときに名前変更させる
    //Title,    //タイトル
    //Select,   //キャラクター選択  （合わせで変更）
    Battle,     //対戦中

}

public class GameController : MonoBehaviour
{
    //ゲームの状態を管理
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;

    [SerializeField] Camera worldCamera;  //フィールド画面

    GameState state = GameState.FreeRoam; //初期状態をFreeRoamへ

    private void Start()
    {
        playerController.OnEncounted += StartBattle;    //外部で読みこめる関数として登録する
        battleSystem.BattleOver += EndBattle;           //外部で読みこめる関数として登録する
    }


    //戦闘開始
    public void StartBattle()
    {
        state = GameState.Battle;                   //ゲーム状況を対戦中へ
        battleSystem.gameObject.SetActive(true);    //戦闘画面を表示
        worldCamera.gameObject.SetActive(false);    //フィールド画面を非表示
        
        //プレイヤーが選択したキャラとエネミーに選択されたキャラを取得
        BattleCharaParty playerParty = playerController.GetComponent<BattleCharaParty>();
        //FindObjectOfTypeでシーンの中から一致するコンポーネントを１つ取得する(GridのEnemyCharaSelect)
        BattleChara enemybattleChara = FindObjectOfType<EnemyCharaSelect>().GetRandomChoiceBattleChara();
        
        battleSystem.StartBattle(playerParty, enemybattleChara);                 //戦闘システムの読み込み開始（リセット）
    }

    //戦闘終了
    public void EndBattle()
    {
        state = GameState.FreeRoam;                 //ゲーム状況をマップ移動へ
        battleSystem.gameObject.SetActive(false);   //戦闘画面を非表示
        worldCamera.gameObject.SetActive(true);    //フィールド画面を表示
    }

    void Update()
    {
        if (state == GameState.FreeRoam)        　//マップ移動(未使用)のとき
        {
            playerController.HandleUpdate();
        }
        //else if (state == GameState.Title)      //タイトル(未使用)のとき 
        //{
        //    Title
        //}
        //else if (state == GameState.Select)     //キャラ選択(未使用)のとき
        //{
        //    Select
        //}
        else if (state == GameState.Battle)       //戦闘中のとき
        {
            battleSystem.HandleUpdate();
        }
    }
}
