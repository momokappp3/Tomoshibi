using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class end : MonoBehaviour
{
    [SerializeField] private Image selectImage;
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
        selectImage.enabled = false;
    }

    void Update()
    {
        UpdateFade(false);
        countUpInput += Time.deltaTime;

        if (!selectImage.enabled && Input.anyKeyDown && countUpInput >= timeInput)
        {
            selectImage.enabled = true;
        }

        if (selectImage.enabled && Input.GetKey(KeyCode.Return))
        {
            onEnter = true;
        }

        if (onEnter)
        {
            countUpFadeOut += Time.deltaTime;
            UpdateFade(true);
            if (countUpFadeOut >= timeFadeOut)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
            }
        }

    }

    private void UpdateFade(bool reverse)
    {
        timer += Time.deltaTime;

        var a = timer / anim;  //0`1‚ÌŠ„‡

        if (reverse)  //O€‰‰Zq‚Å‚«‚È‚©‚Á‚½R(`„DL)É‚È‚ñ‚Å‚â
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

        alpha = Mathf.Clamp(alpha, 0.0f, 1.0f);

        if (!fadeImage.enabled)
        {
            fadeImage.enabled = true;
        }

        var c = fadeImage.color;

        fadeImage.color = new Color(c.r, c.g, c.b, alpha);
    }
}