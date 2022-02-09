using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHpBar : MonoBehaviour
{
    [SerializeField] private Image back;
    [SerializeField] private Image front;

    private float _backValue;
    private float _frontValue;

    public float Value
    {
        get => _frontValue;
        set 
        { 
            front.fillAmount = value;
            _frontValue = value;
        }
    }

    public void Lerp()
    {
        StartCoroutine(Co_Lerp());
    }

    private IEnumerator Co_Lerp()
    {
        while (Mathf.Approximately(back.fillAmount, front.fillAmount))
        {
            back.fillAmount = Mathf.Lerp(back.fillAmount, _frontValue, 0.5f);
            yield return YieldCache.WaitForSeconds(0.01f);
        }
    }
}