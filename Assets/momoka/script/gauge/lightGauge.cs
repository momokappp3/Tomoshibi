using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

/*
ゲームにfadeを追加
体力ゲージscaleとα追加
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

    [SerializeField] private float lightNum = 4000.0f;  //ゲージ本体
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
        unitLight = lightNum / lightTypeMax;  //一つの火の玉の時間
        lightMax = lightNum;
    }

    void Update()
    {
        if (lightNum <= 0.0f)
        {
            noLight = true; 
            lightNum = 0.0f;
            //一回炎全消しする
            SetLightGauge(lightType.Max);
        }
        else  //ラグが酷いelseで少し改善
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

            var rate = lightNum / lightMax;  //0～1に変換
            var invRate = 1.0f - rate;
            var light = (lightType)(lightTypeMax * invRate);  //ライト

            if (nowLight < light)
            {
                timerLight = 0.0f;
                nowLight = light;
                SetLightGauge(light);
            }
            else  //現在から消えるまでの間 (nowLight == light)同じとき
            {
                timerLight += delta;

                var effectRate = timerLight / unitLight;  //火の玉のRate 0～1

                if (effectRate < GAUGE_EFFECT_RATE)
                {
                    SetLightScaleAndAlpha(nowLight, 1 - effectRate);
                }
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

    //渡した引数まで大きさとアルファを変化させる   rate = 大きく  1 - rate = 小さく
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

        lightImage[gaugeInt].color = new Color(c.r, c.g, c.b, rate);  //アルファ
        lightImage[gaugeInt].rectTransform.localScale = new Vector3(rate, rate, rate);  //大きさ
    }
}