using System.Collections;
using MonsterSkill;
using UnityEngine;

public class Monster9 : Monster
{
    protected override void Start()
    {
        TryGetComponent(out _animator);

        useSkills.Add(new Strike(6, FootPos.Middle));
        
        base.Start();
    }
}