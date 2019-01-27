using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shark : MonoBehaviour
{
    /*サメの行動パターンの段階
        0→出現
        1→フィールド端からタイミングをうかがっている状態(ヤドカリと軸を合わせてくる)
        2→突進の準備
        3→突進
        4→ピヨピヨ状態
        5→ピヨピヨが治って元の位置に戻る状態
        6→ピヨピヨ状態時にダメージを受けた時のノックバック
    */
    enum SharkState
    {
        Appearance,
        Aiming,
        Ready,
        Charge,
        Stun,
        TurnBack,
        KnockBack,
    }

    SharkState sharkState = SharkState.Appearance;

    //追跡用のプレイヤー
    Transform player;
    HermitClab playerScript;

    //-------------サメの行動関係の値-------------
    [SerializeField, Header("サメの待機時のX座標")]
    float waitXPosition = 0;

    [SerializeField, Header("サメの待機時間")]
    float waitTimerMax = 0;
    float waitTimer = 0;

    [SerializeField, Header("サメ待機時、プレイヤーを追従する速度")]
    float waitMoveSpeed = 0;

    [SerializeField, Header("サメ待機終了から突撃になる間、サメの引く距離")]
    float chargeReadyRange = 0;
    float chargeReadyTimer = 0;

    [SerializeField, Header("サメ突撃時、プレイヤーを追従する速度")]
    float chargeMoveSpeed = 0;

    [SerializeField, Header("サメがピヨピヨ状態になる時のノックバックの長さ")]
    int stunKnockFrame = 0;
    //2つのノックバックで共通で使用
    int knockFrame = 0;
    //ピヨピヨ状態になるときのノックバックの力、プレイヤーの殻のダメージ量で変化
    float knockPower = 1;

    [SerializeField, Header("サメがピヨピヨ状態時に殻を受けた時のノックバックの長さ")]
    int damageKnockFrame = 0;

    //[SerializeField, Header("ピヨピヨ状態の持続時間")]
    //ピヨピヨ状態の持続時間(当たった殻の種類によって変化)
    float stunTimeMax = 0;
    float stunTime;

    [SerializeField, Header("サメの体力")]
    int maxLife = 0;
    int life;

    //体力が0になったフラグ
    bool endFlag = false;
    //撃破演出用
    int endFrame = 0;

    //パーティクルの量調整用
    int particleDelay = 0;

    //マテリアル変更用
    SkinnedMeshRenderer meshRenderer;

    [SerializeField, Header("サメのマテリアル")]
    Material[] sharkMaterials;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.gameObject.GetComponent<HermitClab>();
        GameObject walk = transform.GetChild(0).gameObject;
        meshRenderer = walk.GetComponentInChildren<SkinnedMeshRenderer>();
        life = maxLife;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //撃破時演出
        if(endFlag)
        {
            particleDelay++;
            if (particleDelay >= 2)
            {
                EffectManager.Instance.ShowEffect("Levelup", new Vector3(transform.position.x + Random.Range(-30f, 30f), Random.Range(7f, 13f), transform.position.z + Random.Range(-15f, 15f)), transform.rotation);
                particleDelay = 0;
            }
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z);
            if(transform.position.y < -19f)
            {
                Destroy(this);
                GameScene.Instance.ShowGameClear();
                
            }
            return;
        }

        if(player == null)
        {
            return;
        }

        //サメの移動
        switch(sharkState)
        {
            //---------------登場時---------------
            case SharkState.Appearance :
                transform.position = new Vector3(transform.position.x - 1,transform.position.y,transform.position.z);
                //待機座標に到達した場合、フィールド端からプレイヤーを追従する状態に変更
                if(transform.position.x <= waitXPosition)
                {
                    waitTimer = 0;
                    sharkState = SharkState.Aiming;
                }
                break;

            //---------------フィールド端---------------
            case SharkState.Aiming :
                if(player.position.z > transform.position.z + waitMoveSpeed)
                {
                    transform.position = new Vector3(waitXPosition, 17, transform.position.z + waitMoveSpeed);
                }
                else if(player.position.z < transform.position.z - waitMoveSpeed)
                {
                    transform.position = new Vector3(waitXPosition, 17, transform.position.z - waitMoveSpeed);
                }
                else
                {
                    transform.position = new Vector3(waitXPosition, 17, transform.position.z);
                }
                waitTimer += Time.deltaTime;
                //待機時間が終了した時、突進準備段階に移行
                if(waitTimer >= waitTimerMax)
                {
                    sharkState = SharkState.Ready;
                }
                break;

            //---------------突進準備---------------
            case SharkState.Ready : 
                if(transform.position.x < waitXPosition + chargeReadyRange)
                {
                    transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z) ;
                    transform.LookAt(player);
                }
                else
                {
                    //指定ポジションまで引いた場合
                    chargeReadyTimer += Time.deltaTime;
                    if(chargeReadyTimer >= 1)
                    {
                        chargeReadyTimer = 0;
                        sharkState = SharkState.Charge;
                    }
                }
                break;

            //---------------突進---------------
            case SharkState.Charge:
                transform.LookAt(player);
                transform.position += transform.forward * chargeMoveSpeed;

                break;

            //---------------ピヨピヨ状態---------------
            case SharkState.Stun:
                if(knockFrame <= stunKnockFrame)
                {
                    knockFrame++;
                    transform.position -= transform.forward * 1.3f;
                }
                stunTime += Time.deltaTime;
                if(stunTime >= stunTimeMax)
                {
                    sharkState = SharkState.TurnBack;
                }

                particleDelay++;
                if(particleDelay >= 5)
                {
                    EffectManager.Instance.ShowEffect("StunTime", transform.position + 
                                                     (transform.forward * Random.Range(8f,24f)) + 
                                                     ( transform.up * Random.Range(7f,11f)) +
                                                     ( transform.right * Random.Range(-7f,7f)), transform.rotation);
                    particleDelay = 0;
                }

                break;

            //---------------ピヨピヨ状態から復帰---------------
            case SharkState.TurnBack:
                if (transform.position.y < 17f)
                {
                    transform.position = new Vector3(transform.position.x + 0.7f, transform.position.y + 0.2f, transform.position.z);

                }
                else
                {
                    transform.position = new Vector3(transform.position.x + 0.7f, transform.position.y, transform.position.z);
                }
                transform.rotation = Quaternion.Euler(0, 270, 0);
                if (transform.position.x >= waitXPosition)
                {
                    waitTimer = 0;
                    sharkState = SharkState.Aiming;
                }

                break;

            //---------------ノックバック---------------
            case SharkState.KnockBack:
                if (knockFrame <= damageKnockFrame)
                {
                    knockFrame++;
                    transform.position -= transform.forward * 1.3f;
                }
                else
                {
                    sharkState = SharkState.Stun;
                }

                break;
        }

        //サメのマテリアル変更
        if(sharkState == SharkState.Stun && meshRenderer.material != sharkMaterials[1])
        {
            meshRenderer.material = sharkMaterials[1];
        }
        else if(sharkState != SharkState.Stun && meshRenderer.material != sharkMaterials[0])
        {
            meshRenderer.material = sharkMaterials[0];
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //プレイヤーに当たった時
        if (collision.gameObject.tag == "Player" && sharkState == SharkState.Charge)
        {
            //サメが突進状態でかつプレイヤーが殻に入っていてかつプレイヤーが停止中の場合
            if (playerScript.IsShell() &&  !playerScript.IsMove())
            {
                //ピヨピヨ状態になる
                EffectManager.Instance.ShowEffect("StunHit", transform.position + (transform.forward * 13f), transform.rotation);
                stunTimeMax = playerScript.GetStanTime();
                stunTime = 0;
                knockFrame = 0;
                knockPower = 1.2f + (playerScript.GetStanTime() / 10f);
                sharkState = SharkState.Stun;
            }
            //サメが突進状態でかつプレイヤーが殻に入っていてかつプレイヤーが移動中の場合
            else if(playerScript.IsShell() &&  playerScript.IsMove())
            { 
                sharkState = SharkState.TurnBack;
            }
            //サメが突進状態でかつプレイヤーが殻に入っていなかった場合
            else if (!playerScript.IsShell())
            {
                //ゲームオーバー処理
                sharkState = SharkState.TurnBack;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        //殻と当たった時
        if(other.gameObject.tag == "Shell")
        {
            Shell shell = other.gameObject.GetComponent<Shell>();
            //殻が発射されたものなら
            if (shell.IsShot)
            {
                Debug.Log("sharkHit");
                EffectManager.Instance.ShowEffect("Levelup", transform.position, this.transform.rotation);

                //サメのライフ減少
                life -= (int)shell.GetAttack();

                //ピヨピヨ状態であればノックバック処理に移行
                if(sharkState == SharkState.Stun)
                {
                    transform.LookAt(player);
                    knockFrame = 0;
                    sharkState = SharkState.KnockBack;
                }
                
                //体力が0になった場合
                if(life <= 0)
                {
                    gameObject.GetComponent<BoxCollider>().enabled = false;
                    endFlag = true;
                    //GameScene.Instance.ShowGameClear();
                }
            }
        }
    }

    /// <summary>
    /// サメの撃破演出が始まっていたらTrue
    /// </summary>
    /// <returns></returns>
    public bool SharkDied()
    {
        return endFlag;
    }
}
