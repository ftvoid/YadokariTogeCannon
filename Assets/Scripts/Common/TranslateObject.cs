using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateObject : MonoBehaviour
{
    [SerializeField]
    Vector3 eulerAngle;

    [SerializeField]
    Vector3 movePosition;

    Vector3 firstAngle;
    Vector3 firstPos;

    float time = 0;

    bool IsMove = false;

    private void Start()
    {
        firstAngle = this.transform.eulerAngles;
        firstPos = this.transform.position;
    }

    public void MoveStart()
    {
        IsMove = true;
    }

    private void Update()
    {
        Move();
        Skip();

        if (Input.GetButtonDown("Jump"))
        {
            MoveStart();
        }
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

    /// <summary>
    /// スキップ
    /// </summary>
    void Skip()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire1"))
        {
            this.transform.position = movePosition;
            this.transform.eulerAngles = eulerAngle;

            IsMove = false;
        }
    }

}
