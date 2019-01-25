using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shell : MonoBehaviour
{
    protected float attack;
    private Timer lifeTimer;

    // Update is called once per frame
    void FixedUpdate()
    {
    }



    public virtual void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
           
        }
    }
}