using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public float maxAlpha = 4.2f;


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
        Gost,

        Max
    }

    [SerializeField] private Image selectImage;

    [SerializeField] private List<Image> colorImage = new List<Image>();
    [SerializeField] private List<TitleFade> fadeInfo = new List<TitleFade>();

    private bool onEnter;

    private float countUpGoGame = 0.0f;
    [SerializeField] public float timeLimitGoGame = 5.0f;  //�V�[�����ڂ鎞���b�����邩


    private void Start()
    {
        foreach (var image in colorImage)
        {
            image.enabled = false;
        }

        selectImage.enabled = false;
    }

    void Update()
    {


        if (onEnter)
        {
            selectImage.enabled = false;
            ReverseUpdateAlpha();

            countUpGoGame += Time.deltaTime;

            if (countUpGoGame >= timeLimitGoGame) {  //�J�E���g
                SceneManager.LoadScene("GameSample");
            }

        }
        else
        {
            if (!selectImage.enabled && Input.anyKeyDown)
            {

                selectImage.enabled = true;
            }

            if (selectImage.enabled && Input.GetKey(KeyCode.Return))
            {
                onEnter = true;

                for (var i = 0; i < fadeInfo.Count; i++)   //���������Ȃ���
                {
                    fadeInfo[i].alpha = 0.0f;
                    fadeInfo[i].anim /= 2.0f;
                }
            }
            UpdateAlpha();
        }
    }

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
                if (fadeInfo[i].alpha > maxAlpha)
                {
                    break;
                }
                fadeInfo[i].alpha += Time.deltaTime;
                var a = fadeInfo[i].alpha / fadeInfo[i].anim;  //0�`1�̊���

                SetImageAlpha(i, a);
            }
        }
    }

    private void ReverseUpdateAlpha()
    {  //���΂����������ĂȂ�
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

                SetImageAlpha(i, 1 - a);
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