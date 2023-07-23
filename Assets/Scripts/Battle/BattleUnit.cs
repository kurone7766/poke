using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//===戦闘時のキャラ情報取得＆キャラ画像表示を行うスクリプト===//

public class BattleUnit : MonoBehaviour
{
    //変更：戦わせるキャラをBattleSystemからセットする。
    //[SerializeField] BattleCharaBase _base;   //BattleSystem.csでセットするため不要
    //[SerializeField] int level;               //BattleSystem.csでセットするため不要
    [SerializeField] bool isPlayerUnit;

    public BattleChara BattleChara { get; set; }

    Vector3 originalPos;    //初期位置の座標(登場ｱﾆﾒｰｼｮﾝ用)
    Color originalColor;    //初期カラー(被弾時のｱﾆﾒｰｼｮﾝ用)
    Image image;

    //バトルで使うキャラを保持
    //キャラの画像を反映する。

    //unityで設置した画像の高さを保持
    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = transform.localPosition;
        originalColor = image.color;
    }

    //対応したキャラの画像を読み込む
    public void Setup(BattleChara battleChara)
    {
        //_baseからレベルに応じたモンスターを生成する。
        //BattleSystemで使うためにプロパティに入れる。
        BattleChara = battleChara; //BattkeChara.csから参照　new BattleChara(_base, level);

        if (isPlayerUnit)
        {
            image.sprite = BattleChara.Base.LeftSprite;
        }
        else
        {
            image.sprite = BattleChara.Base.RightSprite;
        }
        image.color = originalColor;                        //戦闘毎にキャラ画像の色を戻す
        PlayerEnterAnimetion();
    }

    //キャラクター登場ｱﾆﾒｰｼｮﾝ
    public void PlayerEnterAnimetion()
    {
        if(isPlayerUnit)
        {
            //左端に配置
            transform.localPosition = new Vector3(-1000,originalPos.y);
        }
        else
        {
            //右端に配置
            transform.localPosition = new Vector3(1000, originalPos.y);
        }
        //戦闘時の位置までアニメーション
        transform.DOLocalMoveX(originalPos.x, 1f);  //1秒をかけて配置場所まで動く

    }

    //キャラクター攻撃ｱﾆﾒｰｼｮﾝ
    public void PlayerAttackAnimetion()
    {
        //シーケンス
        //右に動いた後、元の位置に戻る
        Sequence sequence = DOTween.Sequence();                                   //DOTweenの使用を宣言
        if (isPlayerUnit)   //プレイヤー側
        {
            sequence.Append(transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));  //DOTweenにより0.25秒でx座標を＋50する
        }
        else　              //エネミー側
        {
            sequence.Append(transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));  //DOTweenにより0.25秒でx座標を-50する
        }
        sequence.Append(transform.DOLocalMoveX(originalPos.x, 0.2f));             //DOTweenにより0.2秒でx座標を初期値にする
    }

    //キャラクター被弾ｱﾆﾒｰｼｮﾝ
    public void PlayerHitAnimetion()
    {
        //（攻撃を受けた時に）キャラの色を一度GLAYにしてから戻す
        Sequence sequence = DOTween.Sequence();                                   //DOTweenの使用を宣言
        sequence.Append(image.DOColor(Color.gray, 0.1f));                         //DOTweenによりキャラを0.1秒灰色にする
        sequence.Append(image.DOColor(originalColor, 0.1f));                      //DOTweenによりキャラを0.1秒で通常色に戻す
    }

    //キャラクター戦闘不能ｱﾆﾒｰｼｮﾝ
    public void PlayerFaintAnimation()
    {
        //画面端に下がりながら薄くなる
        Sequence sequence = DOTween.Sequence();                                   //DOTweenの使用を宣言
        if (isPlayerUnit)   //プレイヤー側
        {
            sequence.Append(transform.DOLocalMoveX(originalPos.x - 500, 1.5f));   //DOTweenにより1.5秒でx座標を-500する
            sequence.Join(image.DOColor(Color.white, 0.25f));                         //DOTweenによりキャラを0.1秒白色にする
            sequence.Join(image.DOFade(0, 1f));                                       //DOTweenにより上のAppendを適用しながらキャラを0.5秒で0の薄さにする。

            sequence.Append(transform.DOLocalMoveX(originalPos.x - 500, 0.5f));   //DOTweenにより0.5秒でx座標を-500する
        }
        else　              //エネミー側
        {
            sequence.Append(transform.DOLocalMoveX(originalPos.x + 500, 1.5f));   //DOTweenにより1.5秒でx座標を+1000する
            sequence.Join(image.DOColor(Color.white, 0.25f));                         //DOTweenによりキャラを0.1秒白色にする
            sequence.Join(image.DOFade(0, 1f));                                       //DOTweenにより上のAppendを適用しながらキャラを0.5秒で0の薄さにする。

            sequence.Append(transform.DOLocalMoveX(originalPos.x + 500, 0.5f));   //DOTweenにより0.5秒でx座標を+1000する

        }
    }



}
