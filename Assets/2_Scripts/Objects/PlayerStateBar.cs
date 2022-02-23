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

    public void SetHpValue(int armor, int curHp, int maxHp)
    {
        hpText.text = $"{curHp}<color=#00a8ff>{(armor > 0 ? " +" + armor : "")}</color> / {maxHp}";
        hpFront.fillAmount = (float)curHp / (float)maxHp;
        _hpFrontValue = (float)curHp / (float)maxHp;
    }

    public void SetMpValue(int armor, int curMp, int maxMp)
    {
        mpText.text = $"{curMp} / {maxMp}";
        mpFront.fillAmount = (float)curMp / (float)maxMp;
        _mpFrontValue = (float)curMp / (float)maxMp;
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