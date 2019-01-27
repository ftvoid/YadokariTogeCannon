using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField,Header("増加する満腹度")]
    float increasSatiety;

    [SerializeField, Header("増加する成長度")]
    float increasGrowth;

    /// <summary>
    /// 満腹度を取得する
    /// </summary>
    /// <returns></returns>
    public float GetIncreasSatiety()
    {
        return increasSatiety; 
    }

    /// <summary>
    /// 育成度を取得する
    /// </summary>
    /// <returns></returns>
    public float GetIncreasGrowth()
    {
        return increasSatiety;
    }
}
