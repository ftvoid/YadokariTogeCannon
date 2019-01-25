﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : SingletonMonoBehaviour<StateManager>
{
    /// <summary>
    /// 満腹度
    /// </summary>
    [SerializeField,Header("満腹度")]
    float satiety = 0;

    /// <summary>
    /// 成長度
    /// </summary>
    [SerializeField,Header("現在の成長度")]
    float growth = 0;

    [SerializeField, Header("満腹度の最大値")]
    float maxSatiety = 0;

    [SerializeField, Header("満腹度の最低値")]
    float minSatiety = 0;

    [SerializeField]
    int growthLv = 0;
    
    int needExp = 0;

    [SerializeField,Header("レベルアップに必要な経験値の初期値")]
    int nextExpBase = 0;

    [SerializeField,Header("レベルアップに必要な経験値の上昇量")]
    int nextExpInterval = 0;


    private void Awake()
    {
        needExp = GetNeedExp(growthLv);
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            LevelUp();
        }

        if (Input.GetMouseButton(0))
            growth += 1;

        if (Input.GetMouseButton(1))
            GetGrowth();

    }

    /// <summary>
    /// 満腹度を追加
    /// </summary>
    public void AddSatiety(float value)
    {
        satiety += value;
        ClampSatiety();
    }

    /// <summary>
    /// 上限設定
    /// </summary>
    void ClampSatiety()
    {
        if (satiety > maxSatiety)
            satiety = maxSatiety;

        if (satiety < minSatiety)
            satiety = minSatiety;
    }

    /// <summary>
    /// 成長度を追加
    /// </summary>
    public void AddGrowth(float value)
    {
        growth += value;
    }


    /// <summary>
    /// 成長レベルを上昇させる
    /// </summary>
    void LevelUp()
    {
        //必要経験値より小さければreturn
        if (growth < needExp)
            return;

        if (needExp == 0 || nextExpInterval == 0)
            return;

        while(growth >= needExp)
        {
            Debug.Log("レベルがあがった！");
            growthLv += 1;

            
            growth -= needExp;
            needExp = GetNeedExp(growthLv);
            Debug.Log(needExp);
        }
    }

    /// <summary>
    /// 育成度/必要経験値の割合
    /// </summary>
    /// <returns></returns>
    public float GetGrowth()
    {
        Debug.Log((growth / needExp) * 100);
        return (growth / needExp) * 100;
    }

    /// <summary>
    /// 育成レベルを取得
    /// </summary>
    /// <returns>現在の育成レベル</returns>
    public int GetGrowthLv()
    {
        return growthLv;
    }

    public float GetSatiety()
    {
        return satiety;
    }


    int GetNeedExp(int level)
    {
        return nextExpBase + nextExpInterval * ((growthLv - 1));
    }

    

}