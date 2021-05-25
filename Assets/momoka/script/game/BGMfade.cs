using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMfade : MonoBehaviour
{
    public AudioClip sound1;
    AudioSource audioSource;

    bool IsFadeIn = true;
    public bool IsFadeOut = false;  //IsFadeOut‚ðtrue‚É‚µ‚Ä‚­‚¾‚³‚¢
    float FadeInCountTime = 0.0f;
    float FadeOutCountTime = 0.0f;

    [SerializeField] private float FadeInSeconds = 15.0f;
    [SerializeField] private float FadeOutSeconds = 5.0f;

    void Start()
    {
        //Component‚ðŽæ“¾
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (IsFadeIn)
        {
            FadeInCountTime += Time.deltaTime;
            audioSource.volume = (float)(FadeInCountTime / FadeInSeconds);

            if (FadeInCountTime >= FadeInSeconds)
            {
                IsFadeIn = false;
            }
        }

        if (IsFadeOut)
        {
            FadeOutCountTime += Time.deltaTime;
            audioSource.volume = 1 - (float)(FadeOutCountTime / FadeOutSeconds);
        }
    }
}