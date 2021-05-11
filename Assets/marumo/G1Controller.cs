﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G1Controller : MonoBehaviour
{
    enum EneState
    {
        Normal,
        Attack,
        Wait,
        CoolDown,
        Damage
    }

    EneState state = EneState.Normal;
    //　歩くスピード
    public float speed = 20.0f;
    public float noise = 0.3f;
    public float attackSpeed = 3.0f;
    public Texture damText;
    public Texture norText;
    public Material TargetMaterial;

    Vector3 oriPositoin, target, amount;
    //　速度
    private Vector3 velocity;
    //　移動方向
    private Vector3 direction;
    float time = 0.0f;
    float waitTime = 0.0f;
    float countTime = 0.0f;
    float maeTime = 0.0f;
    int[] num = new int[] { -1, 0, 1, 0 };
    int k = 0;
    int xDir = 1;
    int isGood = 0;
    int buruDir = 1;
    GameManager GMmanager;
    GameObject player;
    float distance = 0.0f;
    float distance2 = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        GMmanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GMmanager.GetPlayer();

        TargetMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        distance2 = Mathf.Abs(player.transform.Find("Point Light").transform.position.x - transform.position.x);
        // 攻撃された
        if (distance2 < 4 && player.GetComponent<PlayerController>().state == PlayerController.State.Attack)
        {
            countTime = 0.0f;
            maeTime = 1.5f;
            state = EneState.Damage;
            TargetMaterial.SetTexture("_MainTex", damText);
        }


        if (state == EneState.Attack)
        {
            if (Mathf.Abs(amount.x) > 1.0f)
            {
                transform.position += target * Time.deltaTime * attackSpeed;
                amount -= target * Time.deltaTime * attackSpeed;
            }
            else
            {
                if (isGood == 1)
                {
                    maeTime = 0.5f;
                    state = EneState.CoolDown;
                    return;
                }
                target = oriPositoin - transform.position;
                amount = target;
                isGood = 1;
            }
            return;
        }

        if (state == EneState.Wait)
        {
            maeTime -= Time.deltaTime;
            if (maeTime < 0.0f)
            {
                state = EneState.Attack;
            }
            return;
        }

        if (state == EneState.CoolDown)
        {
            maeTime -= Time.deltaTime;
            if (maeTime < 0.0f)
            {
                TargetMaterial.SetTexture("_MainTex", norText);
                state = EneState.Normal;
            }
            return;
        }
        // ダメージ時の動き
        if (state == EneState.Damage)
        {
            TargetMaterial.color -= new Color(0, 0, 0, 0.005f);

            maeTime -= Time.deltaTime;
            countTime += Time.deltaTime;
            if (countTime > 0.05f)
            {
                countTime = 0.0f;
                buruDir *= -1;

            }
            transform.position += new Vector3(buruDir, 0, 0) * 10 * Time.deltaTime;

            if (maeTime < 0.0f || TargetMaterial.color.a < 0.001f)
            {
                //TargetMaterial.SetTexture("_MainTex", norText);
                //state = EneState.Normal;
                GMmanager.Taos(0);
                Destroy(gameObject);
            }
            return;
        }


        direction = (player.transform.position - transform.position).normalized;
        distance = Mathf.Abs(player.transform.position.x - transform.position.x);
        direction.y = noise*num[k];

        // ある程度近いとき
        if (distance < 11)
        {
            direction.x = 0.0f;
            if (player.GetComponent<PlayerController>().state == PlayerController.State.Normal)
            {
                isGood = 0;
                oriPositoin = transform.position;
                target = player.transform.position - transform.position;
                amount = target;
                state = EneState.Wait;
                maeTime = 0.3f;
            }
        }

        transform.position += direction * speed * Time.deltaTime;
        time += Time.deltaTime;

        if (time >= waitTime)
        {
            time = 0.0f;
            k = (k + 1) % 4;
            waitTime = Mathf.Abs(num[k]) * 0.6f + 0.2f;
        }

        if (xDir * direction.x < 0)
        {
            xDir *= -1;
            transform.localScale = new Vector3(xDir, 1, 1);
        }

        
    }

    // 当たり判定
    private void OnTriggerEnter(Collider other)
    {
        // 攻撃した
        if (other.tag == "Core"
            && state != EneState.Damage
            && player.GetComponent<PlayerController>().state != PlayerController.State.Damage
            && player.GetComponent<PlayerController>().state != PlayerController.State.Muteki)
        {
            player.GetComponent<PlayerController>().Damage();
        }

        // 攻撃された
        if (other.tag == "Attack" && player.GetComponent<PlayerController>().state == PlayerController.State.Attack)
        {
            countTime = 0.0f;
            maeTime = 1.5f;
            state = EneState.Damage;
            TargetMaterial.SetTexture("_MainTex", damText);
        }
    }

}
