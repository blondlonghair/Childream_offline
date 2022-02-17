using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingPanel : MonoBehaviour
{
    public bool isEnd;
    
    private Coroutine _coroutine;

    private void Start()
    {
        Open();
        if (TryGetComponent(out SpriteRenderer spriteRenderer))
            spriteRenderer.enabled = true;
    }

    public void Open()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        _coroutine = StartCoroutine(Co_OpenPanel());
    }

    public void Close(Action action)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }

        _coroutine = StartCoroutine(Co_ClosePanel(action));
    }

    IEnumerator Co_OpenPanel()
    {
        while (transform.position.y < 39.9f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, 40, 0), 0.05f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator Co_ClosePanel(Action action)
    {
        while (transform.position.y > 0.1f)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0, 0), 0.05f);
            yield return new WaitForSeconds(0.01f);
        }

        action.Invoke();
    }
}
