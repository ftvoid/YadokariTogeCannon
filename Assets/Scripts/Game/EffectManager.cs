using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// エフェクト管理
/// </summary>
public class EffectManager : SingletonMonoBehaviour<EffectManager>
{
    /// <summary>
    /// エフェクトの管理情報
    /// </summary>
    [Serializable]
    private struct EffectInfo
    {
        public string effectID;
        public Effect effectPrefab;
        public int cacheNum;

        [NonSerialized]
        public List<Effect> playingEffects;
        [NonSerialized]
        public List<Effect> poolEffects;
    }

    /// <summary>
    /// エフェクト定義
    /// </summary>
    [Header("エフェクト定義"), SerializeField]
    private EffectInfo[] _effectInfo;

    /// <summary>
    /// エフェクトの親オブジェクト
    /// </summary>
    [Header("エフェクトの親オブジェクト"), SerializeField]
    private Transform _effectParent;

    /// <summary>
    /// エフェクトを表示する
    /// </summary>
    /// <param name="effectID"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    public void ShowEffect(string effectID, Vector3 position, Quaternion rotation)
    {
        var index = Array.FindIndex(_effectInfo, x => x.effectID.Equals(effectID, StringComparison.CurrentCultureIgnoreCase));

        if ( index < 0 )
        {
            Debug.LogError($"不正なエフェクトID\"{effectID}\"が指定されました");
            return;
        }

        var info = _effectInfo[index];

        // TODO : 今は楽な実装。あとでオブジェクトプールを使う方法に切り替える。
        var effect = Instantiate(info.effectPrefab, position, rotation, _effectParent);
        effect.Play(() =>
        {
            info.playingEffects.Remove(effect);
            GameObject.Destroy(effect.gameObject);
            //Debug.Log($"エフェクト\"{effectID}\"消滅");
        });
        info.playingEffects.Add(effect);

        Debug.Log($"エフェクト\"{effectID}\"出現");

        _effectInfo[index] = info;
    }

    /// <summary>
    /// 初期化
    /// </summary>
    protected override void Awake()
    {
        base.Awake();

        // エフェクト作成準備
        for ( var i = 0 ; i < _effectInfo.Length ; i++ )
        {
            var info = _effectInfo[i];

            info.playingEffects = new List<Effect>();
            info.poolEffects = new List<Effect>();

            _effectInfo[i] = info;
        }
    }

    #region テストコード
    [ContextMenu("テストエフェクト表示")]
    private void TestEffect()
    {
        ShowEffect("Hit", Vector3.zero, Quaternion.identity);
    }
    #endregion
}
