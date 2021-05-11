using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    private const float GAUGE_EFFECT_RATE = 1.0f;
    private const float ADD_ANGLE = 0.3f;
    private const float RENGE_Y = 40.0f;

    [System.Serializable] private class TitleFade
    {
        public float wait;  //�n�܂��Ă��牽�b��
        public float anim;   //���b�����ă���������
        public float alpha = 0.0f;
    }

    private enum lightType{

        Rogo = 0,
        Car,
        String,
        Ghost,

        Max
    }

    [SerializeField] private Image selectImage;
    [SerializeField] private List<Image> colorImage = new List<Image>();
    [SerializeField] private List<TitleFade> fadeInfo = new List<TitleFade>();

    private bool onEnter = false;
    private bool onSelect = false;

    private float countUpGoGame = 0.0f;
    [SerializeField] public float timeLimitGoGame = 0.0f;  //�V�[�����ڂ鎞���b�����邩

    private float angle = 0.0f;
    private Vector2 firstGhost = Vector2.zero;
    private int rengeCount = 0;
    private float rengeScale = 1.0f;
    private float lastRengeScale = 1.0f;
    private float nowRengeScale = 1.0f;

    private void Start()
    {
        foreach (var image in colorImage)
        {
            image.enabled = false;
        }

        var c = selectImage.color;

        selectImage.color = new Color(c.r, c.g, c.b, 0.0f);  //�A���t�@
        selectImage.rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);  //�傫��

        //�ŏ��̂��΂���xy���W
        firstGhost = colorImage[(int)lightType.Ghost].rectTransform.anchoredPosition;
    }

    void Update()
    {
        UpdateGhost();

        if (!onSelect && Input.anyKeyDown )
        {
            //����SE�����
            onSelect = true;
        }

        if(!onEnter && onSelect)
        {
            //SetLightScaleAndAlpha(rate);
        }

        if (selectImage.enabled && Input.GetKeyUp(KeyCode.Return))
        {
            onEnter = true;

            
            for (var i = 0; i < fadeInfo.Count; i++)   //���������Ȃ���
            {
                fadeInfo[i].alpha = 0.0f;
                fadeInfo[i].anim /= 1.5f;  //1.5�{�ɑ������Ă���
            }
        }

        if (onEnter)
        {
            ReverseUpdateAlpha();
            //SetLightScaleAndAlpha(1 - rate);

            countUpGoGame += Time.deltaTime;

            if (countUpGoGame >= timeLimitGoGame)
            {  //�J�E���g
                SceneManager.LoadScene("GameSample");
            }
        }
        else
        {
            UpdateAlpha();
        }
    }

    //===============================================
    //�I���摜���̑傫���ƃA���t�@�̑���
    //�n���������܂ő傫���ƃA���t�@��ω�������   rate = �傫��  1 - rate = ������

    void SetLightScaleAndAlpha(float rate)
    {

        var c = selectImage.color;

        if (rate > GAUGE_EFFECT_RATE)
        {
            rate = GAUGE_EFFECT_RATE;
        }

        if (rate < 0.0f)
        {
            rate = 0.0f;
        }

        selectImage.color = new Color(c.r, c.g, c.b, rate);  //�A���t�@
        selectImage.rectTransform.localScale = new Vector3(rate, rate, rate);  //�傫��
    }

    //=======================================================================
    //���΂��̓���

    private void UpdateGhost()
    {
        RectTransform rt = colorImage[(int)lightType.Ghost].rectTransform;

        angle += ADD_ANGLE;

        int count = (int)(angle / 180.0f);

        if(count > rengeCount)
        {
            rengeCount = count;
            lastRengeScale = rengeScale;
            nowRengeScale = Random.Range(0.8f, 1.2f);
        }

        rengeScale = Mathf.Lerp(lastRengeScale, nowRengeScale, 0.4f);

        //-1�`1�͈̔͂�RENGE_Y��������RENGE_Y�̕��ɕϊ�
        var y = Mathf.Cos(angle * Mathf.Deg2Rad) * RENGE_Y * rengeScale;

        rt.anchoredPosition = new Vector2(firstGhost.x + y, firstGhost.y + y);

    }

    //=============================================================================
    //�^�C�g���摜��Alpha����

    private void UpdateAlpha()
    {
        for (var i = 0; i < fadeInfo.Count; i++)
        {
           if ( fadeInfo[i].wait > 0.0f)
           {
                fadeInfo[i].wait -= Time.deltaTime;
            }
            else
            {
                fadeInfo[i].alpha += Time.deltaTime;

                var a = fadeInfo[i].alpha / fadeInfo[i].anim;  //0�`1�̊���

                SetImageAlpha(i, a);
            }
        }
    }

    private void ReverseUpdateAlpha()
    {
        for (var i = 0; i < fadeInfo.Count; i++)
        {
            if (fadeInfo[i].wait > 0.0f)
            {
                fadeInfo[i].wait -= Time.deltaTime;
            }
            else
            {
                fadeInfo[i].alpha += Time.deltaTime;

                var a = fadeInfo[i].alpha / fadeInfo[i].anim;  //0�`1�̊���

                SetImageAlpha(i,1 - a);
            }
        }
    }

    private void SetImageAlpha(int index, float alpha)
    {
        if (index < 0 || index >= colorImage.Count)
        {
            return;
        }

        if (alpha > 1.0f)
        {
            alpha = 1.0f;
        }

        if (alpha < 0.0f)
        {
            alpha = 0.0f;
        }

        if (!colorImage[index].enabled)
        {
            colorImage[index].enabled = true;
        }

        var c = colorImage[index].color;

        colorImage[index].color = new Color(c.r, c.g, c.b, alpha);
    }
}