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

    [Header("ヤシガニ 到達Lv"), SerializeField]
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
            _message.text = $"ホンヤドカリ級　1～5センチ\n次はきっと大きくなれるはず。";

            yadokariType = "ホンヤドカリ";
            size = 1;
        }
        else if ( _growthLv < _rankALv )
        {
            _message.text = $"オカヤドカリ級　10センチ\nけっこう育った。";

            yadokariType = "オカヤドカリ";
            size = 5;
        }
        else if ( _growthLv < _rankSLv )
        {
            _message.text = $"ヤシガニ級　50センチ\nBigになってやったぜ！";

            yadokariType = "ヤシガニ";
            size = 50;
        }
        else
        {
            _message.text = $"タラバガニ級　1メートルくらい\nBigもはやカニ！！";

            yadokariType = "タラバガニ";
            size = 100;
        }

        //_message.text = $"あなたのヤドカリは{size}センチ！\nこのヤドカリは{yadokariType}級！！";
    }

    private void Update()
    {
        if ( Input.GetButtonDown("Submit") )
        {
            SceneChanger.Load("TitleScene");
        }
    }
}
