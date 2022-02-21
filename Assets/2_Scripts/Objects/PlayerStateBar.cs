using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateBar : MonoBehaviour
{
    [Header("HP")]
    [SerializeField] private Image hpBack;
    [SerializeField] private Image hpFront;
    [SerializeField] private TextMeshProUGUI hpText;
    [Header("MP")]
    [SerializeField] private Image mpBack;
    [SerializeField] private Image mpFront;
    [SerializeField] private TextMeshProUGUI mpText;

    [Header("Value")]
    [SerializeField, Range(0, 1)] private float _hpBackValue = 1;
    [SerializeField, Range(0, 1)] private float _hpFrontValue = 1;
    [SerializeField, Range(0, 1)] private float _mpBackValue = 1;
    [SerializeField, Range(0, 1)] private float _mpFrontValue = 1;

    public float HpValue
    {
        get => _hpFrontValue;
        set
        {
            hpText.text = $"{GameManager.Instance.player.CurHp}<color=#00a8ff>{(GameManager.Instance.player.armor > 0 ? (" + " + GameManager.Instance.player.armor) : ' ')}</color>/ {GameManager.Instance.player.MaxHp}";
            hpFront.fillAmount = value;
            _hpFrontValue = value;
        }
    }
    
    
    public float MpValue
    {
        get => _mpFrontValue;
        set 
        { 
            mpText.text = $"{GameManager.Instance.player.CurMp} / {GameManager.Instance.player.MaxMp}";
            mpFront.fillAmount = value;
            _mpFrontValue = value;
        }
    }

    public void HpLerp()
    {
        StartCoroutine(Co_HpLerp());
    }

    private IEnumerator Co_HpLerp()
    {
        while (!Mathf.Approximately(hpBack.fillAmount, hpFront.fillAmount))
        {
            hpBack.fillAmount = Mathf.Lerp(hpBack.fillAmount, _hpFrontValue, 0.1f);
            yield return YieldCache.WaitForSeconds(0.02f);
        }
    }
    
    public void MpLerp()
    {
        StartCoroutine(Co_MpLerp());
    }

    private IEnumerator Co_MpLerp()
    {
        while (!Mathf.Approximately(mpBack.fillAmount, mpFront.fillAmount))
        {
            mpBack.fillAmount = Mathf.Lerp(mpBack.fillAmount, _mpFrontValue, 0.1f);
            yield return YieldCache.WaitForSeconds(0.02f);
        }
    }
}