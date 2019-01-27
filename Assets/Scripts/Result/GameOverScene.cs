using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ゲームオーバーシーン
/// </summary>
public class GameOverScene : MonoBehaviour
{
    [SerializeField]
    private Text _message;

    [Header("オカヤドカリ 到達Lv"), SerializeField]
    private int _rankBLv = 4;

    [Header("ヤシガニ 到達Lv"), SerializeField]
    private int _rankALv = 8;

    [Header("タラバガニ 到達Lv"), SerializeField]
    private int _rankSLv = 12;

    int _growthLv;

    private void Start()
    {
        SoundManager.Instance.PlayBGM("GameOver");

        object growthLv = null;
        SceneChanger.Instance?.SceneParams?.TryGetValue("growthLv", out growthLv);

        if ( growthLv != null && growthLv is int )
        {
            _growthLv = (int)growthLv;
        }
        else
        {
            _growthLv = 1;
        }

        string yadokariType;
        int size;

        if ( _growthLv < _rankBLv )
        {
            yadokariType = "ホンヤドカリ";
            size = 1;
        }
        else if ( _growthLv < _rankALv )
        {
            yadokariType = "オカヤドカリ";
            size = 5;
        }
        else if ( _growthLv < _rankSLv )
        {
            yadokariType = "ヤシガニ";
            size = 50;
        }
        else
        {
            yadokariType = "タラバガニ";
            size = 100;
        }

        _message.text = $"あなたのヤドカリは{size}センチ！\nこのヤドカリは{yadokariType}級！！";
    }

    private void Update()
    {
        if ( Input.GetButtonDown("Submit") )
        {
            SceneChanger.Load("TitleScene");
        }
    }
}
