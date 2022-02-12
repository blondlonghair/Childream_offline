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

    [SerializeField] private int strength;
    [SerializeField] private int agility;
    [SerializeField] private int vulnerable;
    [SerializeField] private int weakness;
    
    public int Strength
    {
        get => strength;
        set => strength = value < strength ? 0 : value;
    }//힘

    public int Agility
    {
        get => agility;
        set => agility = value < agility ? 0 : value;
    }//민첩

    public int Vulnerable
    {
        get => vulnerable;
        set => vulnerable = value < vulnerable ? 0 : value;
    }//취약

    public int Weakness
    {
        get => weakness;
        set => weakness = value < weakness ? 0 : value;
    }//약화

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