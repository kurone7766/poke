using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;                               //OnEncouuntedの読み込みに使用

//===マップ移動のプレイヤーを管理するスクリプト===//

//===メモ===//
//プレイヤーの移動を作成
//・キー入力による1マス移動を作成
//・移動の判定（移動しているかしていないか）を作成

//壁の作成
//・Tilemapを作成
//・壁にコライダーをつける
//・PlayerからRayを飛ばして、壁判定をする

//草むらでランダムエンカウントをする
//・草むらをつくる
//・草むらにコライダーをつける
//・草むらを判定をつける

//モンスターのデータ管理(BattleCharaBase)
//・モンスターの多様化:ScriptableObjectを使う
//・レベルに応じたステータスの多様化:それ用のクラスを使う


public class PlayerController : MonoBehaviour
{
    //Playerの１マス移動

    [SerializeField] float moveSpeed;                      //移動速度

    bool isMoving;                                         //移動しているかの判定
    Vector2 input;

    Animator animator;                                     //変数の宣言
    
    //壁判定のLayer
    [SerializeField] LayerMask solidObjectsLayer;
    
    //草むら判定のLayer
    [SerializeField] LayerMask longGrassLayer;

    //相互依存を解消；UnityAction(関数を登録する)
    public UnityAction OnEncounted;                        //外部(GameController)のOnEncountedを直接読み込み     
    //[SerializeField] GameController gameController;      //GameControllerで読み込ませる(相互依存解消で↑に変更)

    private void Awake()                                   //unity側のanimatorを取得する。（読み込み）
    {
        animator = GetComponent<Animator>();
    }
    public void HandleUpdate()
    {
        if (!isMoving)                                     //移動中は入力を受け付けない
        {
            //キーボードの入力方向に動く
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            //斜め移動の無効化
            if (input.x != 0)                               //横方向の入力がされている場合
            {
                input.y = 0;                                //縦方向の入力を受け付けない
            }
            
            //入力があった場合
            if (input != Vector2.zero)
            {
                //向きを変えたい
                animator.SetFloat("moveX", input.x);        //unityのanimetorをセットする（横方向）
                animator.SetFloat("moveY", input.y);        //unityのanimetorをセットする（縦方向）
                Vector2 targetPos = transform.position;     //キーボードで入力した分を目的地として、
                targetPos += input;
                if(IsWalkable(targetPos))                   //移動可能かを調べる関数がtrueの場合
                {
                    StartCoroutine(Move(targetPos));        //コルーチンにより目的地まで移動させる。
                }
            }
        }
        animator.SetBool("isMoving", isMoving);
    }
    //コルーチンを使って徐々に目的地に近づける
    IEnumerator Move(Vector3 targetPos)
    {
        //移動中は入力を受け付けたくない
        isMoving = true;                                                        //移動中の判定にする。

        //targetPosとの左があるなら繰り返す
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)   //距離に応じて{}内の命令を復唱させる
        {
            //targetPosに近づける
            transform.position = Vector3.MoveTowards(
                transform.position,                                              //現在の場所
                targetPos,                                                       //目的地
                moveSpeed*Time.deltaTime                                         //近づける際の速度
                );
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;                                                        //移動していない判定にする(移動処理の完了時)
        CheckForEncounters();                                                    //移動処理が終わった後にエンカウントの確認   
    }

    //targetPosに移動可能かを調べる関数　=可能でtrue不可能でfalseを返す
    bool IsWalkable(Vector2 targetPos)
    {
        //壁(solidObjectsLayer)に当たっているかの判定
        return Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) == false; //targetPosに半径0.2fでできた円形のRayを作成し、    
                                                                                     //そのRayがsolidObjectsLayerにぶつかったらfalseを返す（プレイやーのy座標を+0.8すると判定位置が丁度になる）
    }

    //プレイヤーの座標から円形のRayを飛ばして、草むらLayerに当たったらランダムエンカウント
    void CheckForEncounters()
    {
        if(Physics2D.OverlapCircle(transform.position, 0.2f, longGrassLayer))
        {
            //ランダムエンカウント
            if (Random.Range(0, 100) < 10)
            {
                //Random..Range(0,100);0～99までのどれかの数字が出る
                //10より小さい強いずは0～9までの10個＝10％
                //10以上の数字は10～99までの90個＝90％
                Debug.Log("モンスターに遭遇");

                OnEncounted();                                          //GameControllerにある関数を直接読み込み
                //gameController.StartBattle();                         //BattleSystemを読み込み(戦闘へ移行)
                animator.SetBool("isMoving", false);                    //戦闘画面に切り替わるとプレイヤーの動きを停止
            }
        }
    }
}
