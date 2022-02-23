using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchingDoor : MonoBehaviour
{
    private Animator _animator;
    private Coroutine _doorCoroutine;

    private bool _isCloseOver;
    private bool _isOpenOver;
    
    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void IsCloseOver()
    {
        _isCloseOver = true;
    }

    public void IsOpenOver()
    {
        _isOpenOver = true;
    }
    
    public void OpenDoor(Action action)
    {
        if (_doorCoroutine != null)
        {
            StopCoroutine(_doorCoroutine);
        }
        
        _doorCoroutine = StartCoroutine(Co_OpenDoor(action));
    }

    public void CloseDoor(Action action)
    {
        if (_doorCoroutine != null)
        {
            StopCoroutine(_doorCoroutine);
        }
        
        _doorCoroutine = StartCoroutine(Co_CloseDoor(action));
    }

    private IEnumerator Co_OpenDoor(Action action)
    {
        gameObject.SetActive(true);
        _animator.SetTrigger("DoorOpen");
        yield return null;

        while (!_isOpenOver)
        {
            yield return YieldCache.WaitForSeconds(0.1f);
        }

        action?.Invoke();
    }

    private IEnumerator Co_CloseDoor(Action action)
    {
        gameObject.SetActive(true);
        _animator.SetTrigger("DoorClose");
        yield return null;

        while (!_isCloseOver)
        {
            yield return YieldCache.WaitForSeconds(0.1f);
        }

        action?.Invoke();
    }
}