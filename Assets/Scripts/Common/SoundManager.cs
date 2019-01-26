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
    }

    [Serializable]
    private struct BGMInfo
    {
        public string bgmID;
        public AudioClip clip;
    }

    /// <summary>
    /// オーディオ再生用のAudioSource
    /// </summary>
    [SerializeField]
    private AudioSource _audioSource;

    [Header("SE"), SerializeField]
    private SEInfo[] _seInfo;

    [Header("BGM"), SerializeField]
    private BGMInfo[] _bgmInfo;

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

        _audioSource.PlayOneShot(_seInfo[index].clip);
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

        _audioSource.PlayOneShot(_bgmInfo[index].clip);
    }
}
