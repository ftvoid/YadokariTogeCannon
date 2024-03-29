﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SingletonMonoBehaviour<CameraManager>
{
    Camera mainCamera;

    private void Awake()
    {
        base.Awake();
        mainCamera = Camera.main;
    }
    public void PlayStart()
    {
        mainCamera.GetComponent<TranslateObject>().Play();
    }

    public void PlayEnd()
    {
        mainCamera.GetComponent<TranslateObject>().PlayGameOver();
    }
}
