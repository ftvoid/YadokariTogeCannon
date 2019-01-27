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
    */
    enum SharkState
    {
        Appearance,
        Aiming,
        Ready,
        Charge,
        Stun,
        TurnBack,
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

    //[SerializeField, Header("ピヨピヨ状態の持続時間")]
    //ピヨピヨ状態の持続時間(当たった殻の種類によって変化)
    float stunTimeMax = 0;
    float stunTime;

    [SerializeField, Header("サメの体力")]
    int maxLife = 0;
    int life;


    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.gameObject.GetComponent<HermitClab>();
        life = maxLife;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
                stunTime += Time.deltaTime;
                if(stunTime >= stunTimeMax)
                {
                    sharkState = SharkState.TurnBack;
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
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //プレイヤーに当たった時
        if(collision.gameObject.tag == "Player")
        {
            //サメが突進状態でかつプレイヤーが殻に入っていた場合
            if(playerScript.IsShell() && sharkState == SharkState.Charge)
            {
                //ピヨピヨ状態になる
                stunTimeMax = playerScript.GetStanTime();
                stunTime = 0;
                sharkState = SharkState.Stun;
            }
            //サメが突進状態でかつプレイヤーが殻に入っていなかった場合
            else if(!playerScript.IsShell() && sharkState == SharkState.Charge)
            {
                //ゲームオーバー処理
            }
        }

        //殻と当たった時
        if(collision.gameObject.tag == "Shell")
        {
            Shell shell = collision.gameObject.GetComponent<Shell>();
            Debug.Log("SharkHit");
            //殻が発射されたものなら
            if (shell.IsShot)
            {
                //サメのライフ減少
                life -= (int)shell.GetAttack();
                
                //体力が0になった場合
                if(life <= 0)
                {
                    GameScene.Instance.ShowGameClear();
                }
            }
        }
    }
}
