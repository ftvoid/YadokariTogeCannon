using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkManeger : SingletonMonoBehaviour<SharkManeger>
{
	//サメ出現までの時間最大値
	[SerializeField] float timerMax = 0;

	//サメ出現までの時間
	float timer;

	//タイマー減少を行うか
	bool timerFlag = true;

	void Start()
	{
		timer = timerMax;
	}

	void Update()
	{
		if(timerFlag)
		{
			timer -= Time.deltaTime;
			if(timer <= 0)
			{
				//サメ出現までの時間が0になった時に1度だけ行う処理

				timerFlag = false;
			}
		}
	}

	/// <summary>
	/// サメ出現までの時間を取得
	/// </summary>
	public float GetTime()
	{
		return timer;
	}

	/// <summary>
	/// サメ出現までの時間を割合で取得(0~100)
	/// </summary>
	public float GetTimeProportion()
	{
		return (timer / timerMax) * 100;
	}

}
