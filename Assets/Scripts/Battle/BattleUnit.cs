using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

//===�퓬���̃L�������擾���L�����摜�\�����s���X�N���v�g===//

public class BattleUnit : MonoBehaviour
{
    //�ύX�F��킹��L������BattleSystem����Z�b�g����B
    //[SerializeField] BattleCharaBase _base;   //BattleSystem.cs�ŃZ�b�g���邽�ߕs�v
    //[SerializeField] int level;               //BattleSystem.cs�ŃZ�b�g���邽�ߕs�v
    [SerializeField] bool isPlayerUnit;

    public BattleChara BattleChara { get; set; }

    Vector3 originalPos;    //�����ʒu�̍��W(�o���Ұ��ݗp)
    Color originalColor;    //�����J���[(��e���̱�Ұ��ݗp)
    Image image;

    //�o�g���Ŏg���L������ێ�
    //�L�����̉摜�𔽉f����B

    //unity�Őݒu�����摜�̍�����ێ�
    private void Awake()
    {
        image = GetComponent<Image>();
        originalPos = transform.localPosition;
        originalColor = image.color;
    }

    //�Ή������L�����̉摜��ǂݍ���
    public void Setup(BattleChara battleChara)
    {
        //_base���烌�x���ɉ����������X�^�[�𐶐�����B
        //BattleSystem�Ŏg�����߂Ƀv���p�e�B�ɓ����B
        BattleChara = battleChara; //BattkeChara.cs����Q�Ɓ@new BattleChara(_base, level);

        if (isPlayerUnit)
        {
            image.sprite = BattleChara.Base.LeftSprite;
        }
        else
        {
            image.sprite = BattleChara.Base.RightSprite;
        }
        image.color = originalColor;                        //�퓬���ɃL�����摜�̐F��߂�
        PlayerEnterAnimetion();
    }

    //�L�����N�^�[�o���Ұ���
    public void PlayerEnterAnimetion()
    {
        if(isPlayerUnit)
        {
            //���[�ɔz�u
            transform.localPosition = new Vector3(-1000,originalPos.y);
        }
        else
        {
            //�E�[�ɔz�u
            transform.localPosition = new Vector3(1000, originalPos.y);
        }
        //�퓬���̈ʒu�܂ŃA�j���[�V����
        transform.DOLocalMoveX(originalPos.x, 1f);  //1�b�������Ĕz�u�ꏊ�܂œ���

    }

    //�L�����N�^�[�U����Ұ���
    public void PlayerAttackAnimetion()
    {
        //�V�[�P���X
        //�E�ɓ�������A���̈ʒu�ɖ߂�
        Sequence sequence = DOTween.Sequence();                                   //DOTween�̎g�p��錾
        if (isPlayerUnit)   //�v���C���[��
        {
            sequence.Append(transform.DOLocalMoveX(originalPos.x + 50f, 0.25f));  //DOTween�ɂ��0.25�b��x���W���{50����
        }
        else�@              //�G�l�~�[��
        {
            sequence.Append(transform.DOLocalMoveX(originalPos.x - 50f, 0.25f));  //DOTween�ɂ��0.25�b��x���W��-50����
        }
        sequence.Append(transform.DOLocalMoveX(originalPos.x, 0.2f));             //DOTween�ɂ��0.2�b��x���W�������l�ɂ���
    }

    //�L�����N�^�[��e��Ұ���
    public void PlayerHitAnimetion()
    {
        //�i�U�����󂯂����Ɂj�L�����̐F����xGLAY�ɂ��Ă���߂�
        Sequence sequence = DOTween.Sequence();                                   //DOTween�̎g�p��錾
        sequence.Append(image.DOColor(Color.gray, 0.1f));                         //DOTween�ɂ��L������0.1�b�D�F�ɂ���
        sequence.Append(image.DOColor(originalColor, 0.1f));                      //DOTween�ɂ��L������0.1�b�Œʏ�F�ɖ߂�
    }

    //�L�����N�^�[�퓬�s�\��Ұ���
    public void PlayerFaintAnimation()
    {
        //��ʒ[�ɉ�����Ȃ��甖���Ȃ�
        Sequence sequence = DOTween.Sequence();                                   //DOTween�̎g�p��錾
        if (isPlayerUnit)   //�v���C���[��
        {
            sequence.Append(transform.DOLocalMoveX(originalPos.x - 500, 1.5f));   //DOTween�ɂ��1.5�b��x���W��-500����
            sequence.Join(image.DOColor(Color.white, 0.25f));                         //DOTween�ɂ��L������0.1�b���F�ɂ���
            sequence.Join(image.DOFade(0, 1f));                                       //DOTween�ɂ����Append��K�p���Ȃ���L������0.5�b��0�̔����ɂ���B

            sequence.Append(transform.DOLocalMoveX(originalPos.x - 500, 0.5f));   //DOTween�ɂ��0.5�b��x���W��-500����
        }
        else�@              //�G�l�~�[��
        {
            sequence.Append(transform.DOLocalMoveX(originalPos.x + 500, 1.5f));   //DOTween�ɂ��1.5�b��x���W��+1000����
            sequence.Join(image.DOColor(Color.white, 0.25f));                         //DOTween�ɂ��L������0.1�b���F�ɂ���
            sequence.Join(image.DOFade(0, 1f));                                       //DOTween�ɂ����Append��K�p���Ȃ���L������0.5�b��0�̔����ɂ���B

            sequence.Append(transform.DOLocalMoveX(originalPos.x + 500, 0.5f));   //DOTween�ɂ��0.5�b��x���W��+1000����

        }
    }



}
