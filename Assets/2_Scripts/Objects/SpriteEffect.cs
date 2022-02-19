using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpriteEffect : MonoBehaviour
{
    private enum EffectType
    {
        Normal,
        GoUp,
        GoDown
    }

    [SerializeField] private EffectType effectType;
    
    private SpriteRenderer _spriteRenderer;
    private Vector3 _firstScale;
    
    private void Start()
    {
        TryGetComponent(out _spriteRenderer);
        _firstScale = transform.localScale;

        StartCoroutine(DestoryEffect());
    }

    private IEnumerator DestoryEffect()
    {
        Color color = _spriteRenderer.color;

        yield return new WaitForSeconds(0.5f);

        while (_spriteRenderer.color.a >= 0)
        {
            color.a -= 0.1f;
            _spriteRenderer.color = color;
            
            if (effectType == EffectType.Normal)
            {
                transform.localScale += Vector3.one * 0.1f;
            }

            if (effectType == EffectType.GoDown)
            {
                transform.position += Vector3.down * 0.1f;
            }

            if (effectType == EffectType.GoUp)
            {
                transform.position += Vector3.up * 0.1f;
            }
            
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(gameObject);
    }
}