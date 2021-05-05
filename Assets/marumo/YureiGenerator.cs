using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YureiGenerator : MonoBehaviour
{
    public GameObject[] gameObjects = new GameObject[4];
    public float coolTime = 4.0f;

    GameManager GMmanager;
    GameObject player;
    float time = 0.0f;
    int k;
    Vector3[] yureiPositions = new Vector3[] {
        new Vector3(-65, 0, 0.1f),
        new Vector3(65, 0, 0.1f),
        new Vector3(-65, 17, 0.1f),
        new Vector3(65, 17, 0.1f),
        new Vector3(-65, -5, 0.1f), 
        new Vector3(65, -5, 0.1f),
        new Vector3(-65, -5, 0.1f),
        new Vector3(65, -5, 0.1f)
    };

    // Start is called before the first frame update
    void Start()
    {
        k = Random.Range(0, 3);
        coolTime = Random.Range(4, 6);
        GMmanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GMmanager.GetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > coolTime)
        {
            time = 0.0f;
            k = Random.Range(0, 4);
            coolTime = Random.Range(8, 12);

            int i = Random.Range(0, 2);
            GameObject instance = Instantiate(gameObjects[k]);
            instance.transform.position = player.transform.position + yureiPositions[k * 2 + i];
        }
    }
}
