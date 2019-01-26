using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UniRx;
using UniRx.Triggers;

/// <summary>
/// エフェクト
/// </summary>
public class Effect : MonoBehaviour
{
    /// <summary>
    /// 表示対象のパーティクル
    /// </summary>
    [Header("パーティクルシステム"), SerializeField]
    private ParticleSystem _particleSystem;

    private UnityAction _onComplete;

    private Coroutine _playEffect = null;

    /// <summary>
    /// エフェクト再生
    /// </summary>
    /// <param name="onComplete"></param>
    public void Play(UnityAction onComplete = null)
    {
        _onComplete = onComplete;

        if ( _particleSystem == null )
        {
            return;
        }

        _particleSystem.Play();

        if ( _playEffect != null )
        {
            StopCoroutine(_playEffect);
        }

        _playEffect = StartCoroutine(PlayEffect());
    }

    /// <summary>
    /// エフェクト停止
    /// </summary>
    public void Stop()
    {
        _particleSystem?.Stop();
        _onComplete?.Invoke();

        if ( _playEffect != null )
        {
            StopCoroutine(_playEffect);
            _playEffect = null;
        }
    }

    private IEnumerator PlayEffect()
    {
        yield return new WaitWhile(() => _particleSystem.IsAlive());
        _onComplete?.Invoke();
        _playEffect = null;
    }
}
