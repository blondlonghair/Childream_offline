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

    public void Move(FootPos footPos)
    {
        switch (footPos)
        {
            case FootPos.Left: StartCoroutine(Co_Move(new Vector3(-3.5f, -1, 0))); curPos = footPos; break;
            case FootPos.Middle: StartCoroutine(Co_Move(new Vector3(0, -1, 0))); curPos = footPos;break;
            case FootPos.Right: StartCoroutine(Co_Move(new Vector3(3.5f, -1, 0))); curPos = footPos;break;
        }

    }

    private IEnumerator Co_Move(Vector3 pos)
    {
        while (!Mathf.Approximately(transform.position.x, pos.x))
        {
            transform.position = Vector3.Lerp(transform.position, pos, 0.1f);
            yield return YieldCache.WaitForSeconds(0.01f);
        }
    }

    public override void GetHit()
    {
        // stateBar.HpValue = (float)curHp / (float)maxHp;
        stateBar.SetHpValue(armor, curHp, maxHp);

        base.GetHit();
    }

    public override void OnDeath()
    {
        base.OnDeath();
    }
    
    public void GetDamage(int damage)
    {
        int getDamage = (int)((float)damage * (Vulnerable > 0 ? 1.5f : 1));

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
        
        
    }
}