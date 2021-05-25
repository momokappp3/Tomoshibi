using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class end : MonoBehaviour
{
    [SerializeField] private Image selectImage;
    [SerializeField] private Image fadeImage;

    [SerializeField] private float anim;   //âΩïbÇ©ÇØÇƒÉøÇ™ìÆÇ≠Ç©
    private float timer = 0.0f;

    private bool onEnter;

    private float countUpInput = 0.0f;  //InputCount
    [SerializeField] private float timeInput = 3.0f;  //InputÇ§ÇØÇ¬ÇØÇ‹Ç≈âΩïbÇ©ÇØÇÈÇ©

    private float countUpFadeOut = 0.0f;
    [SerializeField] private float timeFadeOut = 3.0f;

    [SerializeField] private AudioClip selectSE;
    AudioSource audioSource;

    void Start()
    {
        selectImage.enabled = false;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        UpdateFade(false);
        countUpInput += Time.deltaTime;

        if (!selectImage.enabled && Input.anyKeyDown && countUpInput >= timeInput)
        {
            selectImage.enabled = true;
            audioSource.PlayOneShot(selectSE);
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

        var a = timer / anim;  //0Å`1ÇÃäÑçá

        if (reverse)
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