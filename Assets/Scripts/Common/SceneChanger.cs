using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン遷移管理
/// </summary>
public class SceneChanger : SingletonMonoBehaviour<SceneChanger>
{
    /// <summary>
    /// シーンのトランジション情報
    /// </summary>
    [Serializable]
    private struct TransitionInfo
    {
        public string transitionID;
        public Transition prefab;
    }

    [Header("トランジション情報"), SerializeField]
    private TransitionInfo[] _transitionInfo;

    /// <summary>
    /// 絶対に破棄しないシーン
    /// </summary>
    [Header("絶対に破棄しないシーン"), SerializeField]
    private List<string> _neverUnloadScenes = new List<string>();

    private Subject<Unit> _onEnd = new Subject<Unit>();
    private Transition _currentTransition = null;

    /// <summary>
    /// シーン遷移終了通知
    /// </summary>
    public IObservable<Unit> OnEnd => _onEnd;

    /// <summary>
    /// シーンパラメータ
    /// </summary>
    public Dictionary<string, object> SceneParams { get; private set; }

    /// <summary>
    /// シーンをロードする
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="sceneParams"></param>
    /// <param name="transitionID"></param>
    /// <param name="collectGc"></param>
    public static void Load(string scene, Dictionary<string, object> sceneParams = null, string transitionID = "", bool collectGc = true)
    {
        Instance.SceneParams = sceneParams;
        Instance.OnLoad(scene, transitionID, collectGc);
    }

    /// <summary>
    /// シーンを追加でロードする
    /// </summary>
    /// <param name="scene"></param>
    public static void LoadAdditive(string scene)
    {
        Instance.OnLoadAdditive(scene);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        GameObject.DontDestroyOnLoad(this);
    }

    /// <summary>
    /// シーンロード処理
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="transitionID"></param>
    /// <param name="collectGc"></param>
    private void OnLoad(string scene, string transitionID, bool collectGc)
    {
        // 以前のシーンをアンロード
        for ( int i = 0 ; i < SceneManager.sceneCount ; ++i )
        {
            var name = SceneManager.GetSceneAt(i).name;
            if ( !IsNerverUnloadScene(name) )
            {
                SceneManager.UnloadSceneAsync(name);
            }
        }

        // GCの実施
        if ( collectGc )
            System.GC.Collect();

        if ( string.IsNullOrEmpty(scene) )
            return;

        // トランジション設定
        if ( !string.IsNullOrEmpty(transitionID) )
        {
            var index = Array.FindIndex(_transitionInfo, x => x.transitionID == transitionID);
            if ( index >= 0 )
            {
                _currentTransition = _transitionInfo[index].prefab;
                _currentTransition.Begin(() => _onEnd.OnNext(Unit.Default));
            }
        }

        // 対象のシーンをロード
        SceneManager.LoadScene(scene, LoadSceneMode.Single);

        if ( _currentTransition == null )
        {
            _onEnd.OnNext(Unit.Default);
        }
    }

    /// <summary>
    /// シーンの追加ロード処理
    /// </summary>
    /// <param name="scene"></param>
    private void OnLoadAdditive(string scene)
    {
        if ( string.IsNullOrEmpty(scene) )
            return;

        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
    }

    /// <summary>
    /// 破棄しないシーンかどうか
    /// </summary>
    /// <param name="scene">シーン名</param>
    /// <returns></returns>
    private bool IsNerverUnloadScene(string scene)
    {
        return _neverUnloadScenes.FindIndex(x => x.ToString() == scene) >= 0;
    }

    #region テストコード
    [ContextMenu("シーン変更")]
    private void ChangeScene()
    {
        Load("mainScene");
    }
    #endregion
}
