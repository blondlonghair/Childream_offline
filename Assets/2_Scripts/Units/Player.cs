using System;
using UnityEngine;

public class Player : Unit
{
    [Flags]
    enum Hit
    {
        none = 0,
        one = 1 << 0,
        two = 1 << 1,
        three = 1 << 2
    }

    public int curMp;
    public int maxMp;
    
    private Hit _hit = Hit.one | Hit.two | Hit.three;
}