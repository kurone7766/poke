using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===�L�������g�p����Z�̏����Ǘ�����X�N���v�g===//

public class Move
{
    //BattleChara�����ۂɎg���Ƃ��̋Z�f�[�^

    //�Z�̃}�X�^�[�f�[�^������
    //�g���₷���悤�ɂ��邽�߂�PP������

    //BattleChara.cs���Q�Ƃ��邽�߁Apublic(���J)�ɂ��Ă���
    public MoveBase Base { get; set; }  //Movebase.cs����l���Q��
    public int PP { get; set; }

    //�����ݒ�
    public Move(MoveBase pBase)
    {
        Base = pBase;
        PP = pBase.PP;
    }

}