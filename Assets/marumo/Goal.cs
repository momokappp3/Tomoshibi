using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    GameManager GM;

    // Start is called before the first frame update
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // �����蔻��
    private void OnTriggerEnter(Collider other)
    {
        // �U������
        if (other.tag == "Player")
        {
            GM.Clear();
        }
    }

}
