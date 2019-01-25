using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 加算式のタイマーです
/// </summary>
[System.Serializable]
public class Timer
{
    
    private float currentTime;
    [SerializeField]
    private float limitTime;

    /// <summary>
    /// コンストラクタ
    /// 1秒
    /// </summary>
    public Timer()
    {
        currentTime = 0;
        limitTime = 1;       
    }

    /// <summary>
    /// 時間指定できるコンストラクタ
    /// </summary>
    /// <param name="time">秒</param>
    public Timer(float time)
    {
        currentTime = 0;
        limitTime = time;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        currentTime = 0;
    }

    /// <summary>
    /// 初期化
    /// 制限時間を変更する
    /// </summary>
    /// <param name="time">制限時間</param>
    public void Initialize(float time)
    {
        currentTime = 0;
        limitTime = time;
    }

    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        //時間になったらこれ以上足さない
        currentTime = Mathf.Min((currentTime + Time.deltaTime), limitTime);
    }

    /// <summary>
    /// 現在の時間を返す
    /// </summary>
    /// <returns></returns>
    public float NowTime()
    {
        return currentTime;
    }

    /// <summary>
    /// 現在の時間を変更する
    /// </summary>
    public float CurrentTime
    {
        set
        {
            currentTime = value;
        }
    }

    /// <summary>
    /// 制限時間
    /// </summary>
    public float LimitTime
    {
        get
        {
            return limitTime;
        }
        private set
        {
            limitTime = value;
        }
    }

    /// <summary>
    /// タイマーを強制的に終了させる
    /// </summary>
    public void Finish()
    {
        currentTime = limitTime; 
    }

    /// <summary>
    /// 時間になったか
    /// </summary>
    /// <returns></returns>
    public bool IsTime()
    {
        return currentTime >= limitTime;
    }
    
}
