using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rigid;
    public float jumpForce = 230.0f;
    public float walkForce = 10.0f;
    public float maxWalkSpeed = 7.0f;
    float time = 0.0f;

    int k = 1;

    public Texture[] myTexts = new Texture[4];
    public Material TargetMaterial;


    // Start is called before the first frame update
    void Start()
    {
        this.rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.rigid.AddForce(new Vector3(0, 1, 0) * this.jumpForce);
        }

        int key = 0;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) key = -1;
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) key = 1;

        float speedx = Mathf.Abs(this.rigid.velocity.x);

        if (speedx < this.maxWalkSpeed)
        {
            this.rigid.AddForce(transform.right * key * this.walkForce);
        }

        if(key != 0)
        {
            transform.localScale = new Vector3(key * 2f, 2f, 2f);
        }

        if (speedx > 0)
        {
            time += Time.deltaTime;
            if (time > 2.0f / speedx)
            {
                time = 0.0f;
                TargetMaterial.SetTexture("_MainTex", myTexts[k]);
                k = (k + 1) % 4;
            }
        }

    }
}
