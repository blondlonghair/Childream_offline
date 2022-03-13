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
        GoDown,
        Grid
    }

    [SerializeField] private EffectType effectType;
    
    private SpriteRenderer _spriteRenderer;
    
    private void Start()
    {
        TryGetComponent(out _spriteRenderer);

        switch (effectType)
        {
            case EffectType.Normal:
                StartCoroutine(Co_DestroyNormal());
                break;
            case EffectType.GoDown:
            case EffectType.GoUp:
                StartCoroutine(Co_DestroySide());
                break;
            case EffectType.Grid:
                StartCoroutine(Co_DestroyGrid());
                break;
        }
    }

    private IEnumerator Co_DestroyNormal()
    {
        Color color = _spriteRenderer.color;

        yield return YieldCache.WaitForSeconds(0.5f);

        while (_spriteRenderer.color.a >= 0)
        {
            color.a -= 0.1f;
            _spriteRenderer.color = color;
            
            transform.localScale += Vector3.one * 0.1f;
            
            yield return YieldCache.WaitForSeconds(0.01f);
        }
        
        Destroy(gameObject);
        yield return null;
    }

    private IEnumerator Co_DestroySide()
    {
        Color color = _spriteRenderer.color;

        yield return YieldCache.WaitForSeconds(0.5f);

        while (color.a >= 0)
        {
            color.a -= 0.1f;
            _spriteRenderer.color = color;

            if (effectType == EffectType.GoDown)
            {
                transform.position += Vector3.down * 0.1f;
            }

            else if (effectType == EffectType.GoUp)
            {
                transform.position += Vector3.up * 0.1f;
            }
            
            yield return YieldCache.WaitForSeconds(0.01f);
        }
        
        Destroy(gameObject);
        yield return null;
    }

    private IEnumerator Co_DestroyGrid()
    {
        _spriteRenderer.color = new Color(1, 1, 1, 0);
        Color color = _spriteRenderer.color;

        while (color.a < 1)
        {
            color.a += 0.1f;
            _spriteRenderer.color = color;

            yield return YieldCache.WaitForSeconds(0.01f);
        }

        yield return YieldCache.WaitForSeconds(0.5f);
        
        while (color.a >= 0)
        {
            color.a -= 0.1f;
            _spriteRenderer.color = color;

            yield return YieldCache.WaitForSeconds(0.01f);
        }
        
        Destroy(gameObject);
        yield return null;
    }
}