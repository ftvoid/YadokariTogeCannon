using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabManager : MonoBehaviour
{
    public int StartCrabs = 2;
    public int EndCrabs = 5;
    private int NumberOfCrabs;

    public float CounterOfRespawnMax = 2.0f;
    public float CounterOfNextRespawnMax = 5.0f;
    private float CounterOfRespawn;

    public GameObject ObjCrabs;

    public float FieldWidth = 100.0f;
    public float FieldEdgeSpan = 30.0f;

    public float SpawnWithOutWidth = 40.0f;
    public int SpawnRetryMax = 100;

    // player check
    private GameObject ObjPlayer;


    // Start is called before the first frame update
    void Start()
    {
        NumberOfCrabs = 0;
        CounterOfRespawn = 0.0f;

        // player
        ObjPlayer = GameObject.FindGameObjectWithTag("Player");

        // initialize crab
        if (NumberOfCrabs < StartCrabs)
        {
            // create crabs
            int nummax = StartCrabs - NumberOfCrabs;
            for (int i = 0; i < nummax; ++i)
            {
                CreateCrabs();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // respawn crabs
        if(NumberOfCrabs < EndCrabs)
        {
            CounterOfRespawn += Time.deltaTime;
            if(CounterOfRespawn > CounterOfRespawnMax)
            {
                CreateCrabs();
                CounterOfRespawn = 0.0f;
            }
        }
    }

    // Create Crabs
    void CreateCrabs(){
        float width = FieldWidth - FieldEdgeSpan;

        Vector3 pos = new Vector3( Random.Range( -width, width ), 0.0f, Random.Range( -width, width ) );
        Vector3 plpos = new Vector3(0.0f, 0.0f, 0.0f);
        if (ObjPlayer == null)
        {

        }
        else
        {
            plpos = ObjPlayer.transform.position;
        }

        for (int i = 0;i < SpawnRetryMax; ++i)
        {
            pos = new Vector3(Random.Range(-width, width), 0.0f, Random.Range(-width, width));
            if ( ( pos.x < plpos.x + SpawnWithOutWidth )
                && ( pos.x > plpos.x - SpawnWithOutWidth)
                && ( pos.z < plpos.z + SpawnWithOutWidth)
                && ( pos.z > plpos.z - SpawnWithOutWidth))
            {
                ;
            }
            else
            {
                break;
            }
        }

        Instantiate(ObjCrabs, pos, Quaternion.identity);

        NumberOfCrabs++;
    }


    // Next Crabs by Died
    public void NextCrabs()
    {
        NumberOfCrabs--;
        if(NumberOfCrabs <= 0)
        {
            NumberOfCrabs = 0;
        }
        float nextrespawn = CounterOfRespawnMax - CounterOfNextRespawnMax;
        if(CounterOfRespawn < nextrespawn)
        {
            CounterOfRespawn = nextrespawn;
        }
    }
}
