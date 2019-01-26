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
    /// 絶対に破棄しないシーン
    /// </summary>
    [SerializeField]
    private List<string> _neverUnloadScenes = new List<string>();

    private Subject<Unit> _onEnd;

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
    /// <param name="collectGc"></param>
    public static void Load(string scene, Dictionary<string, object> sceneParams = null, bool collectGc = true)
    {
        Instance.SceneParams = sceneParams;
        Instance.OnLoad(scene, collectGc);
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
    /// シーンロード処理
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="collectGc"></param>
    private void OnLoad(string scene, bool collectGc)
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

        // 対象のシーンをロード
        SceneManager.LoadScene(scene, LoadSceneMode.Additive);
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
}
