using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// タイトルシーン
/// </summary>
public class TitleScene : MonoBehaviour
{
    private void Update()
    {
        if ( Input.GetButtonDown("Submit") )
        {
            SceneChanger.Load("mainScene");
        }
    }
}
