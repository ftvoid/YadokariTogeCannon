using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shell : MonoBehaviour
{
    [SerializeField]
    float attack = 1;

    [SerializeField]
    float stanTime = 2;

    public bool IsShot
    {
        get;
        set;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.transform.position.x >= 300 || this.transform.position.x <= -300 ||
            this.transform.position.z >= 300 || this.transform.position.z <= -300)
            Dead();
    }

     void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Dead();
        }
    }

    public float GetAttack()
    {
        return attack;
    }

    public float GetStanTime()
    {
        return stanTime;
    }

    void Dead()
    {
        ShellManager.Instance.DeleteShell(this.gameObject);
    }


}