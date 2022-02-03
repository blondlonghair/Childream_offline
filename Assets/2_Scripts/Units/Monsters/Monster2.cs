using MonsterSkill;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Monster2 : Monster
{
    protected override void Start()
    {
        _useSkills.Add(new Strike(6, FootPos.Middle));
        _useSkills.Add(new Bash(8, FootPos.Middle));
        
        base.Start();
    }
}