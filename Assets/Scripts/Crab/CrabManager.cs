﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabManager : MonoBehaviour
{
    public int CrabsMax = 5;
    public int StartCrabs = 2;
    private int NumberOfCrabs;

    public float CounterOfRespawnMax = 2.0f;
    private float CounterOfRespawn;

    public GameObject ObjCrabs;

    public float FieldWidth = 150.0f;
    public float FieldEdgeSpan = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        NumberOfCrabs = 0;
        CounterOfRespawn = 0.0f;

        // initialize crab
        if (NumberOfCrabs < StartCrabs)
        {
            // create crabs
            int nummax = StartCrabs - NumberOfCrabs;
            for (int i = 0; i < nummax; ++i)
            {
                CreateCrabs();
                NumberOfCrabs++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {




        // count crabs
        CountCrabs();
    }

    // Create Crabs
    void CreateCrabs(){
        float width = FieldWidth - FieldEdgeSpan;
        Vector3 pos = new Vector3( Random.Range( -width, width ), 0.0f, Random.Range( -width, width ) );
        Instantiate(ObjCrabs, pos, Quaternion.identity);
    }


    // count crabs
    void CountCrabs()
    {

    }
}