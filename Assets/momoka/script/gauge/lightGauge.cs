using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class lightGauge : MonoBehaviour
{

    private const float GAUGE_EFFECT_RATE = 0.5f;

    private enum lightType
    {

        Red = 0,
        Purple,
        Orange,
        Yellow,
        Blue,

        Max
    }

    //ゲージ本体なのでこれを呼び出して減らしてください
    public float lightNum = 1000.0f;

    [SerializeField] private float downLightNum = 0.01f;
    [SerializeField] private List<Image> lightImage = new List<Image>();
    private float lightMax;
    private lightType nowLight = lightType.Red;
    private float lightTypeMax = (float)lightType.Max;
    private float unitLight = 0.0f;
    private float timerLight = 0.0f;

    int k = 0;

    
    public void decLight(float degree)
    {
        lightImage[k].color -= new Color(0, 0, 0, 100);
        k += 1;
    }

    void Start()
    {
        unitLight = lightNum / lightTypeMax;
        lightMax = lightNum;
    }

    void Update()
    {
        UpdataGage();
    }

    void UpdataGage()
    {

        //Debug.Log(lightNum);
        if (lightNum > 0.0f)
        {
            var delta = downLightNum * Time.deltaTime;

            lightNum -= delta;

            if (lightNum < 0.0f)
            {
                lightNum = 0.0f;
            }

            //Debug.Log(lightNum);

            var rate = lightNum / lightMax;  //0～1に変換
            var invRate = 1.0f - rate;
            var light = (lightType)(lightTypeMax * invRate);

            if (nowLight < light)
            {
                timerLight = 0.0f;
                nowLight = light;
                SetLightGauge(light);
            }
            else
            {
                timerLight += delta;

                var effectRate = timerLight / unitLight;

                if(effectRate < GAUGE_EFFECT_RATE)
                {
                    SetLightScale(nowLight, effectRate);
                }/*
                else
                {
                    SetLightScaleAndAlpha()
                }
                */
            }
        }
    }

    //渡した引数とそれ以下を非表示にする
    void SetLightGauge(lightType gauge)
    {
        int gaugeInt = (int)gauge;
        int max = (int)lightType.Max;

        foreach (int light in System.Enum.GetValues(typeof(lightType)))
        {
            if(light == max)   //バグで追加
            { 
                break;
            }
            lightImage[light].enabled = !(light < gaugeInt);
        }
    }

    void SetLightScale(lightType gauge, float rate)
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

        float val = 1.0f - rate;

        lightImage[gaugeInt].color = new Color(c.r, c.g, c.b, val);

        lightImage[gaugeInt].rectTransform.localScale = new Vector3(val, val, val);
    }

    private void Down(float num)
    {
        lightNum -= num;
    }
}