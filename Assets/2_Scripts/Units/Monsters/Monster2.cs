using MonsterSkill;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Monster2 : Monster
{
    protected override void Start()
    {
        useSkills.Add(new Strike(6, FootPos.Middle));
        useSkills.Add(new StrikeRandom(8));
        
        base.Start();
    }
}