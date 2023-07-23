using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;           //BattleOverで直接読み込む用

//===戦闘時の各プレイヤーの行動を管理するスクリプト===//
//戦闘フェーズを列挙
public enum BattleState
{
    Start,          //戦闘開始フェーズ
    PlayerAction,   //行動選択フェーズ
    //ActuonSelection
    PlayerMove,     //技選択フェーズ
    EnemyMove,
    //MoveSelection
    //PerformScreen //技実行フェーズ
    Busy,           //処理中
    BattleOver,     //戦闘終了フェーズ
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;         //ｽｸﾘﾌﾟﾄ[BattleUnit]の中にある[playerUnit]関数を読み出し
    [SerializeField] BattleUnit enemyUnit;          //unity側でスクリプトをセットしなければならない
    [SerializeField] BattleHUD  playerHUD;          //読み出し
    [SerializeField] BattleHUD  enemyHUD;           //読み出し
    [SerializeField] BattleDialogBox dialogBox;     //読み出し

    public UnityAction BattleOver;                  //GameControllerから直接読み込み
    //[SerializeField] GameController gameController; //読み出し

    //BattleStateを変数stateに置き換え
    BattleState state;    

    //プレイヤーの行動選択
    int currentAction;      //0=fightたたかう　1=run逃げる
    //プレイヤーの技選択
    int currentMove;        //0=左上、1=右上、2＝左下、3=右下

    //セットするキャラの変数
    BattleCharaParty playerParty;
    BattleChara enemybattleChara;
    
    //戦闘システムの読み込み開始(セットするキャラの変数の参照も行う）
    public void StartBattle(BattleCharaParty playerParty, BattleChara enemybattleChara)                    
    {
        this.playerParty = playerParty;
        this.enemybattleChara = enemybattleChara;
        StartCoroutine(SetupBattle());

    }

    IEnumerator SetupBattle()
    {
        state = BattleState.Start;

        //キャラの生成と描画
        playerUnit.Setup(playerParty.GetHealthyBattleChara()); //プレイヤーの選択したキャラをセット
        enemyUnit.Setup(enemybattleChara);  //エネミーに選ばれたキャラをセット

        //HUDの描画
        playerHUD.SetData(playerUnit.BattleChara);
        enemyHUD.SetData(enemyUnit.BattleChara);

        //プレイヤーの技を描画
        dialogBox.SetMoveNames(playerUnit.BattleChara.Moves);

        //・ﾒｯｾｰｼﾞが出て、１秒後にActionSelectorを表示する。
        yield return dialogBox.TypeDialog($"対戦相手の {enemyUnit.BattleChara.Base.Name} が現れた！");    //テキスト表示
        PlayerAction();
    }

    //行動選択フェーズ
    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        dialogBox.EnableActionSelector(true);
        StartCoroutine (dialogBox.TypeDialog("何をする？"));     //テキスト表示
    }

    //技選択フェーズ
    void PlayerMove()   //Zを押したときの処理
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableDialogText(false);          //DialogTextを非表示にする
        dialogBox.EnableActionSelector(false);      //ActionSelectorを非表示にする
        dialogBox.EnableMoveSelector(true);         //MoveSelectorを表示させる
    }

    //PlayerMoveの実行(プレイヤーのターン)
    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;

        //技を決定
        Move move = playerUnit.BattleChara.Moves[currentMove];                                           //決定された技の読み込み
        move.PP--;                                                                                       //PPの消費
        yield return dialogBox.TypeDialog($"{playerUnit.BattleChara.Base.Name}の\n{move.Base.Name}");    //テキスト表示

        //ｱﾆﾒｰｼｮﾝ
        playerUnit.PlayerAttackAnimetion();         //攻撃ｱﾆﾒｰｼｮﾝ
        yield return new WaitForSeconds(0.7f);      //ｱﾆﾒｰｼｮﾝ待機時間
        
        enemyUnit.PlayerHitAnimetion();             //被弾時のｱﾆﾒｰｼｮﾝ


        //playerダメージ計算
        DamageDetails damageDetails = enemyUnit.BattleChara.TakeDamage(move, playerUnit.BattleChara);

        //HPbarに反映
        yield return enemyHUD.UpdateHP();                //エネミーの体力ゲージに反映させる

        //相性/クリティカルのメッセージ
        yield return ShowDamegeDetails(damageDetails);   //相性発生/クリティカル発生時のメッセージ表示

        //戦闘不能ならメッセージ
        if (damageDetails.Fainted) 
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.BattleChara.Base.Name}は倒れた！");    //テキスト表示
            enemyUnit.PlayerFaintAnimation();                                                     //戦闘不能ｱﾆﾒｰｼｮﾝ
            yield return new WaitForSeconds(0.7f);                                                //テキスト表示させた画面で0.7秒待つ

            //戦闘終了
            BattleOver();                       //GameControllerのEndBattleを直接読み込み
            //gameController.EndBattle();
        }
        //戦闘可能ならEnemyMove
        else
        {

            StartCoroutine(EnemyMove());
        }

    }

    //EnemyMoveの実行(エネミーのターン)
    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;

        //技を決定(ランダム)
        Move move = enemyUnit.BattleChara.GetRandomMove();                                              //決定された技の読み込み
        move.PP--;                                                                                      //PPの消費
        yield return dialogBox.TypeDialog($"{enemyUnit.BattleChara.Base.Name}の\n{move.Base.Name}");    //テキスト表示
        
        //攻撃時のｱﾆﾒｰｼｮﾝ
        enemyUnit.PlayerAttackAnimetion();          //攻撃ｱﾆﾒｰｼｮﾝ                                               
        yield return new WaitForSeconds(0.7f);      //ｱﾆﾒｰｼｮﾝ待機時間

        playerUnit.PlayerHitAnimetion();            //被弾時のｱﾆﾒｰｼｮﾝ

        //enemyダメージ計算
        DamageDetails damageDetails = playerUnit.BattleChara.TakeDamage(move, enemyUnit.BattleChara);

        //HPbarに反映
        yield return playerHUD.UpdateHP();

        //相性/クリティカルのメッセージ
        yield return ShowDamegeDetails(damageDetails);  //相性発生/クリティカル発生時のメッセージ表示

        //戦闘不能ならメッセージ
        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.BattleChara.Base.Name}は倒れた！");    //テキスト表示
            playerUnit.PlayerFaintAnimation();                                                     //戦闘不能ｱﾆﾒｰｼｮﾝ
            yield return new WaitForSeconds(0.7f);                                              　 //テキスト表示させた画面で0.7秒待つ

            //戦闘終了
            BattleOver();                       //GameControllerのEndBattleを直接読み込み

        }
        //戦闘可能ならEnemyMove
        else
        {
            PlayerAction();
        }

    }

    //相性発生/クリティカル発生時に表示するメッセージの判別
    IEnumerator ShowDamegeDetails(DamageDetails damageDetails)
    {
        if (damageDetails.TypeEffectiveness > 1f)
        {
            yield return dialogBox.TypeDialog($"効果は抜群だ！");      //テキスト表示（相性＝良）
        }
        if (damageDetails.TypeEffectiveness < 1f)
        {
            yield return dialogBox.TypeDialog($"効果はいまひとつ…");        //テキスト表示（相性＝悪）
        }
        //if (damageDetails.Critical > 1f)
        //{
        //yield return dialogBox.TypeDialog($"急所にあたった！");          //テキスト表示（クリティカル発生）
        //}

    }

    public void HandleUpdate()
    {
        if (state == BattleState.PlayerAction)      //行動選択フェーズ移行時のコード読み出し
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove)   //技選択フェーズ移行時のコード読み出し
        {
            HandleMoveSelection();
        }
    }

    //行動選択フェーズ移行時のコード内容
    void HandleActionSelection()
    {
        //・Zボタンを押すと、MoveSelectorとMoveDetailsを表示する。
        //↓キーを入力するとRun(1)、↑キーを押すとfight(0)
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentAction < 1)
            {
                currentAction++;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentAction > 0)
            {
                currentAction--;
            }
        }

        //色をつけてどちらをせんたくしているのかわかるようにする。
        //・ActionSelectorでどちらを選択しているのかをわかりやすくする。
        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)                         //fightを選択したとき
            {
                PlayerMove();
            }
        }
    }

    //技選択フェーズ移行時のコード内容
    void HandleMoveSelection()
    {
        //・Zボタンを押すと、MoveSelector(技の選択肢)とMoveDetails(PPとタイプ)を表示する。
        if (Input.GetKeyDown(KeyCode.RightArrow))       //右矢印キーの入力時
        {
            if (currentMove < playerUnit.BattleChara.Moves.Count - 1)
            {
                currentMove++;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))        //左矢印キーの入力時
        {
            if (currentMove > 0)
            {
                currentMove--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))   //下矢印キーの入力時
        {
            if (currentMove < playerUnit.BattleChara.Moves.Count - 2)
            {
                currentMove += 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))     //上矢印キーの入力時
        {
            if (currentMove > 1)
            {
                currentMove -= 2;
            }
        }
        dialogBox.UpdateMoveSelection(currentMove, playerUnit.BattleChara.Moves[currentMove]);

        //・Zボタンを押すと技を決定
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //・技選択のUIを非表示
            dialogBox.EnableMoveSelector(false);

            //・ダイアログを表示
            dialogBox.EnableDialogText(true);

            //・技決定の処理
            StartCoroutine(PerformPlayerMove());


        }
    }
}
