using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

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
    [Header("カウントダウン用UI"), SerializeField]
    private CountDown _countDownUI;

    private ReactiveProperty<GameState> _gameState = new ReactiveProperty<GameState>();

    /// <summary>
    /// 状態
    /// </summary>
    public IReadOnlyReactiveProperty<GameState> OnStateChanged => _gameState;

    /// <summary>
    /// ゲームを開始する
    /// </summary>
    public void StartGame()
    {
        _gameState.Value = GameState.Play;
    }

    /// <summary>
    /// ゲームオーバー画面を表示する
    /// </summary>
    public void ShowGameOver()
    {
        _gameState.Value = GameState.GameOver;
    }

    /// <summary>
    /// クリア画面を表示する
    /// </summary>
    public void ShowGameClear()
    {
        _gameState.Value = GameState.GameClear;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        _gameState.Value = GameState.Init;
    }

    private void Start()
    {
        StartCoroutine(Init());
    }

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator Init()
    {
        _gameState.Value = GameState.Start;

        // カウントダウンUI表示
        Debug.Log("GameScene : カウントダウンUI表示");
        bool isComplete = false;
        _countDownUI.StartCountDown(() => isComplete = true);
        yield return new WaitUntil(() => isComplete);

        // ゲーム開始
        Debug.Log("GameScene : ゲーム開始");
        StartGame();
    }
}
