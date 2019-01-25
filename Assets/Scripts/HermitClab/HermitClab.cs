using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermitClab : MonoBehaviour
{


    /// <summary>
    /// 殻をかぶってたらtrue
    /// </summary>
    public bool IsShelled;

    /// <summary>
    /// 殻
    /// </summary>
    GameObject shell;

    [SerializeField]
    float rotateSpeed;

    [SerializeField]
    float shellSpeed;

    float rotateMargin = 0.9f;


    enum MoveState
    {
        Stop,
        Move
    }

    MoveState state = MoveState.Stop;

    private void Awake()
    {
        IsShelled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        TakeOff();
        Shot();
    }

    /// <summary>
    /// 移動
    /// </summary>
    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Rotate(x, z);

        this.transform.position += new Vector3(x, 0, z);
    }

    void Rotate(float x, float y)
    {
        if (x == 0 && y == 0)
            return;
        
        float step = rotateSpeed * Time.deltaTime;
        float rotate = 0;


        if (x >= rotateMargin)
        {
            rotate = 90f;
        }
        else if(x <= -rotateMargin)
        {
            rotate = -90f;
        }
        else if ( y >= rotateMargin)
        {
            rotate = 0;
        }
        else if(y <= -rotateMargin)
        {
            rotate = 180;
        }

        transform.rotation = Quaternion.RotateTowards
            (transform.rotation,
            Quaternion.Euler(0, rotate, 0),
            step);


    }

    /// <summary>
    /// 殻発射
    /// </summary>
    void Shot()
    {
        //殻がなければreturn
        if (!IsShelled)
            return;


        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rigidbody rigid = shell.GetComponent<Rigidbody>();
            rigid.drag = 0;

            shell.transform.parent = transform.parent;

            Vector3 dir = new Vector3
                (90, Mathf.Atan(transform.forward.x / transform.forward.z) * 180 / Mathf.PI, 0);//弾をそちら側に回転させる
            shell.transform.Rotate(new Vector3(0, this.transform.eulerAngles.y, 0));
            Vector3 shotPower = Vector3.Normalize(shell.transform.forward) * shellSpeed;
            shell.GetComponent<Rigidbody>().AddForce(shotPower);

            IsShelled = false;

        }
    }

    void TakeOff()
    {
        if (!IsShelled)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            shell.transform.parent = transform.parent;
            IsShelled = false;
        }
    }

    void Dead()
    {

    }


    void OnCollisionEnter(Collision col)
    {
        string colTag = col.gameObject.tag;
        Debug.Log(colTag);

        //ぶつかったオブジェクトが餌、殻、敵じゃなければreturn

        //殻にぶつかったとき
        if (colTag == "Shell")
        {
            //殻持ってたらreturn
            if (IsShelled)
                return;

            shell = col.gameObject;
            col.gameObject.transform.parent = this.transform;
            IsShelled = true;
        }

        //餌にぶつかったとき
        if (col.gameObject.tag == "Esa")
        {
            //餌情報を取得する
            Food food = col.gameObject.GetComponent<Food>();
            //育成度と満腹度をプラスする
            StateManager.Instance.AddGrowth(food.GetIncreasGrowth());
            StateManager.Instance.AddSatiety(food.GetIncreasSatiety());
        }


        //敵にぶつかったとき
        if (col.gameObject.tag == "enemy")
        {
            //殻を持っていて、動いていなければreturn
            if (IsShelled && !IsMove())
                return;

            //そうでなければ死ぬ
            Dead();
        }
    }


    bool IsMove()
    {
        return state == MoveState.Move;
    }
}
