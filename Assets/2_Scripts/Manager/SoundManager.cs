using System;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : SingletonMonoCreate<SoundManager>
{
    [SerializeField] private AudioClip[] sfx;
    [SerializeField] private AudioClip[] bgm;

    private Dictionary<string, AudioClip> _sfx = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> _bgm = new Dictionary<string, AudioClip>();

    private AudioSource sfxObj;
    private AudioSource bgmObj;
    
    private void Start()
    {
        sfxObj = new GameObject("sfx").AddComponent<AudioSource>();
        sfxObj.transform.parent = transform;
        
        bgmObj = new GameObject("bgm").AddComponent<AudioSource>();
        bgmObj.transform.parent = transform;

        foreach (var audioClip in sfx)
        {
            _sfx.Add(audioClip.name, audioClip);
        }

        foreach (var audioClip in bgm)
        {
            _bgm.Add(audioClip.name, audioClip);
        }
    }

    public void PlaySFX(string clip)
    {
        sfxObj.PlayOneShot(_sfx[clip]);
    }

    public void PlayBGM(string clip)
    {
        bgmObj.PlayOneShot(_bgm[clip]);
    }
}