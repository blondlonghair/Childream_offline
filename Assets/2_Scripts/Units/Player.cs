using System;
using UnityEngine;

public class Player : Unit
{
    [Flags]
    enum Hit
    {
        None = 0,
        One = 1 << 0,
        Two = 1 << 1,
        Three = 1 << 2
    }

    [Header("Mp")]
    public int curMp;
    public int maxMp;

    private Hit _nextHit = Hit.None;

    private void Start()
    {
    }
}