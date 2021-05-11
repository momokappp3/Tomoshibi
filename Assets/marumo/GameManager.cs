﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject gauge;
    public Text[] Texts = new Text[4];

    int[] scores = new int[] {0, 0, 0, 0 };
    //int hp = 5;

    //momoka
    public GameObject audi;

    [SerializeField] private float anim;   //何秒かけてαが動くか
    private float timer = 0.0f;

    [SerializeField] private Image fadeImage;
    [SerializeField] private float timeFadeIn = 3.0f;
    [SerializeField] private float timeFadeOut = 1.0f;
    private float countUpFadeIn = 0.0f;
    private float countUpFadeOut = 0.0f;

    private bool goGameOver = false;

    void Start(){   
    }

    void Update()
    {
        UpdateFade(false);

        if (goGameOver)
        {
            countUpFadeOut += Time.deltaTime;
            UpdateFade(true);

            if (countUpFadeOut >= timeFadeOut)
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public void Damage()
    {
        gauge.GetComponent<lightGauge>().decLight(500);

        if (!goGameOver && gauge.GetComponent<lightGauge>().noLight)  //0だとゲームオーバーに行かなかった  炎の表示にずれが出てる
        {
            goGameOver = true;
            timer = 0.0f;
            audi.GetComponent<Audi>().IsFadeOut = true;
        }
    }

    public void Taos(int num)
    {
        scores[num] += 1;
        Texts[num].GetComponent<Text>().text = scores[num].ToString();
    }

    public void Clear()
    {
        Debug.Log("go");
        SceneManager.LoadScene("GameClear");
    }

    private void UpdateFade(bool reverse)
    {
        timer += Time.deltaTime;

        var a = timer / anim;  //0～1の割合

        if (reverse)  //三項演算子できなかったヽ(`Д´)ﾉなんでや
        {
            SetImageAlpha(a);
        }
        else
        {
            SetImageAlpha(1 - a);
        }
    }

    private void SetImageAlpha(float alpha)
    {
        if (alpha > 1.0f)
        {
            alpha = 1.0f;
        }

        if (alpha < 0.0f)
        {
            alpha = 0.0f;
        }

        if (!fadeImage.enabled)
        {
            fadeImage.enabled = true;
        }

        var c = fadeImage.color;

        fadeImage.color = new Color(c.r, c.g, c.b, alpha);
    }
}