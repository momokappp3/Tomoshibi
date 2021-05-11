using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

/*
�Q�[����fade��ǉ�
�̗̓Q�[�Wscale�ƃ��ǉ�
 */

public class lightGauge : MonoBehaviour
{
    private const float GAUGE_EFFECT_RATE = 1.0f;

    private enum lightType
    {
        Red = 0,
        Red1,
        Red2,
        Red3,
        Red4,
        Red5,
        Red6,

        Max
    }

    [SerializeField] private float lightNum = 4000.0f;  //�Q�[�W�{��
    public bool noLight = false;

    [SerializeField] private float downLightNum = 0.01f;
    [SerializeField] private List<Image> lightImage = new List<Image>();
    private float lightMax;
    private lightType nowLight = lightType.Red;
    private float lightTypeMax = (float)lightType.Max;
    private float unitLight = 0.0f;
    private float timerLight = 0.0f;
    //int k = 0;
    
    public void decLight(float degree)
    {
        lightNum -= degree;

        //lightImage[k].color -= new Color(0.0f, 0.0f, 0.0f, 100.0f);
        // k += 1;
    }

    void Start()
    {
        unitLight = lightNum / lightTypeMax;  //��̉΂̋ʂ̎���
        lightMax = lightNum;
    }

    void Update()
    {
        if (lightNum <= 0.0f)
        {
            noLight = true; 
            lightNum = 0.0f;
            //��񉊑S��������
            SetLightGauge(lightType.Max);
        }
        else  //���O������else�ŏ������P
        {
            UpdataGage();
        }
    }

    void UpdataGage()
    {
        if (lightNum > 0.0f)
        {
            var delta = downLightNum * Time.deltaTime;

            lightNum -= delta;

            if (lightNum < 0.0f)
            {
                lightNum = 0.0f;
            }

            var rate = lightNum / lightMax;  //0�`1�ɕϊ�
            var invRate = 1.0f - rate;
            var light = (lightType)(lightTypeMax * invRate);  //���C�g

            if (nowLight < light)
            {
                timerLight = 0.0f;
                nowLight = light;
                SetLightGauge(light);
            }
            else  //���݂��������܂ł̊� (nowLight == light)�����Ƃ�
            {
                timerLight += delta;

                var effectRate = timerLight / unitLight;  //�΂̋ʂ�Rate 0�`1

                if (effectRate < GAUGE_EFFECT_RATE)
                {
                    SetLightScaleAndAlpha(nowLight, 1 - effectRate);
                }
            }
        }
    }

    //�n���������Ƃ���ȉ����\���ɂ���
    void SetLightGauge(lightType gauge)
    {
        int gaugeInt = (int)gauge;
        int max = (int)lightType.Max;

        foreach (int light in System.Enum.GetValues(typeof(lightType)))
        {
            if(light == max)   //�o�O�Œǉ�
            { 
                break;
            }
            lightImage[light].enabled = !(light < gaugeInt);
        }
    }

    //�n���������܂ő傫���ƃA���t�@��ω�������   rate = �傫��  1 - rate = ������
    void SetLightScaleAndAlpha(lightType gauge, float rate)
    {

        int gaugeInt = (int)gauge;
        var c = lightImage[gaugeInt].color;

        if (rate > GAUGE_EFFECT_RATE)
        {
            rate = GAUGE_EFFECT_RATE;
        }

        if (rate < 0.0f)
        {
            rate = 0.0f;
        }

        lightImage[gaugeInt].color = new Color(c.r, c.g, c.b, rate);  //�A���t�@
        lightImage[gaugeInt].rectTransform.localScale = new Vector3(rate, rate, rate);  //�傫��
    }
}