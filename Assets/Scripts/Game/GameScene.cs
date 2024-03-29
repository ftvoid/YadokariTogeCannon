﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// ゲームの画面状態
/// </summary>
public enum GameState
{
    None,

    /// <summary>
    /// 初期化中
    /// </summary>
    Init,

    /// <summary>
    /// 開始演出中
    /// </summary>
    Start,

    /// <summary>
    /// ゲームプレイ中
    /// </summary>
    Play,

    /// <summary>
    /// ゲームオーバー演出中
    /// </summary>
    GameOver,

    /// <summary>
    /// クリア演出中
    /// </summary>
    GameClear,
}

/// <summary>
/// ゲームシーン管理
/// </summary>
public class GameScene : SingletonMonoBehaviour<GameScene>
{
    [Header("説明UI"), SerializeField]
    private GameObject _tutorialUI;

    [Header("カウントダウン用UI"), SerializeField]
    private CountDown _countDownUI;

    [Header("サメ管理"), SerializeField]
    private SharkManeger _sharkManager;

    [Header("プレイヤー"), SerializeField]
    private HermitClab _player;

    private ReactiveProperty<GameState> _gameState = new ReactiveProperty<GameState>();

    private TranslateObject _translateObj;

    /// <summary>
    /// 状態
    /// </summary>
    public IReadOnlyReactiveProperty<GameState> OnStateChanged => _gameState;

    /// <summary>
    /// ゲームを開始する
    /// </summary>
    public void StartGame()
    {
        _sharkManager.enabled = true;
        _player.enabled = true;

        SoundManager.Instance.PlayBGM("Game");

        _gameState.Value = GameState.Play;
    }

    /// <summary>
    /// ゲームオーバー画面を表示する
    /// </summary>
    public void ShowGameOver()
    {
        CameraManager.Instance.PlayEnd();
        _gameState.Value = GameState.GameOver;

        SceneChanger.Load("GameOverScene", new Dictionary<string, object>()
        {
            { "growthLv", StateManager.Instance.GetGrowthLv() },
        });
    }

    /// <summary>
    /// クリア画面を表示する
    /// </summary>
    public void ShowGameClear()
    {
        _gameState.Value = GameState.GameClear;
        SceneChanger.Load("ResultScene", new Dictionary<string, object>()
        {
            { "growthLv", StateManager.Instance.GetGrowthLv() },
        });
    }

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        _tutorialUI.SetActive(true);
        _countDownUI.gameObject.SetActive(false);
        _sharkManager.enabled = false;
        _player.enabled = false;

        _translateObj = GameObject.FindObjectOfType<TranslateObject>();

        _gameState.Value = GameState.Init;

        this.UpdateAsObservable()
            //.ThrottleFirst(System.TimeSpan.FromSeconds(0.1f))
            .Where(_ => _gameState.Value == GameState.Init)
            .Subscribe(_ => OnTutorial())
            .AddTo(this);
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator Init()
    {
        _gameState.Value = GameState.Start;

        // カメラ移動
        _translateObj.Play();

        // カウントダウンUI表示
        Debug.Log("GameScene : カウントダウンUI表示");
        CameraManager.Instance.PlayStart();

        SoundManager.instance.PlaySE("CountDown");

        _countDownUI.gameObject.SetActive(true);

        bool isComplete = false;
        _countDownUI.StartCountDown(() => isComplete = true);
        yield return new WaitUntil(() => isComplete);

        // ゲーム開始
        Debug.Log("GameScene : ゲーム開始");
        StartGame();
    }

    private void OnTutorial()
    {
        if ( Input.GetButtonDown("Submit") )
        {
            _tutorialUI.SetActive(false);
            SoundManager.Instance.PlaySE("Decide");
            StartCoroutine(Init());
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if ( Input.GetKeyDown(KeyCode.R) )
        {
            ShowGameClear();
        }
        else if ( Input.GetKeyDown(KeyCode.G) )
        {
            ShowGameOver();
        }
    }
#endif
}
