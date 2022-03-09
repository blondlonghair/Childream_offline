using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonoCreate<SoundManager>
{
    [Range(0, 1)] public float sfxVolume = 1f;
    [Range(0, 1)] public float bgmVolume = 1f;
    [Range(0, 1)] public float masterVolume = 1f;
    
    [SerializeField] private AudioClip[] sfx;
    [SerializeField] private AudioClip[] bgm;

    private Dictionary<string, AudioClip> _sfxDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> _bgmDict = new Dictionary<string, AudioClip>();

    private AudioSource sfxPlayer;
    private AudioSource bgmPlayer;
    
    private void Start()
    {
        sfxPlayer = new GameObject("sfx").AddComponent<AudioSource>();
        sfxPlayer.transform.SetParent(transform);
        
        bgmPlayer = new GameObject("bgm").AddComponent<AudioSource>();
        bgmPlayer.transform.SetParent(transform);

        foreach (var audioClip in sfx)
        {
            _sfxDict.Add(audioClip.name, audioClip);
        }

        foreach (var audioClip in bgm)
        {
            _bgmDict.Add(audioClip.name, audioClip);
        }
    }

    public void PlaySFXSound(string clipName, float volume = 1f)
    {
        if (_sfxDict.ContainsKey(clipName) == false)
        {
            Debug.Log(clipName + " : 존재하지 않는 오디오 클립");
            return;
        }
        sfxPlayer.PlayOneShot(_sfxDict[clipName], volume * sfxVolume * masterVolume);
    }

    public void PlayBGMSound(string clipName, float volume = 1f)
    {
        bgmPlayer.loop = true;
        bgmPlayer.volume = volume * bgmVolume * masterVolume;
        bgmPlayer.clip = _bgmDict[clipName];
        bgmPlayer.Play();
    }
}