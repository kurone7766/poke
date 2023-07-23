using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//===�Q�[���̏�Ԃ��Ǘ�����X�N���v�g===//

public enum GameState   //�񋓌^�ŃQ�[���󋵂�c��
{
    FreeRoam,   //�}�b�v�ړ�(���g�p)    �����킹�̂Ƃ��ɖ��O�ύX������
    //Title,    //�^�C�g��
    //Select,   //�L�����N�^�[�I��  �i���킹�ŕύX�j
    Battle,     //�ΐ풆

}

public class GameController : MonoBehaviour
{
    //�Q�[���̏�Ԃ��Ǘ�
    [SerializeField] PlayerController playerController;
    [SerializeField] BattleSystem battleSystem;

    [SerializeField] Camera worldCamera;  //�t�B�[���h���

    GameState state = GameState.FreeRoam; //������Ԃ�FreeRoam��

    private void Start()
    {
        playerController.OnEncounted += StartBattle;    //�O���œǂ݂��߂�֐��Ƃ��ēo�^����
        battleSystem.BattleOver += EndBattle;           //�O���œǂ݂��߂�֐��Ƃ��ēo�^����
    }


    //�퓬�J�n
    public void StartBattle()
    {
        state = GameState.Battle;                   //�Q�[���󋵂�ΐ풆��
        battleSystem.gameObject.SetActive(true);    //�퓬��ʂ�\��
        worldCamera.gameObject.SetActive(false);    //�t�B�[���h��ʂ��\��
        
        //�v���C���[���I�������L�����ƃG�l�~�[�ɑI�����ꂽ�L�������擾
        BattleCharaParty playerParty = playerController.GetComponent<BattleCharaParty>();
        //FindObjectOfType�ŃV�[���̒������v����R���|�[�l���g���P�擾����(Grid��EnemyCharaSelect)
        BattleChara enemybattleChara = FindObjectOfType<EnemyCharaSelect>().GetRandomChoiceBattleChara();
        
        battleSystem.StartBattle(playerParty, enemybattleChara);                 //�퓬�V�X�e���̓ǂݍ��݊J�n�i���Z�b�g�j
    }

    //�퓬�I��
    public void EndBattle()
    {
        state = GameState.FreeRoam;                 //�Q�[���󋵂��}�b�v�ړ���
        battleSystem.gameObject.SetActive(false);   //�퓬��ʂ��\��
        worldCamera.gameObject.SetActive(true);    //�t�B�[���h��ʂ�\��
    }

    void Update()
    {
        if (state == GameState.FreeRoam)        �@//�}�b�v�ړ�(���g�p)�̂Ƃ�
        {
            playerController.HandleUpdate();
        }
        //else if (state == GameState.Title)      //�^�C�g��(���g�p)�̂Ƃ� 
        //{
        //    Title
        //}
        //else if (state == GameState.Select)     //�L�����I��(���g�p)�̂Ƃ�
        //{
        //    Select
        //}
        else if (state == GameState.Battle)       //�퓬���̂Ƃ�
        {
            battleSystem.HandleUpdate();
        }
    }
}
