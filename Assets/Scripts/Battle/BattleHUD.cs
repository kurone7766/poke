using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//===戦闘時に表示するHUDを管理するスクリプト===//

public class BattleHUD : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] HPBar hpBar;

    BattleChara _battleChara;

    public void SetData(BattleChara battleChara)
    {
        _battleChara = battleChara;
        nameText.text = battleChara.Base.Hudname;
        //レベルは省略
        hpBar.SetHP((float)battleChara.HP / battleChara.MaxHP);
    }

    //HP減少の反映
    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float)_battleChara.HP / _battleChara.MaxHP);
    }


}
