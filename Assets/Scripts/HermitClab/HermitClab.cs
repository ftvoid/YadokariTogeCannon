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

    [SerializeField, Header("１秒間に回転する速度")]
    float rotateSpeed = 10f;

    [SerializeField, Header("貝殻を発射するスピード")]
    float shotShellSpeed = 3000f;

    [SerializeField, Header("殻を持ってるときの速度係数")]
    float getShellSpeedScale = 0.3f;

    [SerializeField, Header("殻持ってるときに減る満腹度の量")]
    float getShellSatietyScale = 0.5f;

    [SerializeField, Header("どのぐらいずつ大きくなるか")]
    float sizeScaler = 0.1f;

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

    /// <summary>
    /// すぐにShellにあたってくっつかないように
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayShellHit()
    {
        for (int i = 0; i < 5; i++)
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

        //Shellついてない時は早い
        if(!IsShelled)
            this.transform.position += new Vector3(x, 0, z);
        else
        {
            this.transform.position += new Vector3(x, 0, z) * getShellSpeedScale;
            StateManager.Instance.AddSatiety(getShellSatietyScale);
        }
            
    }

    /// <summary>
    /// プレイヤーの回転
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
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
        if (!IsShelled || !IsShellExistence())
            return;


        if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("Fire1"))
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

    /// <summary>
    /// 脱ぐ！
    /// </summary>
    void TakeOff()
    {
        if (!IsShelled || !IsShellExistence())
            return;

        if (Input.GetKeyDown(KeyCode.E) || Input.GetButtonDown("Fire2"))
        {
            ResetShell();
            DelayShellHit();
        }
    }

    //死亡
    void Dead()
    {
        Destroy(this.gameObject);
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
            shell.transform.position = Vector3.zero + new Vector3(0,1,0);
            col.gameObject.transform.parent = this.transform;
            IsShelled = true;
        }

        //餌にぶつかったとき
        if (col.gameObject.tag == "Food")
        {
            //餌情報を取得する
            Food food = col.gameObject.GetComponent<Food>();
            Debug.Log(StateManager.Instance);
            //育成度と満腹度をプラスする
            StateManager.Instance.AddGrowth(food.GetIncreasGrowth());
            StateManager.Instance.AddSatiety(food.GetIncreasSatiety());

            this.transform.localScale += new Vector3(1, 1, 1) * sizeScaler;

            FoodManager.Instance.DeleteFood(col.gameObject.GetComponent<Food>());
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

    /// <summary>
    /// Shellのリセット
    /// </summary>
    void ResetShell()
    {
        shell.transform.parent = this.transform.parent;
        shell = null;
    }

    /// <summary>
    /// 移動できる？
    /// </summary>
    bool IsMove()
    {
        return state == MoveState.Move;
    }

    bool IsShellExistence()
    {
        return shell != null;
    }
}
