using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogBox : MonoBehaviour
{
    [SerializeField] Color highlightColor;

    //�����Fdialog��Text���擾���āA�ύX����B
    [SerializeField] int letterPerSecond; //�P����������ɂ����鎞��
    [SerializeField] Text dialogText;

    [SerializeField] GameObject actionSelector;
    [SerializeField] GameObject moveSelector;
    [SerializeField] GameObject moveDetails;

    [SerializeField] List<Text> actionTexts;
    [SerializeField] List<Text> moveTexts;

    [SerializeField] Text ppText;
    [SerializeField] Text typeText;


    //Text��ύX���邽�߂̊֐�
    public void SetDialog(string dialog)
    {
        dialogText.text = dialog;
    }

    //�^�C�v�`���ŕ�����\������B
    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";                   //������
        foreach (char letter in dialog) 
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f/letterPerSecond);    //�����̕\�����x�@�P�b�ŉ�����
        }

        yield return new WaitForSeconds(0.7f);                      //���������������Ƃ���0.7�b�Ԃ��󂯂�
    }

    //UI�̕\��/��\��
    //dialogText�̕\���Ǘ�
    public void EnableDialogText(bool enabled)
    {
        dialogText.enabled = enabled;
    }
    //actionSelector�̕\���Ǘ�
    public void EnableActionSelector(bool enabled)
    {
        actionSelector.SetActive(enabled);
    }
    //moveSelector��moveDetails�̕\���Ǘ�
    public void EnableMoveSelector(bool enabled)
    {
        moveSelector.SetActive(enabled);
        moveDetails.SetActive(enabled);
    }

    //�v���C���[�s���I��
    //�I�𒆂̑I�����̐F��ς���B
    public void UpdateActionSelection(int selectAction)
    {
        //selectAction��0�̎���actionTexts[0]�̐F��ɂ���B����ȊO�͍�
        //selectAction��1�̎���actionTexts[1]�̐F��ɂ���B����ȊO�͍�
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

    //�v���C���[�Z�I��
    //�I�𒆂̑I�����̐F��ς���B
    public void UpdateMoveSelection(int selectMove, Move move)
    {
        //selectmove��0�̎���moveTexts[0]�̐F��ɂ���B����ȊO�͍�
        //selectmove��1�̎���moveTexts[1]�̐F��ɂ���B����ȊO�͍�
        //selectmove��2�̎���moveTexts[2]�̐F��ɂ���B����ȊO�͍�
        //selectmove��3�̎���moveTexts[3]�̐F��ɂ���B����ȊO�͍�
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

    //�E�Z���̔��f
    public void SetMoveNames(List<Move> moves)
        {
        //for���ł܂Ƃ߂Ă�
        // moveTexts[0].text = moves[0].Base.Name;
        // moveTexts[1].text = moves[1].Base.Name;
        // moveTexts[2].text = moves[2].Base.Name;
        // moveTexts[3].text = moves[3].Base.Name;

        for (int i = 0; i < moveTexts.Count; i++)
        {
            //�o���Ă��鐔�������f
            if (i < moves.Count)
            {
                moveTexts[i].text = moves[i].Base.Name; //�o���Ă���Z����\��
            }
            else
            {
                moveTexts[i].text = "-";                //����-
            }
        }
    }
    

}
