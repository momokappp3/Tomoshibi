using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audi : MonoBehaviour
{

    public AudioClip sound1;
    AudioSource audioSource;

    void Start()
    {
        //Component���擾
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // ��
        if (Input.GetMouseButtonDown(0))
        {
            //��(sound1)��炷
            audioSource.PlayOneShot(sound1);
        }
    }
}