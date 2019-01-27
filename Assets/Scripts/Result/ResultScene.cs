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

    int _growthLv;

    private void Start()
    {
        SoundManager.Instance.PlayBGM("Clear");

        Debug.Log("SceneChanger.Instance.SceneParams = " + SceneChanger.Instance.SceneParams);

        object growthLv = null;
        SceneChanger.Instance.SceneParams?.TryGetValue("growthLv", out growthLv);

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

        if ( _growthLv < 4 )
        {
            yadokariType = "ホンヤドカリ";
            size = 1;
        }
        else if ( _growthLv < 8 )
        {
            yadokariType = "オカヤドカリ";
            size = 5;
        }
        else if ( _growthLv < 12 )
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
