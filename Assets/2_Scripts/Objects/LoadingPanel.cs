using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingPanel : MonoBehaviour
{
    private Coroutine _coroutine;

    private void Start()
    {
        Open(null);
        if (TryGetComponent(out SpriteRenderer spriteRenderer))
            spriteRenderer.enabled = true;
    }

    public void Open(Action action)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        _coroutine = StartCoroutine(Co_OpenPanel(action));
    }

    public void Close(Action action)
    {
        SoundManager.Instance.PlaySFXSound("BookSlide");
        
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(Co_ClosePanel(action));
    }

    IEnumerator Co_OpenPanel(Action action)
    {
        while (transform.position.y < 39.9f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, 40, 0), 0.05f);
            yield return new WaitForSeconds(0.01f);
        }
        
        action?.Invoke();
    }

    IEnumerator Co_ClosePanel(Action action)
    {
        while (transform.position.y > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0, 0), 0.05f);
            yield return new WaitForSeconds(0.01f);
        }

        action?.Invoke();
    }
}
