using System;
using UnityEngine;
using UnityEngine.UI;

public class PausePanel : MonoBehaviour
{
    [SerializeField] private GameObject checkPanel;
    
    [Header("버튼")]
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button okButton;
    
    [Header("사운드바")]
    [SerializeField] private Slider masterBar;
    [SerializeField] private Slider bgmBar;
    [SerializeField] private Slider sfxBar;

    private void Start()
    {
        mainMenuButton.onClick.AddListener(() => checkPanel.SetActive(true));
        okButton.onClick.AddListener(() => gameObject.SetActive(false));
        masterBar.onValueChanged.AddListener((x) => SoundManager.Instance.masterVolume = x);
        bgmBar.onValueChanged.AddListener((x) => SoundManager.Instance.bgmVolume = x);
        sfxBar.onValueChanged.AddListener((x) => SoundManager.Instance.sfxVolume = x);
    }
}