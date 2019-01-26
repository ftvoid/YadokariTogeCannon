using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サウンドマネージャー
/// </summary>
public class SoundManager : SingletonMonoBehaviour<SoundManager>
{
    [Serializable]
    private struct SEInfo
    {
        public string seID;
        public AudioClip clip;
        public bool isLoop;

        [NonSerialized]
        public AudioSource audioSource;
    }

    [Serializable]
    private struct BGMInfo
    {
        public string bgmID;
        public AudioClip clip;
        public bool isLoop;

        [NonSerialized]
        public AudioSource audioSource;
    }

    [Header("SE"), SerializeField]
    private SEInfo[] _seInfo;

    [Header("BGM"), SerializeField]
    private BGMInfo[] _bgmInfo;

    private AudioSource[] _seAudioSources;
    private AudioSource[] _bgmAudioSources;

    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="seID"></param>
    public void PlaySE(string seID)
    {
        var index = Array.FindIndex(_seInfo, x => x.seID == seID);

        if ( index < 0 )
        {
            Debug.LogError($"SE\"{seID}\"が存在しません");
            return;
        }

        var audioSource = _seInfo[index].audioSource;
        if ( audioSource == null )
        {
            return;
        }

        if ( !_seInfo[index].isLoop || !audioSource.isPlaying )
        {
            audioSource?.Play();
        }
    }

    /// <summary>
    /// BGM再生
    /// </summary>
    /// <param name="bgmID"></param>
    public void PlayBGM(string bgmID)
    {
        var index = Array.FindIndex(_bgmInfo, x => x.bgmID == bgmID);

        if ( index < 0 )
        {
            Debug.LogError($"BGM\"{bgmID}\"が存在しません");
            return;
        }

        var audioSource = _bgmInfo[index].audioSource;
        if ( audioSource == null )
        {
            return;
        }

        if( !audioSource.isPlaying )
        {
            audioSource.Play();
        }
    }

    /// <summary>
    /// SE停止
    /// </summary>
    /// <param name="seID"></param>
    public void StopSE(string seID)
    {
        var index = Array.FindIndex(_seInfo, x => x.seID == seID);

        if ( index < 0 )
        {
            Debug.LogError($"SE\"{seID}\"が存在しません");
            return;
        }

        _seInfo[index].audioSource?.Stop();
    }

    /// <summary>
    /// 全SE停止
    /// </summary>
    public void StopAllSE()
    {
        for ( var i = 0 ; i < _seInfo.Length ; i++ )
        {
            _seInfo[i].audioSource?.Stop();
        }
    }

    /// <summary>
    /// BGM停止
    /// </summary>
    public void StopBGM()
    {
        // TODO : 余裕あれば処理改善
        for ( var i = 0 ; i < _bgmInfo.Length ; i++ )
        {
            _bgmInfo[i].audioSource?.Stop();
        }
    }

    protected override void Awake()
    {
        base.Awake();

        _seAudioSources = new AudioSource[_seInfo.Length];
        for ( var i = 0 ; i < _seInfo.Length ; i++ )
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = _seInfo[i].isLoop;
            audioSource.clip = _seInfo[i].clip;
            _seInfo[i].audioSource = audioSource;
        }

        _bgmAudioSources = new AudioSource[_bgmInfo.Length];
        for ( var i = 0 ; i < _bgmInfo.Length ; i++ )
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = _bgmInfo[i].isLoop;
            audioSource.clip = _bgmInfo[i].clip;
            _bgmInfo[i].audioSource = audioSource;
        }
    }

    #region テストコード
    [ContextMenu("SEテスト")]
    private void TestPlaySE()
    {
        PlaySE("Shoot");
    }

    [ContextMenu("BGMテスト")]
    private void TestPlayBGM()
    {
        PlayBGM("Title");
    }
    #endregion
}
