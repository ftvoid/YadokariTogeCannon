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

    [SerializeField,Header("１秒間に回転する速度")]
    float rotateSpeed;

    [SerializeField,Header("貝殻を発射するスピード")]
    float shotShellSpeed;


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

    IEnumerator DelayShellHit()
    {
        for(int i = 0;i<10;i++)
        {
            yield return null;
        }

        IsShelled = false;
        yield return null;
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

        Vector3 target_dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (target_dir.magnitude < 0.1)
            return;

        //体の向きを変更
        Quaternion rotation = Quaternion.LookRotation(target_dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);

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

           
            

            Vector3 dir = new Vector3
                (90, Mathf.Atan(transform.forward.x / transform.forward.z) * 180 / Mathf.PI, 0);//弾をそちら側に回転させる
            //shell.transform.Rotate(new Vector3(0, this.transform.eulerAngles.y, 0));
            Vector3 shotPower = Vector3.Normalize(shell.transform.forward) * shotShellSpeed;
            shell.GetComponent<Rigidbody>().AddForce(shotPower);

            ResetShell();
            StartCoroutine(DelayShellHit());

        }
    }

    void TakeOff()
    {
        if (!IsShelled || shell == null)
            return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            ResetShell();
            DelayShellHit();
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

    void ResetShell()
    {
        shell.transform.parent = this.transform.parent;
        shell = null;
    }




    bool IsMove()
    {
        return state == MoveState.Move;
    }
}
