using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHpBar : MonoBehaviour
{
    [SerializeField] private Image back;
    [SerializeField] private Image front;
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private float backValue = 1;
    [SerializeField] private float frontValue = 1;

    public void SetValue(int armor, int curHp, int maxHp)
    {
        front.fillAmount = (float)curHp / (float)maxHp;
        frontValue = (float)curHp / (float)maxHp;
        text.text = $"{curHp}<color=#00a8ff>{(armor > 0 ? " +" + armor : "")}</color> / {maxHp}";
    }

    public void Lerp()
    {
        StartCoroutine(Co_Lerp());
    }

    private IEnumerator Co_Lerp()
    {
        while (!Mathf.Approximately(back.fillAmount, front.fillAmount))
        {
            back.fillAmount = Mathf.Lerp(back.fillAmount, frontValue, 0.1f);
            yield return YieldCache.WaitForSeconds(0.02f);
        }
    }
}