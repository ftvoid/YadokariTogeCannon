using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SingletonMonoBehaviour<CameraManager>
{
    Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }
    public void StartCameraMove()
    {
        mainCamera.GetComponent<TranslateObject>().Play();
    }
}
