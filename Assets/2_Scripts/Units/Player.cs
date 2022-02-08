using System;
using Unity.VisualScripting;
using UnityEngine;

[Flags]
public enum FootPos
{
    None = 0,
    Left = 1 << 0,
    Middle = 1 << 1,
    Right = 1 << 2
}

public class Player : Unit
{
    [Header("Mp")]
    public int maxMp;
    public int curMp;

    [Header("Position")] public FootPos curPos = FootPos.Middle;

    public void Move(FootPos footPos)
    {
        switch (footPos)
        {
            case FootPos.Left: transform.position = Vector3.left * 3.5f; break;
            case FootPos.Middle: transform.position = Vector3.zero; break;
            case FootPos.Right: transform.position = Vector3.right * 3.5f; break;
            default: break;
        }
    }
}