using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;           //BattleOver�Œ��ړǂݍ��ޗp

//===�퓬���̊e�v���C���[�̍s�����Ǘ�����X�N���v�g===//
//�퓬�t�F�[�Y���
public enum BattleState
{
    Start,          //�퓬�J�n�t�F�[�Y
    PlayerAction,   //�s���I���t�F�[�Y
    //ActuonSelection
    PlayerMove,     //�Z�I���t�F�[�Y
    EnemyMove,
    //MoveSelection
    //PerformScreen //�Z���s�t�F�[�Y
    Busy,           //������
    BattleOver,     //�퓬�I���t�F�[�Y
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit;         //������[BattleUnit]�̒��ɂ���[playerUnit]�֐���ǂݏo��
    [SerializeField] BattleUnit enemyUnit;          //unity���ŃX�N���v�g���Z�b�g���Ȃ���΂Ȃ�Ȃ�
    [SerializeField] BattleHUD  playerHUD;          //�ǂݏo��
    [SerializeField] BattleHUD  enemyHUD;           //�ǂݏo��
    [SerializeField] BattleDialogBox dialogBox;     //�ǂݏo��

    public UnityAction BattleOver;                  //GameController���璼�ړǂݍ���
    //[SerializeField] GameController gameController; //�ǂݏo��

    //BattleState��ϐ�state�ɒu������
    BattleState state;    

    //�v���C���[�̍s���I��
    int currentAction;      //0=fight���������@1=run������
    //�v���C���[�̋Z�I��
    int currentMove;        //0=����A1=�E��A2�������A3=�E��

    //�Z�b�g����L�����̕ϐ�
    BattleCharaParty playerParty;
    BattleChara enemybattleChara;
    
    //�퓬�V�X�e���̓ǂݍ��݊J�n(�Z�b�g����L�����̕ϐ��̎Q�Ƃ��s���j
    public void StartBattle(BattleCharaParty playerParty, BattleChara enemybattleChara)                    
    {
        this.playerParty = playerParty;
        this.enemybattleChara = enemybattleChara;
        StartCoroutine(SetupBattle());

    }

    IEnumerator SetupBattle()
    {
        state = BattleState.Start;

        //�L�����̐����ƕ`��
        playerUnit.Setup(playerParty.GetHealthyBattleChara()); //�v���C���[�̑I�������L�������Z�b�g
        enemyUnit.Setup(enemybattleChara);  //�G�l�~�[�ɑI�΂ꂽ�L�������Z�b�g

        //HUD�̕`��
        playerHUD.SetData(playerUnit.BattleChara);
        enemyHUD.SetData(enemyUnit.BattleChara);

        //�v���C���[�̋Z��`��
        dialogBox.SetMoveNames(playerUnit.BattleChara.Moves);

        //�Eү���ނ��o�āA�P�b���ActionSelector��\������B
        yield return dialogBox.TypeDialog($"�ΐ푊��� {enemyUnit.BattleChara.Base.Name} �����ꂽ�I");    //�e�L�X�g�\��
        PlayerAction();
    }

    //�s���I���t�F�[�Y
    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        dialogBox.EnableActionSelector(true);
        StartCoroutine (dialogBox.TypeDialog("��������H"));     //�e�L�X�g�\��
    }

    //�Z�I���t�F�[�Y
    void PlayerMove()   //Z���������Ƃ��̏���
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableDialogText(false);          //DialogText���\���ɂ���
        dialogBox.EnableActionSelector(false);      //ActionSelector���\���ɂ���
        dialogBox.EnableMoveSelector(true);         //MoveSelector��\��������
    }

    //PlayerMove�̎��s(�v���C���[�̃^�[��)
    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;

        //�Z������
        Move move = playerUnit.BattleChara.Moves[currentMove];                                           //���肳�ꂽ�Z�̓ǂݍ���
        move.PP--;                                                                                       //PP�̏���
        yield return dialogBox.TypeDialog($"{playerUnit.BattleChara.Base.Name}��\n{move.Base.Name}");    //�e�L�X�g�\��

        //��Ұ���
        playerUnit.PlayerAttackAnimetion();         //�U����Ұ���
        yield return new WaitForSeconds(0.7f);      //��Ұ��ݑҋ@����
        
        enemyUnit.PlayerHitAnimetion();             //��e���̱�Ұ���


        //player�_���[�W�v�Z
        DamageDetails damageDetails = enemyUnit.BattleChara.TakeDamage(move, playerUnit.BattleChara);

        //HPbar�ɔ��f
        yield return enemyHUD.UpdateHP();                //�G�l�~�[�̗̑̓Q�[�W�ɔ��f������

        //����/�N���e�B�J���̃��b�Z�[�W
        yield return ShowDamegeDetails(damageDetails);   //��������/�N���e�B�J���������̃��b�Z�[�W�\��

        //�퓬�s�\�Ȃ烁�b�Z�[�W
        if (damageDetails.Fainted) 
        {
            yield return dialogBox.TypeDialog($"{enemyUnit.BattleChara.Base.Name}�͓|�ꂽ�I");    //�e�L�X�g�\��
            enemyUnit.PlayerFaintAnimation();                                                     //�퓬�s�\��Ұ���
            yield return new WaitForSeconds(0.7f);                                                //�e�L�X�g�\����������ʂ�0.7�b�҂�

            //�퓬�I��
            BattleOver();                       //GameController��EndBattle�𒼐ړǂݍ���
            //gameController.EndBattle();
        }
        //�퓬�\�Ȃ�EnemyMove
        else
        {

            StartCoroutine(EnemyMove());
        }

    }

    //EnemyMove�̎��s(�G�l�~�[�̃^�[��)
    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;

        //�Z������(�����_��)
        Move move = enemyUnit.BattleChara.GetRandomMove();                                              //���肳�ꂽ�Z�̓ǂݍ���
        move.PP--;                                                                                      //PP�̏���
        yield return dialogBox.TypeDialog($"{enemyUnit.BattleChara.Base.Name}��\n{move.Base.Name}");    //�e�L�X�g�\��
        
        //�U�����̱�Ұ���
        enemyUnit.PlayerAttackAnimetion();          //�U����Ұ���                                               
        yield return new WaitForSeconds(0.7f);      //��Ұ��ݑҋ@����

        playerUnit.PlayerHitAnimetion();            //��e���̱�Ұ���

        //enemy�_���[�W�v�Z
        DamageDetails damageDetails = playerUnit.BattleChara.TakeDamage(move, enemyUnit.BattleChara);

        //HPbar�ɔ��f
        yield return playerHUD.UpdateHP();

        //����/�N���e�B�J���̃��b�Z�[�W
        yield return ShowDamegeDetails(damageDetails);  //��������/�N���e�B�J���������̃��b�Z�[�W�\��

        //�퓬�s�\�Ȃ烁�b�Z�[�W
        if (damageDetails.Fainted)
        {
            yield return dialogBox.TypeDialog($"{playerUnit.BattleChara.Base.Name}�͓|�ꂽ�I");    //�e�L�X�g�\��
            playerUnit.PlayerFaintAnimation();                                                     //�퓬�s�\��Ұ���
            yield return new WaitForSeconds(0.7f);                                              �@ //�e�L�X�g�\����������ʂ�0.7�b�҂�

            //�퓬�I��
            BattleOver();                       //GameController��EndBattle�𒼐ړǂݍ���

        }
        //�퓬�\�Ȃ�EnemyMove
        else
        {
            PlayerAction();
        }

    }

    //��������/�N���e�B�J���������ɕ\�����郁�b�Z�[�W�̔���
    IEnumerator ShowDamegeDetails(DamageDetails damageDetails)
    {
        if (damageDetails.TypeEffectiveness > 1f)
        {
            yield return dialogBox.TypeDialog($"���ʂ͔��Q���I");      //�e�L�X�g�\���i�������ǁj
        }
        if (damageDetails.TypeEffectiveness < 1f)
        {
            yield return dialogBox.TypeDialog($"���ʂ͂��܂ЂƂc");        //�e�L�X�g�\���i���������j
        }
        //if (damageDetails.Critical > 1f)
        //{
        //yield return dialogBox.TypeDialog($"�}���ɂ��������I");          //�e�L�X�g�\���i�N���e�B�J�������j
        //}

    }

    public void HandleUpdate()
    {
        if (state == BattleState.PlayerAction)      //�s���I���t�F�[�Y�ڍs���̃R�[�h�ǂݏo��
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove)   //�Z�I���t�F�[�Y�ڍs���̃R�[�h�ǂݏo��
        {
            HandleMoveSelection();
        }
    }

    //�s���I���t�F�[�Y�ڍs���̃R�[�h���e
    void HandleActionSelection()
    {
        //�EZ�{�^���������ƁAMoveSelector��MoveDetails��\������B
        //���L�[����͂����Run(1)�A���L�[��������fight(0)
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentAction < 1)
            {
                currentAction++;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentAction > 0)
            {
                currentAction--;
            }
        }

        //�F�����Ăǂ�������񂽂����Ă���̂��킩��悤�ɂ���B
        //�EActionSelector�łǂ����I�����Ă���̂����킩��₷������B
        dialogBox.UpdateActionSelection(currentAction);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)                         //fight��I�������Ƃ�
            {
                PlayerMove();
            }
        }
    }

    //�Z�I���t�F�[�Y�ڍs���̃R�[�h���e
    void HandleMoveSelection()
    {
        //�EZ�{�^���������ƁAMoveSelector(�Z�̑I����)��MoveDetails(PP�ƃ^�C�v)��\������B
        if (Input.GetKeyDown(KeyCode.RightArrow))       //�E���L�[�̓��͎�
        {
            if (currentMove < playerUnit.BattleChara.Moves.Count - 1)
            {
                currentMove++;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))        //�����L�[�̓��͎�
        {
            if (currentMove > 0)
            {
                currentMove--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))   //�����L�[�̓��͎�
        {
            if (currentMove < playerUnit.BattleChara.Moves.Count - 2)
            {
                currentMove += 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))     //����L�[�̓��͎�
        {
            if (currentMove > 1)
            {
                currentMove -= 2;
            }
        }
        dialogBox.UpdateMoveSelection(currentMove, playerUnit.BattleChara.Moves[currentMove]);

        //�EZ�{�^���������ƋZ������
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //�E�Z�I����UI���\��
            dialogBox.EnableMoveSelector(false);

            //�E�_�C�A���O��\��
            dialogBox.EnableDialogText(true);

            //�E�Z����̏���
            StartCoroutine(PerformPlayerMove());


        }
    }
}
