using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrowthSlider : MonoBehaviour
{
    //成長度の取得用
    StateManager stateManager;

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
        slider.value = stateManager.GetGrowth();
    }
}
