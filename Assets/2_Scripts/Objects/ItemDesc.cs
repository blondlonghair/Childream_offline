using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDesc : MonoBehaviour
{
    [SerializeField] private Image descImage;
    [SerializeField] private TextMeshProUGUI descText;

    private Coroutine _coroutine;
    private Color _color = new Color(1,1,1,0);

    public void TextOn(string str)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(Co_Desc(str, true));
    }

    public void TextOff(string str)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(Co_Desc(str, false));
    }

    private IEnumerator Co_Desc(string str, bool isOn)
    {
        descText.text = str;
        
        while (isOn ? _color.a <= 1 : _color.a >= 0)
        {
            _color.a += isOn ? 0.01f : -0.01f;

            descImage.color = _color;
            descText.color = _color;

            yield return YieldCache.WaitForSeconds(0.01f);
        }
    }
}