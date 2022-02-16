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

    [SerializeField] private int strength;  // 힘     카드로 주는 피해량이 힘 수치만큼 데미지 상승
    [SerializeField] private int agility;   // 민첩   카드로 얻는 방어도가 민첩 수치만큼 증가
    [SerializeField] private int vulnerable;// 취약   공격 피해를 입을때 50%의 피해를 추가로 입는다
    [SerializeField] private int weakness;  // 약화   공격 피해량이 25% 줄어든다
    
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