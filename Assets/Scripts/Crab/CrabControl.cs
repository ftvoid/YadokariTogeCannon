using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabControl : MonoBehaviour
{
    // Hp
    public int MaxHp = 1;
    private int Hp;

    // State
    enum CrabState
    {
        StateWait,
        StateMove,
        StateDiscovery,
        StateQuickMove,
        StateDied,
    };
    private CrabState state;

    // wait control
    public float WaitCounterMax = 2;
    private float WaitCounter;

    // move control
    private bool isDecideDirection;
    public float MoveCounterMax = 2;
    private float MoveCounter;
    private Vector3 MoveDirection;
    public float MoveSpeed = 2.0f;
    public float MoveQuickSpeed = 100.0f;
    public float FieldWidth = 150.0f;
    public float FieldReturnWidth = 30.0f;

    // player check
    private GameObject ObjPlayer;
    private bool isFoundPlayer = false;
    private bool isFoundBodyPlayer = false;

    // Died
    private bool isDied = false;
    public float CounterOfDiedMax = 1.0f;
    private float CounterOfDied;
    private bool isRender;
    public int blinkCounterMax;
    private int blinkCounter;
    private GameObject crabManager;
    public float JumpHeightMax = 100.0f;
    private float JumpHeight;
    public float JumpRotationMax = 30.0f;
    private Vector3 JumpRotation;

    // Start is called before the first frame update
    void Start()
    {
        Hp = MaxHp;
        WaitCounter = 0;
        state = CrabState.StateWait;
        isDecideDirection = false;
        isDied = false;
        CounterOfDied = 0.0f;
        isRender = false;
        blinkCounter = 0;
        JumpHeight = 0.0f;
        crabManager = GameObject.Find("CrabManager");

        // player
        ObjPlayer = GameObject.FindGameObjectWithTag("Player");
        isFoundPlayer = false;
        isFoundBodyPlayer = false;

        // play se
        SoundManager.Instance.PlaySE("Shoot");

        // efx
        Vector3 efxpos = this.transform.position + new Vector3(0.0f, 10.0f, 0.0f);
        EffectManager.Instance.ShowEffect("Out", efxpos, this.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        // search
        SearchHermitCrab();

        // moving
        switch (state)
        {
            case CrabState.StateWait:
                CrabWait();
                break;
            case CrabState.StateMove:
                CrabMove();
                break;
            case CrabState.StateDiscovery:
                CrabDiscovery();
                break;
            case CrabState.StateQuickMove:
                CrabQuickMove();
                break;
            case CrabState.StateDied:
                CrabDied();
                break;
        }

        // debug
        if(Input.GetKeyDown(KeyCode.P))
        {
            state = CrabState.StateDied;

            // debug
            Vector3 efxpos = this.transform.position + new Vector3(0.0f, 10.0f, 0.0f);
            EffectManager.Instance.ShowEffect("In", efxpos, this.transform.rotation);
            // JumpRotatoin
            JumpRotation = new Vector3(Random.Range(0.0f, JumpRotationMax), Random.Range(0.0f, JumpRotationMax), 0.0f);
        }
    }

    // search hermitcrab
    void SearchHermitCrab()
    {
        if(ObjPlayer == null)
        {
            isFoundPlayer = false;
            isFoundBodyPlayer = false;
        }
        else
        {
            bool isMove = ObjPlayer.GetComponent<HermitClab>().IsMove();
            if (isMove)
            {
                isFoundPlayer = true;
            }
            else
            {
                isFoundPlayer = false;
                if(state == CrabState.StateQuickMove)
                {
                    state = CrabState.StateWait;
                }
            }

            bool isShell = ObjPlayer.GetComponent<HermitClab>().IsShell();
            if(isShell == false)
            {
                if(isFoundBodyPlayer == false)
                {
                    state = CrabState.StateDiscovery;
                }else
                {
                    state = CrabState.StateQuickMove;
                }
                isFoundBodyPlayer = true;
            }
            else
            {
                isFoundBodyPlayer = false;
            }
        }
    }

    // wait
    void CrabWait()
    {
        // counter
        WaitCounter += Time.deltaTime;
        if(WaitCounter > WaitCounterMax)
        {
            state = CrabState.StateMove;
            WaitCounter = 0.0f;
        }
    }

    // move
    void CrabMove()
    {
        // initialize direction
        if(isDecideDirection == false)
        {
            if(isFoundPlayer)
            {
                Vector3 pos = transform.position;
                Vector3 plpos = ObjPlayer.transform.position;
                Vector3 plvec = new Vector3(plpos.x - pos.x, 0.0f, plpos.z - pos.z);
                MoveDirection = plvec.normalized;
            }
            else
            {
                Vector3 angleVector = new Vector3(1.0f, 0.0f, 0.0f);
                MoveDirection = Quaternion.Euler(0.0f, Random.Range(-180.0f, 180.0f), 0.0f) * angleVector;

                // move to center
                Vector3 pos = transform.position;
                float width = FieldWidth - FieldReturnWidth;
                if (pos.x < -width
                    || pos.x > width
                    || pos.z < -width
                    || pos.z > width)
                {
                    Vector3 cenVec = new Vector3(-pos.x, 0.0f, -pos.z);
                    MoveDirection = cenVec.normalized;
                    Debug.Log("width");
                }
            }

            isDecideDirection = true;
        }

        // move
        transform.position += MoveDirection * MoveSpeed * Time.deltaTime;
        
        // counter
        MoveCounter += Time.deltaTime;
        if(MoveCounter > MoveCounterMax)
        {
            state = CrabState.StateWait;
            MoveCounter = 0.0f;
            isDecideDirection = false;
        }
    }

    // discovery
    void CrabDiscovery()
    {
        state = CrabState.StateQuickMove;
        // play se
        SoundManager.Instance.PlaySE("Serious");
    }

    // quick move
    void CrabQuickMove()
    {
        Vector3 pos = transform.position;
        Vector3 plpos = ObjPlayer.transform.position;
        Vector3 plvec = new Vector3(plpos.x - pos.x, 0.0f, plpos.z - pos.z);
        MoveDirection = plvec.normalized;
        // move
        transform.position += MoveDirection * MoveQuickSpeed * Time.deltaTime;
    }

    // Died
    void CrabDied()
    {
        // blink
        blinkCounter++;
        if(blinkCounter > blinkCounterMax)
        {
            isRender = !isRender;
            blinkCounter = 0;
        }

        //Renderer renderComponent = transform.Find("kani").gameObject.GetComponent<Renderer>();
        //renderComponent.enabled = isRender;

        // jump
        float angle = 180.0f * Mathf.Deg2Rad * (float)CounterOfDied / (float)CounterOfDiedMax;
        JumpHeight = Mathf.Sin(angle) * JumpHeightMax;
        Vector3 jumppos = new Vector3(transform.position.x, JumpHeight, transform.position.z);
        transform.position = jumppos;

        // rotation
        transform.eulerAngles += JumpRotation;
        //transform.eulerAngles += new Vector3(30.0f, 30.0f, 30.0f);

        // efx
        Vector3 efxpos = this.transform.position + new Vector3(0.0f, 10.0f, 0.0f);
        EffectManager.Instance.ShowEffect("In", efxpos, this.transform.rotation);

        // counter
        CounterOfDied += Time.deltaTime;
        if(CounterOfDied > CounterOfDiedMax)
        {
            crabManager.GetComponent<CrabManager>().NextCrabs();
            Destroy(this.gameObject);
        }
    }

    // damage
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Shell")
        {
            Debug.Log("Hit Crabs by Shell!!");
            bool isShot = other.gameObject.GetComponent<Shell>().IsShot;
            if(isShot)
            {
                Debug.Log("Hit Crabs by Shot!!");
                state = CrabState.StateDied;

                // play se
                SoundManager.Instance.PlaySE("Shoot");

                // Efx
                Vector3 efxpos = this.transform.position + new Vector3(0.0f, 10.0f, 0.0f);
                EffectManager.Instance.ShowEffect("In", efxpos, this.transform.rotation);

                // JumpRotatoin
                JumpRotation = new Vector3(Random.Range(0.0f, JumpRotationMax), Random.Range(0.0f, JumpRotationMax), 0.0f);
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Shell")
        {
            bool isShot = collision.gameObject.GetComponent<Shell>().IsShot;
            state = CrabState.StateDied;

            // play se
            SoundManager.Instance.PlaySE("Shoot");

            // Efx
            Vector3 efxpos = this.transform.position + new Vector3(0.0f, 10.0f, 0.0f);
            EffectManager.Instance.ShowEffect("In", efxpos, this.transform.rotation);

            // JumpRotatoin
            JumpRotation = new Vector3( Random.Range(0.0f, JumpRotationMax), Random.Range(0.0f, JumpRotationMax), 0.0f );

        }
    }
}
