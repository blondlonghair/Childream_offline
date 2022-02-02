using MonsterSkill;
using UnityEngine;

public class Monster2 : Monster
{
    protected override void Start()
    {
        _useSkills.Add(new Strike(6));
        _useSkills.Add(new Bash(8));
        
        base.Start();
    }
}