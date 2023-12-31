using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//===í¬É\¦·éHUDðÇ·éXNvg===//

public class BattleHUD : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] HPBar hpBar;

    BattleChara _battleChara;

    public void SetData(BattleChara battleChara)
    {
        _battleChara = battleChara;
        nameText.text = battleChara.Base.Hudname;
        //xÍÈª
        hpBar.SetHP((float)battleChara.HP / battleChara.MaxHP);
    }

    //HP¸­Ì½f
    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float)_battleChara.HP / _battleChara.MaxHP);
    }


}
