using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shell : MonoBehaviour
{
    [SerializeField]
     float attack;


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

    void Dead()
    {
        ShellManager.Instance.DeleteShell(this.gameObject);
    }
}