using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clear : MonoBehaviour
{

    [SerializeField] private Image fadeImage;
    [SerializeField] private float anim;   //何秒かけてαが動くか
    private float timer = 0.0f;

    private bool onEnter;

    private float countUpInput = 0.0f;  //InputCount
    [SerializeField] public float timeInput = 3.0f;  //Inputうけつけまで何秒かけるか

    private float countUpFadeOut = 0.0f;
    [SerializeField] public float timeFadeOut = 10.0f;  //エディタ3.0f

    // Update is called once per frame
    void Update()
    {
        countUpInput += Time.deltaTime;
        UpdateFade();

        if (onEnter)
        {
            countUpFadeOut += Time.deltaTime;
            ReverseUpdateFade();
            if (countUpFadeOut >= timeFadeOut)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
            }
        }

        if (countUpInput >= timeInput && Input.anyKeyDown)
        {
            onEnter = true;
        }
    }

    private void UpdateFade()
    {
        timer += Time.deltaTime;

        var a = timer / anim;  //0〜1の割合

        SetImageAlpha(1 - a);
    }

    private void ReverseUpdateFade()
    {
        timer += Time.deltaTime;

        var a = timer / anim;  //0〜1の割合

        SetImageAlpha(a);
    }

    private void SetImageAlpha(float alpha)
    {

        alpha = Mathf.Clamp(alpha, 0.0f, 1.0f);

        if (!fadeImage.enabled)
        {
            fadeImage.enabled = true;
        }

        var c = fadeImage.color;

        fadeImage.color = new Color(c.r, c.g, c.b, alpha);
    }
}