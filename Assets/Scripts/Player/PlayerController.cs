using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;                               //OnEncouunted�̓ǂݍ��݂Ɏg�p

//===�}�b�v�ړ��̃v���C���[���Ǘ�����X�N���v�g===//

//===����===//
//�v���C���[�̈ړ����쐬
//�E�L�[���͂ɂ��1�}�X�ړ����쐬
//�E�ړ��̔���i�ړ����Ă��邩���Ă��Ȃ����j���쐬

//�ǂ̍쐬
//�ETilemap���쐬
//�E�ǂɃR���C�_�[������
//�EPlayer����Ray���΂��āA�ǔ��������

//���ނ�Ń����_���G���J�E���g������
//�E���ނ������
//�E���ނ�ɃR���C�_�[������
//�E���ނ�𔻒������

//�����X�^�[�̃f�[�^�Ǘ�(BattleCharaBase)
//�E�����X�^�[�̑��l��:ScriptableObject���g��
//�E���x���ɉ������X�e�[�^�X�̑��l��:����p�̃N���X���g��


public class PlayerController : MonoBehaviour
{
    //Player�̂P�}�X�ړ�

    [SerializeField] float moveSpeed;                      //�ړ����x

    bool isMoving;                                         //�ړ����Ă��邩�̔���
    Vector2 input;

    Animator animator;                                     //�ϐ��̐錾
    
    //�ǔ����Layer
    [SerializeField] LayerMask solidObjectsLayer;
    
    //���ނ画���Layer
    [SerializeField] LayerMask longGrassLayer;

    //���݈ˑ��������GUnityAction(�֐���o�^����)
    public UnityAction OnEncounted;                        //�O��(GameController)��OnEncounted�𒼐ړǂݍ���     
    //[SerializeField] GameController gameController;      //GameController�œǂݍ��܂���(���݈ˑ������Ł��ɕύX)

    private void Awake()                                   //unity����animator���擾����B�i�ǂݍ��݁j
    {
        animator = GetComponent<Animator>();
    }
    public void HandleUpdate()
    {
        if (!isMoving)                                     //�ړ����͓��͂��󂯕t���Ȃ�
        {
            //�L�[�{�[�h�̓��͕����ɓ���
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            //�΂߈ړ��̖�����
            if (input.x != 0)                               //�������̓��͂�����Ă���ꍇ
            {
                input.y = 0;                                //�c�����̓��͂��󂯕t���Ȃ�
            }
            
            //���͂��������ꍇ
            if (input != Vector2.zero)
            {
                //������ς�����
                animator.SetFloat("moveX", input.x);        //unity��animetor���Z�b�g����i�������j
                animator.SetFloat("moveY", input.y);        //unity��animetor���Z�b�g����i�c�����j
                Vector2 targetPos = transform.position;     //�L�[�{�[�h�œ��͂�������ړI�n�Ƃ��āA
                targetPos += input;
                if(IsWalkable(targetPos))                   //�ړ��\���𒲂ׂ�֐���true�̏ꍇ
                {
                    StartCoroutine(Move(targetPos));        //�R���[�`���ɂ��ړI�n�܂ňړ�������B
                }
            }
        }
        animator.SetBool("isMoving", isMoving);
    }
    //�R���[�`�����g���ď��X�ɖړI�n�ɋ߂Â���
    IEnumerator Move(Vector3 targetPos)
    {
        //�ړ����͓��͂��󂯕t�������Ȃ�
        isMoving = true;                                                        //�ړ����̔���ɂ���B

        //targetPos�Ƃ̍�������Ȃ�J��Ԃ�
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)   //�����ɉ�����{}���̖��߂𕜏�������
        {
            //targetPos�ɋ߂Â���
            transform.position = Vector3.MoveTowards(
                transform.position,                                              //���݂̏ꏊ
                targetPos,                                                       //�ړI�n
                moveSpeed*Time.deltaTime                                         //�߂Â���ۂ̑��x
                );
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;                                                        //�ړ����Ă��Ȃ�����ɂ���(�ړ������̊�����)
        CheckForEncounters();                                                    //�ړ��������I�������ɃG���J�E���g�̊m�F   
    }

    //targetPos�Ɉړ��\���𒲂ׂ�֐��@=�\��true�s�\��false��Ԃ�
    bool IsWalkable(Vector2 targetPos)
    {
        //��(solidObjectsLayer)�ɓ������Ă��邩�̔���
        return Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) == false; //targetPos�ɔ��a0.2f�łł����~�`��Ray���쐬���A    
                                                                                     //����Ray��solidObjectsLayer�ɂԂ�������false��Ԃ��i�v���C��[��y���W��+0.8����Ɣ���ʒu�����x�ɂȂ�j
    }

    //�v���C���[�̍��W����~�`��Ray���΂��āA���ނ�Layer�ɓ��������烉���_���G���J�E���g
    void CheckForEncounters()
    {
        if(Physics2D.OverlapCircle(transform.position, 0.2f, longGrassLayer))
        {
            //�����_���G���J�E���g
            if (Random.Range(0, 100) < 10)
            {
                //Random..Range(0,100);0�`99�܂ł̂ǂꂩ�̐������o��
                //10��菬������������0�`9�܂ł�10��10��
                //10�ȏ�̐�����10�`99�܂ł�90��90��
                Debug.Log("�����X�^�[�ɑ���");

                OnEncounted();                                          //GameController�ɂ���֐��𒼐ړǂݍ���
                //gameController.StartBattle();                         //BattleSystem��ǂݍ���(�퓬�ֈڍs)
                animator.SetBool("isMoving", false);                    //�퓬��ʂɐ؂�ւ��ƃv���C���[�̓������~
            }
        }
    }
}
