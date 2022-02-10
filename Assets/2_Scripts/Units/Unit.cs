using System;
using System.Collections;
using System.Numerics;
using UnityEditor.Build;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class Unit : MonoBehaviour
{
    [Header("Hp")]
    public int maxHp;
    public int curHp;
    public int armor;
    
    [Header("State")]
    public int strength = 0;    //힘
    public int agility = 0;     //민첩
    public int vulnerable = 0;  //취약
    public int weakness = 0;    //약화

    private Coroutine _coroutine;

    public virtual void Attack()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        _coroutine = StartCoroutine(Co_Attack());
    }

    public virtual void GetHit()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        _coroutine = StartCoroutine(Co_GetHit());
    }

    protected virtual IEnumerator Co_Attack()
    {
        return null;
    }

    protected virtual IEnumerator Co_GetHit()
    {
        return null;
    }
}