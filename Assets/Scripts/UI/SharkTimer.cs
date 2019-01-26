using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SharkTimer : MonoBehaviour
{
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
        image.fillAmount = SharkManeger.Instance.GetTimeProportion() / 100f;
    }
}
