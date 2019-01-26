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
        Down,
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

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = player.gameObject.GetComponent<HermitClab>();
    }

    // Update is called once per frame
    void Update()
    {
        //サメの移動
        switch(sharkState)
        {
            //---------------登場時---------------
            case SharkState.Appearance :
                transform.Translate(0, 0, 1);
                //待機座標に到達した場合、フィールド端からプレイヤーを追従する状態に変更
                if(transform.position.x <= waitXPosition)
                {
                    waitTimer = waitTimerMax;
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
                waitTimer -= Time.deltaTime;
                //待機時間が終了した時、突進準備段階に移行
                if(waitTimer <= 0)
                {
                    sharkState = SharkState.Ready;
                }
                break;

            //---------------突進準備---------------
            case SharkState.Ready : 
                if(transform.position.x < waitXPosition + chargeReadyRange)
                {
                    transform.Translate(0, 0, -0.2f);
                    transform.LookAt(player);
                }
                else
                {
                    //指定ポジションまで引いた場合
                    //chargeReadyTimer += Time.deltaTime
                }

                break;
        }
    }

}
