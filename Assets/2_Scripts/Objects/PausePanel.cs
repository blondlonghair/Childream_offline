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
        if (TryGetComponent(out Image image))
        {
            image.rectTransform.SetAsLastSibling();
        }
        
        if (checkPanel.transform.GetChild(0).TryGetComponent(out Button button))
        {
            button.onClick.AddListener(() =>
            {
                InGameManager.Instance.LoadScene("Lobby");
                ItemManager.Instance.ClearItem();
            });
        }
        
        mainMenuButton.onClick.AddListener(() => checkPanel.SetActive(true));
        okButton.onClick.AddListener(() => gameObject.SetActive(false));
        masterBar.onValueChanged.AddListener((x) => SoundManager.Instance.masterVolume = x);
        bgmBar.onValueChanged.AddListener((x) => SoundManager.Instance.bgmVolume = x);
        sfxBar.onValueChanged.AddListener((x) => SoundManager.Instance.sfxVolume = x);
    }
}