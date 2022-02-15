using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchingDoor : MonoBehaviour
{
    public bool isCloseOver { get; set; }
    public bool isOpenOver { get; set; }
    private Animator animator;

    public void IsCloseOver()
    {
        isCloseOver = true;
    }

    public void IsOpenOver()
    {
        isOpenOver = true;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (SceneManager.GetActiveScene().name == "Ingame")
        {
            animator.SetTrigger("DoorOpen");
        }
    }

    private void Update()
    {
        if (isOpenOver)
        {
            gameObject.SetActive(false);
        }
    }

    public void OpenDoor()
    {
        animator.SetTrigger("DoorOpen");
        gameObject.SetActive(false);
    }

    public void CloseDoor(Action action)
    {
        StartCoroutine(Co_CloseDoor(action));
    }

    private IEnumerator Co_CloseDoor(Action action)
    {
        gameObject.SetActive(true);
        animator.SetTrigger("DoorClose");
        yield return null;

        while (!isCloseOver)
        {
            yield return YieldCache.WaitForSeconds(0.1f);
        }

        action.Invoke();
    }
}