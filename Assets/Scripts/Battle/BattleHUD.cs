using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//===�퓬���ɕ\������HUD���Ǘ�����X�N���v�g===//

public class BattleHUD : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] HPBar hpBar;

    BattleChara _battleChara;

    public void SetData(BattleChara battleChara)
    {
        _battleChara = battleChara;
        nameText.text = battleChara.Base.Hudname;
        //���x���͏ȗ�
        hpBar.SetHP((float)battleChara.HP / battleChara.MaxHP);
    }

    //HP�����̔��f
    public IEnumerator UpdateHP()
    {
        yield return hpBar.SetHPSmooth((float)_battleChara.HP / _battleChara.MaxHP);
    }


}
