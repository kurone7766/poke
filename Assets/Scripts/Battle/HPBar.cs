using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===BattleHUD�ɘA�Ȃ�HP�o�[�\���֘A�̃X�N���v�g===//

public class HPBar : MonoBehaviour
{
    //HP�̑�����`�������
    [SerializeField] GameObject health;

    public void SetHP(float hp)
    {
        health.transform.localScale = new Vector3(hp, 1, 1);
    }

    public IEnumerator SetHPSmooth(float newHP)
    {
        float currentHP = health.transform.localScale.x;
        float changeAmount = currentHP - newHP;

        //currentHP��newHP�ɍ����킸���ł�����Ȃ�J��Ԃ��B
        while (currentHP - newHP > Mathf.Epsilon)                       //currentHP�����Ԃ�������newHP�Ɠ����l�ɂ���(Mathf.Epsilon�ׂ͍�������)
        {
            currentHP -= changeAmount * Time.deltaTime;                    
            health.transform.localScale = new Vector3(currentHP, 1, 1); //��b�������Č��炷
            yield return null;
        }
        health.transform.localScale = new Vector3(newHP, 1, 1);
    }
}
