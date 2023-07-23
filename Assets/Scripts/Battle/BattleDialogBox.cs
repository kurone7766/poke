using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] Color highlightColor;

    //役割：dialogのTextを取得して、変更する。
    [SerializeField] int letterPerSecond; //１文字当たりにかける時間
    [SerializeField] Text dialogText;

    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;

    [SerializeField] List<Text> actionTexts;
    [SerializeField] List<Text> moveTexts;

    [SerializeField] Text ppText;
    [SerializeField] Text typeText;


    //Textを変更するための関数
    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    //タイプ形式で文字を表示する。
    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";                   //初期化
        foreach (char letter in dialog) 
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f/letterPerSecond);    //文字の表示速度　１秒で何文字
        }

        yield return new WaitForSeconds(0.7f);                      //相性が発生したときに0.7秒間を空ける
    }

    //UIの表示/非表示
    //dialogTextの表示管理
    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }
    //actionSelectorの表示管理
    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }
    //moveSelectorとmoveDetailsの表示管理
    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    //プレイヤー行動選択
    //選択中の選択肢の色を変える。
    public void UpdateActionSelection(int selectAction)
    {
        //selectActionが0の時はactionTexts[0]の色を青にする。それ以外は黒
        //selectActionが1の時はactionTexts[1]の色を青にする。それ以外は黒
        for (int i = 0; i < actionTexts.Count; i++) 
        {
            if (selectAction == i)
            {
                actionTexts[i].color = highlightColor;
            }
            else
            {
                actionTexts[i].color = Color.black;
            }
        }

    }

    //プレイヤー技選択
    //選択中の選択肢の色を変える。
    public void UpdateMoveSelection(int selectMove, Move move)
    {
        //selectmoveが0の時はmoveTexts[0]の色を青にする。それ以外は黒
        //selectmoveが1の時はmoveTexts[1]の色を青にする。それ以外は黒
        //selectmoveが2の時はmoveTexts[2]の色を青にする。それ以外は黒
        //selectmoveが3の時はmoveTexts[3]の色を青にする。それ以外は黒
        for (int i = 0; i < moveTexts.Count; i++)
        {
            if (selectMove == i)
            {
                moveTexts[i].color = highlightColor;
            }
            else
            {
                moveTexts[i].color = Color.black;
            }
        }
        ppText.text = $"PP{move.PP}/{move.Base.PP}";
        typeText.text = move.Base.Type.ToString();
    }

    //・技名の反映
    public void SetMoveNames(List<Move> moves)
        {
        //for分でまとめてる
        // moveTexts[0].text = moves[0].Base.Name;
        // moveTexts[1].text = moves[1].Base.Name;
        // moveTexts[2].text = moves[2].Base.Name;
        // moveTexts[3].text = moves[3].Base.Name;

        for (int i = 0; i < moveTexts.Count; i++)
        {
            //覚えている数だけ反映
            if (i < moves.Count)
            {
                moveTexts[i].text = moves[i].Base.Name; //覚えている技名を表示
            }
            else
            {
                moveTexts[i].text = "-";                //無し-
            }
        }
    }
    

}
