using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpriteEffect : MonoBehaviour
{
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
            transform.localScale += Vector3.one * 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(gameObject);
    }
}