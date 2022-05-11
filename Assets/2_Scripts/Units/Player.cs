using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[Flags]
public enum FootPos
{
    None = 0,
    Left = 1 << 0,
    Middle = 1 << 1,
    Right = 1 << 2,
}

public class Player : Unit
{
    [Header("Mp")]
    protected int maxMp;
    protected int curMp;

    public int CurHp
    {
        get => curHp;
        set
        { 
            curHp = value;
            stateBar.SetHpValue(armor, curHp, maxHp);
        }
    }

    public int MaxHp
    {
        get => maxHp;
        set
        {
            maxHp = value; 
            stateBar.SetHpValue(armor, curHp, maxHp);
        }
    }

    public int Armor
    {
        get => armor;
        set
        {
            armor = value;
            stateBar.SetHpValue(armor, curHp, maxHp);
            if (value <= 0)
                EffectManager.Instance.InitEffect("Defence_Broke", transform);
        }
    }
    
    public int CurMp
    {
        get => curMp;
        set
        {
            curMp = value; 
            stateBar.SetMpValue(armor, curMp, maxMp);
        }
    }

    public int MaxMp
    {
        get => maxMp;
        set
        {
            maxMp = value; 
            stateBar.SetMpValue(armor, curMp, maxMp);
        }
    }

    [Header("Position")] public FootPos curPos = FootPos.Middle;

    public PlayerStateBar stateBar;

    private Coroutine _moveCoroutine;

    public void Move(FootPos footPos)
    {
        if (_moveCoroutine != null)
        {
            StopCoroutine(_moveCoroutine);
        }
        
        switch (footPos)
        {
            case FootPos.Left: 
                _moveCoroutine = StartCoroutine(Co_Move(new Vector3(-3.5f, -1, 0))); curPos = footPos; 
                break;
            case FootPos.Middle: 
                _moveCoroutine = StartCoroutine(Co_Move(new Vector3(0, -1, 0))); curPos = footPos;
                break;
            case FootPos.Right: 
                _moveCoroutine = StartCoroutine(Co_Move(new Vector3(3.5f, -1, 0))); curPos = footPos;
                break;
        }
    }

    private IEnumerator Co_Move(Vector3 pos)
    {
        while (!Helper.Approximately(transform.position, pos))
        {
            transform.position = Vector3.Lerp(transform.position, pos, 0.1f);
            
            yield return YieldCache.WaitForSeconds(0.01f);
        }
    }
    
    protected override IEnumerator Co_Attack()
    {
        Vector3 originPos = transform.position;
        
        while (transform.position.y < originPos.y + 1.5)
        {
            transform.position = Vector3.Lerp(transform.position, originPos + Vector3.up * 2, 0.1f);
            yield return YieldCache.WaitForSeconds(0.01f);
        }

        while (!Mathf.Approximately(transform.position.y, originPos.y))
        {
            transform.position = Vector3.Lerp(transform.position, originPos, 0.1f);
            yield return YieldCache.WaitForSeconds(0.01f);
        }
    }

    public override void GetHit()
    {
        stateBar.SetHpValue(armor, curHp, maxHp);
        SoundManager.Instance.PlaySFXSound("PlayerHit");

        base.GetHit();
    }

    public override void OnDeath()
    {
        SoundManager.Instance.PlaySFXSound("PlayerDie");

        base.OnDeath();
    }
    
    protected override IEnumerator Co_GetHit()
    {
        Vector3 originPos = transform.position;

        for (int i = 0; i < 2; i++)
        {
            while (!Mathf.Approximately(transform.position.x, originPos.x - 0.5f))
            {
                transform.position = Vector3.Lerp(transform.position, originPos + Vector3.left * 0.5f, 0.9f);
                yield return YieldCache.WaitForSeconds(0.01f);
            }

            while (!Mathf.Approximately(transform.position.x, originPos.x + 0.5f))
            {
                transform.position = Vector3.Lerp(transform.position, originPos + Vector3.right * 0.5f, 0.9f);
                yield return YieldCache.WaitForSeconds(0.01f);
            }
        }

        while (!Mathf.Approximately(transform.position.x, originPos.x))
        {
            transform.position = Vector3.Lerp(transform.position, originPos, 0.9f);
            yield return YieldCache.WaitForSeconds(0.01f);
        }
    }
    
    public void GetDamage(int damage)
    {
        int getDamage = (int)((float)damage * (Vulnerable > 0 ? 1.5f : 1));

        Instantiate(damageText, transform.position, Quaternion.identity).TextOn(getDamage);

        for (int i = 0; i < getDamage; i++)
        {
            if (Armor > 0)
            {
                Armor--;
            }
            else
            {
                CurHp--;
            }
        }
        
        GetHit();
    }
}