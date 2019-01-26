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

    [SerializeField, Header("Shell設置用")]
    GameObject shellPos;

    Vector3 myScale = Vector3.zero;

    enum MoveState
    {
        Stop,
        Move
    }

    MoveState state = MoveState.Stop;

    private void Awake()
    {
        IsShelled = false;
        myScale = this.transform.localScale;
    }

    /// <summary>
    /// すぐにShellにあたってくっつかないように
    /// </summary>
    /// <returns></returns>
    IEnumerator DelayShellHit()
    {
        for (int i = 0; i < 2; i++)
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
       // SetShellPostion();
        Shot();
        Shell();
    }

    void Shell()
    {
        if (!IsShelled)
            return;

        if (state == MoveState.Move)
            OpenShell();
        else
            CloseShell();
    }

    /// <summary>
    /// 移動
    /// </summary>
    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (x == 0 && z == 0)
        {
            SoundManager.Instance.StopAllSE();
            state = MoveState.Stop;
            return;
        }
        else
        {
            state = MoveState.Move;
        }
           


        Rotate(x, z);

        //Shellついてない時は早い
        if(!IsShelled)
        {
            //ついてないとき
            this.transform.position += new Vector3(x, 0, z);
            SoundManager.Instance.StopSE("Slow");
            SoundManager.Instance.PlaySE("Run");
        }
        else
        {
            this.transform.position += new Vector3(x, 0, z) * getShellSpeedScale;
            SoundManager.Instance.StopSE("Run");
            SoundManager.Instance.PlaySE("Slow");
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
        
        Vector3 target_dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (target_dir.magnitude < 0.1)
            return;

        //体の向きを変更
        Quaternion rotation = Quaternion.LookRotation(target_dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotateSpeed);

    }


    /// <summary>
    /// 殻にこもる
    /// </summary>
    void CloseShell()
    {
        if (shell == null)
            return;

        shell.GetComponent<ChaseObject>().IsChase = false;
        if(transform.localScale != Vector3.zero)
        myScale = this.transform.localScale;
        Debug.Log(myScale);
        shell.transform.eulerAngles = Vector3.zero;
        this.transform.localScale = Vector3.zero;
       // StartCoroutine(ReductionScale());
    }

    IEnumerator ReductionScale()
    {
        for(float time = 0; time<= 1; time += 0)
        {
            time += 1 * Time.deltaTime;

            this.transform.localScale = Vector3.Lerp(myScale, Vector3.zero, time);
            yield return null;
        }
    }

    void OpenShell()
    {
        StopCoroutine(ReductionScale());
        this.transform.localScale = myScale;
        shell.GetComponent<ChaseObject>().IsChase = true;
        
    }

    /// <summary>
    /// 殻発射
    /// </summary>
    void Shot()
    {
        //殻がなければreturn
        if (!IsShelled || !IsShellExistence())
            return;


        if (Input.GetButtonDown("Fire1"))
        {
            Rigidbody rigid = shell.GetComponent<Rigidbody>();
            rigid.drag = 0;

            //発射エフェクト
            EffectManager.Instance.ShowEffect("Shot", this.transform.position, this.transform.rotation);
            SoundManager.Instance.PlaySE("Shoot");

            this.transform.localScale = myScale;

            Vector3 dir = new Vector3
                (90, Mathf.Atan(transform.forward.x / transform.forward.z) * 180 / Mathf.PI, 0);//弾をそちら側に回転させる
            shell.transform.Rotate(dir);
            Vector3 shotPower = Vector3.Normalize(this.gameObject.transform.forward) * shotShellSpeed;
            shell.GetComponent<Rigidbody>().velocity = shotPower;

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

        if (Input.GetButtonDown("Fire2"))
        {
            //脱ぐエフェクト
            EffectManager.Instance.ShowEffect("Out", this.transform.position, this.transform.rotation);
            SoundManager.Instance.PlaySE("Out");

            this.transform.position += this.transform.forward * 5f;
            this.transform.localScale = myScale;
            ResetShell();
            StartCoroutine(DelayShellHit());
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

        //ぶつかったオブジェクトが餌、殻、敵じゃなければreturn

        //殻にぶつかったとき
        if (colTag == "Shell")
        {
            //殻持ってたらreturn
            if (IsShelled)
                return;

            //殻に入るエフェクト
            EffectManager.Instance.ShowEffect("In", this.transform.position, this.transform.rotation);
            SoundManager.Instance.PlaySE("In");
            shell = col.gameObject;
            shell.GetComponent<ChaseObject>().target = this.gameObject;
            //shell.transform.localPosition = Vector3.zero + new Vector3(0,-0.03f,-0.2f);
            //shell.transform.eulerAngles = new Vector3(-22f, 0);
            //col.gameObject.transform.parent = shellPos.transform;
            IsShelled = true;
        }

        //餌にぶつかったとき
        if (col.gameObject.tag == "Food")
        {
            //餌情報を取得する
            Food food = col.gameObject.GetComponent<Food>();
            //育成度と満腹度をプラスする
            StateManager.Instance.AddGrowth(food.GetIncreasGrowth());
            StateManager.Instance.AddSatiety(food.GetIncreasSatiety());
            //ご飯食べたエフェクト
            EffectManager.Instance.ShowEffect("Eat",this.transform.position,this.transform.rotation);
            SoundManager.Instance.PlaySE("Eat");
            this.transform.localScale += new Vector3(1, 1, 1) * sizeScaler;

            FoodManager.Instance.DeleteFood(col.gameObject.GetComponent<Food>());
        }


        //敵にぶつかったとき
        if (col.gameObject.tag == "Clab")
        {
            ////殻を持っていて、動いていなければreturn
            //if (IsShelled && !IsMove())
            //    return;

            //そうでなければ死ぬ
            Dead();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //殻にぶつかったとき
        if (other.tag == "Shell")
        {
            //殻持ってたらreturn
            if (IsShelled)
                return;
            
            shell = other.gameObject;
            shell.GetComponent<ChaseObject>().target = shellPos;
            shell.transform.localScale = this.transform.localScale * 1.2f;
            //shell.transform.localPosition = Vector3.zero + new Vector3(0,-0.03f,-0.2f);
            //shell.transform.eulerAngles = new Vector3(-22f, 0);
            //col.gameObject.transform.parent = shellPos.transform;
            IsShelled = true;
        }

    }

    /// <summary>
    /// Shellのリセット
    /// </summary>
    void ResetShell()
    {
        shell.transform.parent = this.transform.parent;
        shell.GetComponent<ChaseObject>().target = null;
        shell = null;
    }

    /// <summary>
    /// 移動してる？
    /// </summary>
    ///  /// <returns>移動中ならtrue</returns>
    public bool IsMove()
    {
        return state == MoveState.Move;
    }

    bool IsShellExistence()
    {
        return shell != null;
    }

    /// <summary>
    /// 殻を持っている？
    /// </summary>
    /// <returns>持ってたらtrue</returns>
    public bool IsShell()
    {
        return IsShelled;
    }
}
