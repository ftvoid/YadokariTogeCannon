using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SharkTimer : MonoBehaviour
{
	//サメ出現までの時間を取得
	SharkManeger sharkManeger;

	//画像コンポーネント
	Image image;

    // Start is called before the first frame update
    void Awake()
    {
		image = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
		image.fillAmount = sharkManeger.GetTimeProportion();
    }
}
