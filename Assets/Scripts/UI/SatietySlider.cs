using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SatietySlider : MonoBehaviour
{

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
        //満腹度に応じてスライダーの値を変更
        slider.value = StateManager.Instance.GetSatietyProportion();
    }
}
