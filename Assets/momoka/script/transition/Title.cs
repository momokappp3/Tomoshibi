using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    private const float GAUGE_EFFECT_RATE = 1.0f;
    private const float ADD_ANGLE = 1.5f;         //0.3f エディタ上
    private const float RENGE_Y = 40.0f;

    [System.Serializable] private class TitleFade
    {
        public float wait;  //始まってから何秒後
        public float anim;   //何秒かけてαが動くか
        public float alpha = 0.0f;
    }

    private enum lightType{

        Rogo = 0,
        Car,
        String,
        Ghost,

        Max
    }

    public GameObject audi;

    [SerializeField] private AudioClip selectSE;
    [SerializeField] private Image selectImage;
    [SerializeField] private List<Image> colorImage = new List<Image>();
    [SerializeField] private List<TitleFade> fadeInfo = new List<TitleFade>();

    private bool onEnter = false;
    private bool onSelect = false;
    private bool onSound = false;

    private float countUpGoGame = 0.0f;
    [SerializeField] public float timeLimitGoGame = 5.0f;  //シーンを移る時何秒かけるか

    private float countSelectFire = 0.0f;
    [SerializeField] public float SelectFireTime = 3.0f;  //選択された炎何秒かけるか

    private float angle = 0.0f;
    private Vector2 firstGhost = Vector2.zero;
    private int rengeCount = 0;
    private float rengeScale = 1.0f;
    private float lastRengeScale = 1.0f;
    private float nowRengeScale = 1.0f;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        foreach (var image in colorImage)
        {
            image.enabled = false;
        }

        var c = selectImage.color;

        selectImage.color = new Color(c.r, c.g, c.b, 0.0f);  //アルファ
        selectImage.rectTransform.localScale = new Vector3(0.0f, 0.0f, 0.0f);  //大きさ

        //最初のおばけのxy座標
        firstGhost = colorImage[(int)lightType.Ghost].rectTransform.anchoredPosition;
    }

    void Update()
    {
        UpdateGhost();

        if (!onSelect && Input.anyKeyDown )
        {
            onSelect = true;
        }

        if(!onEnter && onSelect)
        {
            if (countSelectFire >= 0.0f)
            {
                countSelectFire += Time.deltaTime;
            }

            if (countSelectFire >= SelectFireTime)
            {
                
                var rate = SelectFireTime / countSelectFire;
                var r = 1 - rate + 0.5f;  //半分の炎から

                SetLightScaleAndAlpha(r);

                if (!onSound && rate < 0.9f)
                {
                    audioSource.PlayOneShot(selectSE);
                    onSound = true;
                }
            }
        }

        if (selectImage.enabled && Input.GetKeyUp(KeyCode.Return))
        {
            onEnter = true;
            SelectFireTime = 1.2f;
            countSelectFire = 3.0f;

            audi.GetComponent<BGMfade>().IsFadeOut = true;

            for (var i = 0; i < fadeInfo.Count; i++)   //初期化しなおし
            {
                fadeInfo[i].alpha = 0.0f;
                fadeInfo[i].anim /= 2.0f;  //1.5倍に速くしている
            }
        }

        if (onEnter)
        {
            var rate = SelectFireTime / countSelectFire;

            countSelectFire += Time.deltaTime;

            UpdateAlpha(true);
            SetLightScaleAndAlpha(rate);

            countUpGoGame += Time.deltaTime;

            if (countUpGoGame >= timeLimitGoGame)
            {  //カウント
                SceneManager.LoadScene("GameMain");
            }
        }
        else
        {
            UpdateAlpha(false);
        }
    }

    //===============================================
    //選択画像炎の大きさとアルファの操作
    //渡した引数まで大きさとアルファを変化させる   rate = 大きく  1 - rate = 小さく

    void SetLightScaleAndAlpha(float rate)
    {

        var c = selectImage.color;

        rate = Mathf.Clamp(rate, 0.0f, GAUGE_EFFECT_RATE);

        selectImage.color = new Color(c.r, c.g, c.b, rate);  //アルファ
        selectImage.rectTransform.localScale = new Vector3(rate, rate, rate);  //大きさ
    }

    //=======================================================================
    //おばけの動き

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

        //-1〜1の範囲にRENGE_YをかけてRENGE_Yの幅に変換
        var y = Mathf.Cos(angle * Mathf.Deg2Rad) * RENGE_Y * rengeScale;

        rt.anchoredPosition = new Vector2(firstGhost.x + y, firstGhost.y + y);

    }

    //=============================================================================
    //タイトル画像のAlpha操作

    private void UpdateAlpha(bool reverse)
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

                var a = fadeInfo[i].alpha / fadeInfo[i].anim;  //0〜1の割合

                if (!reverse)
                {
                    SetImageAlpha(i, a);
                }
                else
                {
                    SetImageAlpha(i, 1 - a);
                }
            }
        }
    }

    private void SetImageAlpha(int index, float alpha)
    {
        if (index < 0 || index >= colorImage.Count)
        {
            return;
        }

        alpha = Mathf.Clamp(alpha, 0.0f, 1.0f);

        if (!colorImage[index].enabled)
        {
            colorImage[index].enabled = true;
        }

        var c = colorImage[index].color;

        colorImage[index].color = new Color(c.r, c.g, c.b, alpha);
    }
}