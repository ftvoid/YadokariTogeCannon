using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawGrowLv : MonoBehaviour
{
    Text text;
    private void Start()
    {
        text = GetComponent<Text>();
    }
    
    private void Update()
    {
        text.text = "Lv."+StateManager.Instance.GetGrowthLv();
    }
}
