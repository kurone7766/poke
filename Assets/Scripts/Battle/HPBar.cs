using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===BattleHUDに連なりHPバー表示関連のスクリプト===//

public class HPBar : MonoBehaviour
{
    //HPの増減を描画をする
    [SerializeField] GameObject health;

    public void SetHP(float hp)
    {
        health.transform.localScale = new Vector3(hp, 1, 1);
    }

    public IEnumerator SetHPSmooth(float newHP)
    {
        float currentHP = health.transform.localScale.x;
        float changeAmount = currentHP - newHP;

        //currentHPとnewHPに差がわずかでもあるなら繰り返す。
        while (currentHP - newHP > Mathf.Epsilon)                       //currentHPを時間をかけてnewHPと同じ値にする(Mathf.Epsilonは細かい数字)
        {
            currentHP -= changeAmount * Time.deltaTime;                    
            health.transform.localScale = new Vector3(currentHP, 1, 1); //一秒をかけて減らす
            yield return null;
        }
        health.transform.localScale = new Vector3(newHP, 1, 1);
    }
}
