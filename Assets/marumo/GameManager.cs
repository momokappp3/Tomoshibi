using System.Collections;
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
    int hp = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetPlayer()
    {
        return player;
    }

    public void Damage()
    {
        gauge.GetComponent<lightGauge>().decLight(200);
        hp -= 1;
        if (hp == 0)
        {
            SceneManager.LoadScene("GameOver");
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
}
