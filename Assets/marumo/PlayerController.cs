using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum State
    {
        Normal,
        Attack,
        Damage,
        Muteki
    }

    public State state = State.Normal;
    public float walkForce = 15.0f;
    public float maxWalkSpeed = 7.0f;
    public Texture[] myTexts = new Texture[4];
    public Texture[] AttackTexts = new Texture[3];
    public Texture[] DamageTexts = new Texture[3];
    public Material TargetMaterial;
    public SpriteRenderer sp;

    Rigidbody rigid;
    float jumpForce = 230.0f;
    float time = 0.0f;
    float tt = 0.0f;
    GameObject myLight;
    int k = 1;
    int l = 0;
    int m = 0;
    int count = 0;
    GameManager GMmanager;
    Vector3[] lightLocations = new Vector3[] {new Vector3(2.7f, 1.0f, 1.4f),
        new Vector3(3.2f, 1.0f, 1.2f),
        new Vector3(2.7f, 1.0f, 1.4f),
        new Vector3(1.9f, 1.0f, 1.2f) };
    Vector3[] redLightLocations = new Vector3[] {new Vector3(2.6f, 0.9f, 1.5f),
        new Vector3(2.2f, 0.8f, 1.0f),
        new Vector3(2.8f, 0.8f, -1.0f) };


    // Start is called before the first frame update
    void Start()
    {
        this.rigid = GetComponent<Rigidbody>();
        this.myLight = transform.Find("Point Light").gameObject;
        this.GMmanager = GameObject.Find("GameManager").GetComponent<GameManager>();
        TargetMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        // 攻撃時の動き
        if (state == State.Attack) {
            myLight.GetComponent<Light>().color = Color.red;
            myLight.GetComponent<Light>().range = 60;
            time += Time.deltaTime;
            if (time > 0.13f)
            {
                if (l == 2)
                {
                    if (time > 0.2f)
                    {
                        myLight.GetComponent<Light>().color = Color.white;
                        myLight.GetComponent<Light>().range = 10;
                        TargetMaterial.SetTexture("_MainTex", myTexts[k]);
                        state = State.Normal;
                    }
                    return;
                }
                l += 1;
                time = 0.0f;
                TargetMaterial.SetTexture("_MainTex", AttackTexts[l]);
                myLight.transform.localPosition = redLightLocations[l];
            }
            return;
        }

        // ダメージ時の動き
        if (state == State.Damage)
        {
            time += Time.deltaTime;
            if (time > 0.15f)
            {
                if (m == 2)
                {
                    if (time > 0.3f)
                    {
                        myLight.GetComponent<Light>().color = Color.white;
                        TargetMaterial.SetTexture("_MainTex", myTexts[k]);
                        tt = 0.0f;
                        count = 0;
                        state = State.Muteki;
                        TargetMaterial.color = new Color(1f, 1f, 1f, 1f);
                    }
                    return;
                }
                m += 1;
                time = 0.0f;
                TargetMaterial.SetTexture("_MainTex", DamageTexts[l]);
            }


            return;
        }

        if (state == State.Muteki)
        {
            tt += Time.deltaTime;
            int x = 0;


            if (tt > 0.15f && 0.3f > tt)
            {
                x = 1;
                tt = 0.3f;
                TargetMaterial.color -= new Color(0, 0, 0, 255);
                count += 1;
            }
            if (tt > 0.45f)
            {
                x = 2;
                tt = 0.0f;
                TargetMaterial.color += new Color(0, 0, 0, 255);
                count += 1;
            }

            if (count > 5)
            {
                if (x == 1) TargetMaterial.color += new Color(0, 0, 0, 255);
                state = State.Normal;
            }

        }


        //// ジャンプ
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    this.rigid.AddForce(new Vector3(0, 1, 0) * this.jumpForce);
        //}
        // 左右
        int key = 0;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) key = -1;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) key = 1;
        // 攻撃入力
        if (Input.GetMouseButtonDown(0) && state != State.Muteki)
        {
            this.rigid.velocity = Vector3.zero;

            l = 0;
            TargetMaterial.SetTexture("_MainTex", AttackTexts[l]);
            state = State.Attack;
            time = 0.0f;
            return;
        }

        float speedx = Mathf.Abs(this.rigid.velocity.x);

        if (speedx < this.maxWalkSpeed)
        {
            this.rigid.AddForce(transform.right * key * this.walkForce);
        }

        if(key != 0)
        {
            transform.localScale = new Vector3(key*2, 2, 2);
        }

        if (speedx > 0)
        {
            time += Time.deltaTime;
            if (time > 3 / speedx)
            {
                time = 0.0f;
                TargetMaterial.SetTexture("_MainTex", myTexts[k]);
                myLight.transform.localPosition = lightLocations[k];
                k = (k + 1) % 4;
            }
        }
    }

    public void Damage()
    {
        GMmanager.Damage();
        m = 0;
        TargetMaterial.SetTexture("_MainTex", DamageTexts[l]);
        state = State.Damage;
        time = 0.0f;
        return;
    }
}


