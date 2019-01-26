using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {

    [SerializeField]
    int rotateCount;
    [SerializeField]
    Vector3 rotate;

    public bool IsRotate = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        Rotate();
        ClampRotate();
    }

    void Rotate()
    {
        if (!IsRotate)
            return;
        this.gameObject.transform.Rotate(rotate * rotateCount * Time.deltaTime);
    }

    void ClampRotate()
    {
        
        
        //if (this.gameObject.transform.eulerAngles.x > 60 )
        //{           
        //    rotateCount *= -1;
        //    this.gameObject.transform.eulerAngles = new Vector3(60, this.gameObject.transform.eulerAngles.y);
        //    Debug.Log(this.gameObject.transform.eulerAngles.x);
        //}
        //else if(this.gameObject.transform.eulerAngles.x <-60)
        //{
        //    rotateCount *= -1;
        //    this.gameObject.transform.eulerAngles = new Vector3(-60, this.gameObject.transform.eulerAngles.y);
        //}
    }
}
