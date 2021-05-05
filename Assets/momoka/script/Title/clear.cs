using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clear : MonoBehaviour
{
    //fadeInOut‚Ìˆ—

    [SerializeField] private Image fadeImage;

    [SerializeField] private float anim;   //‰½•b‚©‚¯‚Äƒ¿‚ª“®‚­‚©
    private float timer = 0.0f;

    private bool onEnter;

    private float countUpInput = 0.0f;  //InputCount
    [SerializeField] public float timeInput = 3.0f;  //Input‚¤‚¯‚Â‚¯‚Ü‚Å‰½•b‚©‚¯‚é‚©

    private float countUpFadeOut = 0.0f;
    [SerializeField] public float timeFadeOut = 3.0f;

    void Start()
    {
        
    }

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

        var a = timer / anim;  //0`1‚ÌŠ„‡

        SetImageAlpha(1 - a);
    }

    private void ReverseUpdateFade()
    {
        timer += Time.deltaTime;

        var a = timer / anim;  //0`1‚ÌŠ„‡

        SetImageAlpha(a);
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

