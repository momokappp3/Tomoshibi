using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G3Controller : MonoBehaviour
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
    //�@�����X�s�[�h
    public float speed = 20.0f;
    public float noise = 0.3f;
    public float attackSpeed = 3.0f;
    public Texture damText;
    public Texture norText;
    public Material TargetMaterial;


    Vector3 oriPositoin, target, amount;
    //�@���x
    private Vector3 velocity;
    //�@�ړ�����
    private Vector3 direction;
    float time = 0.0f;
    float scaleTime = 0.0f;
    public float waitTime = 2.0f;
    float waitScaleTime = 1.2f;
    float jumpTime = 0.0f;
    float countTime = 0.0f;
    float maeTime = 0.0f;
    float scaleBai = 0.13f;
    int k = 0;
    public float jumpTo = 0.8f;
    int xDir = 1;
    int isGood = 0;
    int buruDir = 1;
    GameManager GMmanager;
    GameObject player;
    float distance = 0.0f;

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
        transform.localScale = transform.localScale + Vector3.one * scaleBai * Time.deltaTime;
        scaleTime += Time.deltaTime;
        if (scaleTime >= waitScaleTime)
        {
            scaleTime = 0.0f;
            k = (k + 1) % 2;
            scaleBai *= -1;
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
        // �_���[�W���̓���
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
            if (maeTime < 0.0f)
            {
                //TargetMaterial.SetTexture("_MainTex", norText);
                //state = EneState.Normal;
                GMmanager.Taos(2);
                Destroy(gameObject);
            }
            return;
        }


        direction = (player.transform.position - transform.position).normalized;
        distance = Mathf.Abs(player.transform.position.x - transform.position.x);

        // ������x�߂��Ƃ�
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

        // �����̈ړ�
        direction.y = jumpTo;
        transform.position += direction * speed * Time.deltaTime;
        time += Time.deltaTime;
        jumpTime += Time.deltaTime;
        if (jumpTime >= waitTime/2)
        {
            jumpTime = 0.0f;
            jumpTo *= -1;

        }

        if (time >= waitTime)
        {
            time = 0.0f;
            maeTime = 1.0f;
            state = EneState.CoolDown;
        }

        if (xDir * direction.x < 0)
        {
            xDir *= -1;
            transform.localScale = new Vector3(xDir, 1, 1);
        }
    }

    // �����蔻��
    private void OnTriggerEnter(Collider other)
    {
        // �U������
        if (other.tag == "Core" && state != EneState.Damage && player.GetComponent<PlayerController>().state == PlayerController.State.Normal)
        {
            player.GetComponent<PlayerController>().Damage();
        }
        // �U�����ꂽ
        if (other.tag == "Attack" && player.GetComponent<PlayerController>().state == PlayerController.State.Attack)
        {
            countTime = 0.0f;
            maeTime = 1.5f;
            state = EneState.Damage;
            TargetMaterial.SetTexture("_MainTex", damText);
        }
    }
}
