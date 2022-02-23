using System;
using System.Collections;
using System.Numerics;
using UnityEditor.Build;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

public class Unit : MonoBehaviour
{
    [Header("Hp")]
    [SerializeField] protected int maxHp;
    [SerializeField] protected int curHp;
    [SerializeField] protected int armor;

    [SerializeField] protected int strength;  // 힘     카드로 주는 피해량이 힘 수치만큼 데미지 상승
    [SerializeField] protected int agility;   // 민첩   카드로 얻는 방어도가 민첩 수치만큼 증가
    [SerializeField] protected int vulnerable;// 취약   공격 피해를 입을때 50%의 피해를 추가로 입는다
    [SerializeField] protected int weakness;  // 약화   공격 피해량이 25% 줄어든다
    
    public int Strength
    {
        get => strength;
        set => strength = value < 0 ? 0 : value;
    }

    public int Agility
    {
        get => agility;
        set => agility = value < 0 ? 0 : value;
    }

    public int Vulnerable
    {
        get => vulnerable;
        set => vulnerable = value < 0 ? 0 : value;
    }

    public int Weakness
    {
        get => weakness;
        set => weakness = value < 0 ? 0 : value;
    }

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
        if (curHp <= 0)
        {
            OnDeath();
        }
        
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        _coroutine = StartCoroutine(Co_GetHit());
    }

    public virtual void OnDeath()
    {
        
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