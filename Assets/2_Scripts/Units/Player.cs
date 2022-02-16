using System;
using Unity.VisualScripting;
using UnityEngine;

[Flags]
public enum FootPos
{
    None = 0,
    Left = 1 << 0,
    Middle = 1 << 1,
    Right = 1 << 2,
    All = 1 << 3
}

public class Player : Unit
{
    [Header("Mp")]
    public int maxMp;
    public int curMp;

    public int CurHp
    {
        get => curHp;
        set
        {
            curHp = value;
            stateBar.HpValue = (float)curHp / (float)maxHp;
            // GetHit();
        }
    }

    public int MaxHp
    {
        get => maxHp;
        set
        {
            maxHp = value; 
            stateBar.HpValue = (float)curHp / (float)maxHp;
        }
    }
    
    public int CurMp
    {
        get => curMp;
        set
        {
            curMp = value; 
            stateBar.MpValue = (float)curMp / (float)maxMp;
        }
    }

    public int MaxMp
    {
        get => maxMp;
        set
        {
            maxMp = value; 
            stateBar.MpValue = (float)curMp / (float)maxMp;
        }
    }

    [Header("Position")] public FootPos curPos = FootPos.Middle;

    public PlayerStateBar stateBar;

    public void Move(FootPos footPos)
    {
        switch (footPos)
        {
            case FootPos.Left: transform.position = Vector3.left * 3.5f; break;
            case FootPos.Middle: transform.position = Vector3.zero; break;
            case FootPos.Right: transform.position = Vector3.right * 3.5f; break;
        }
    }

    public override void GetHit()
    {
        stateBar.HpValue = (float)curHp / (float)maxHp;

        base.GetHit();
    }

    public override void OnDeath()
    {
        base.OnDeath();
    }
}