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

    // Start is called before the first frame update
    void Start()
    {
        Hp = MaxHp;
        WaitCounter = 0;
        state = CrabState.StateWait;
        isDecideDirection = false;
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
        }
    }

    // search hermitcrab
    void SearchHermitCrab()
    {

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
//            float rndAngle = 
            isDecideDirection = true;
        }

        // move


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

    }

    // quick move
    void CrabQuickMove()
    {

    }
}
