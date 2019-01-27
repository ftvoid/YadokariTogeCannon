using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrowthSlider : MonoBehaviour
{
    [SerializeField]
    private Text[] _lvTexts;

    //スライダーコンポーネント
    Slider slider;

    private void Awake()
    {
        //Sliderコンポーネント取得
        slider = gameObject.GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        //成長度に応じてスライダーの値を変更
        slider.value = StateManager.Instance.GetGrowth();

        var lvText = $"Lv.{StateManager.Instance.GetGrowthLv()}";
        for ( var i = 0 ; i < _lvTexts.Length ; i++ )
        {
            _lvTexts[i].text = lvText;
        }
    }
}
