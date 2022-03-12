using System;
using System.Collections;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using Random = UnityEngine.Random;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;
    [SerializeField] private Rigidbody2D rigidbody2D;

    private void Start()
    {
        rigidbody2D.AddForce(new Vector3(Random.Range(-5, 5), 15), ForceMode2D.Impulse);

        StartCoroutine(Co_Destroy());
    }

    public void TextOn(int damage)
    {
        text.text = damage.ToString();
    }

    private IEnumerator Co_Destroy()
    {
        yield return YieldCache.WaitForSeconds(0.7f);
        
        Color color = text.color;
        while (color.a > 0.1)
        {
            color.a -= 0.1f;
            text.color = color;
            yield return YieldCache.WaitForSeconds(0.01f);
        }
        
        Destroy(gameObject);
        yield return null;
    }
}
