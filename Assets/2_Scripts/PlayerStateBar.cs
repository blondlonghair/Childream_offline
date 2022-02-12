using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateBar : MonoBehaviour
{
    [SerializeField] private Image hpBack;
    [SerializeField] private Image hpFront;
    [SerializeField] private Image mpBack;
    [SerializeField] private Image mpFront;

    [SerializeField] private float _hpBackValue = 1;
    [SerializeField] private float _hpFrontValue = 1;
    [SerializeField] private float _mpBackValue = 1;
    [SerializeField] private float _mpFrontValue = 1;

    public float HpValue
    {
        get => _hpFrontValue;
        set 
        { 
            hpFront.fillAmount = value;
            _hpFrontValue = value;
        }
    }
    
    
    public float MpValue
    {
        get => _mpFrontValue;
        set 
        { 
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