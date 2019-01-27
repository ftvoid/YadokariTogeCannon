using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームオーバーシーン
/// </summary>
public class GameOverScene : MonoBehaviour
{
    private void Start()
    {
        SoundManager.Instance.PlayBGM("GameOver");
    }

    private void Update()
    {
        if ( Input.GetButtonDown("Submit") )
        {
            SceneChanger.Load("TitleScene");
        }
    }
}
