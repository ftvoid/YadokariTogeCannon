﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseObject : MonoBehaviour { 

     public GameObject target
    {
        get;
        set;
    }


    [SerializeField]
    Vector3 translatePos;

    [SerializeField]
    Vector3 eulerAngle;

    public bool IsChase = false;

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        if (!IsChase)
            return;

        this.transform.position = target.transform.position + translatePos;
        this.transform.eulerAngles = target.transform.eulerAngles + eulerAngle;
    }
}
