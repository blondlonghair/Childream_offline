using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHpBar : MonoBehaviour
{
    [SerializeField] private Image back;
    [SerializeField] private Image front;

    [SerializeField] private float _backValue = 1;
    [SerializeField] private float _frontValue = 1;

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
        while (!Mathf.Approximately(back.fillAmount, front.fillAmount))
        {
            back.fillAmount = Mathf.Lerp(back.fillAmount, _frontValue, 0.1f);
            yield return YieldCache.WaitForSeconds(0.02f);
        }
    }
}