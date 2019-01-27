using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// リザルトシーン
/// </summary>
public class ResultScene : MonoBehaviour
{
    [SerializeField]
    private Text _message;

    [Header("オカヤドカリ 到達Lv"), SerializeField]
    private int _rankBLv = 4;

    [Header("コモンヤドカリ 到達Lv"), SerializeField]
    private int _rankALv = 8;

    [Header("タラバガニ 到達Lv"), SerializeField]
    private int _rankSLv = 12;

    int _growthLv;

    private void Start()
    {
        SoundManager.Instance.PlayBGM("Clear");

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
            yadokariType = "コモンヤドカリ";
            size = 20;
        }
        else
        {
            yadokariType = "タラバガニ";
            size = 50;
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
