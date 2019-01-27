using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateObject : MonoBehaviour
{
    [SerializeField]
    Vector3 eulerAngle;

    [SerializeField]
    Vector3 movePosition;

    [SerializeField]
    GameObject chaseObject;

    Vector3 firstAngle;
    Vector3 firstPos;

    float time = 0;

    [SerializeField]
    bool IsEndMove = false;

    bool IsMove = false;

    [SerializeField]
    bool IsInit = false;

    private void Start()
    {
        firstAngle = this.transform.eulerAngles;
        firstPos = this.transform.position;
        chaseObject = GameObject.FindGameObjectWithTag("Player");
    }

    public void Play()
    {
        IsMove = true;
        InitTime();
    }

    void InitTime()
    {
        time = 0;
    }

    public void PlayGameOver()
    {
        IsEndMove = true;
        InitTime();
    }

    private void Update()
    {
        Move();
        EndMove();
        Skip();
        PlayTime();
    }

    void PlayTime()
    {
        if(IsInit)
        time = 0;
    }

    /// <summary>
    /// カメラの移動!
    /// </summary>
    private void Move()
    {
        if (!IsMove)
            return;

        
        time += 1f * Time.deltaTime * 0.5f;
        this.transform.position = Vector3.Lerp(firstPos, movePosition, time);
        this.transform.eulerAngles = Vector3.Lerp(firstAngle, eulerAngle, time);

    }

    private void EndMove()
    {
        if (!IsEndMove)
            return;

        Vector3 pos = chaseObject.transform.position + new Vector3(0, 12, -40);
        Vector3 angle = new Vector3(30, 0, 0);
        
        time += 1f * Time.deltaTime * 0.5f;
        this.transform.position = Vector3.Lerp(movePosition, pos, time);
        this.transform.eulerAngles = Vector3.Lerp(firstAngle, eulerAngle, time);
    }

    /// <summary>
    /// スキップ
    /// </summary>
    void Skip()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            this.transform.position = movePosition;
            this.transform.eulerAngles = eulerAngle;
        }
    }

}
