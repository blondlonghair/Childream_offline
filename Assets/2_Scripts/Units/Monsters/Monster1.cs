using MonsterSkill;
using Unity.VisualScripting;
using UnityEngine;

public class Monster1 : Monster
{
    protected override void Start()
    {
        _useSkills.Add(new Strike(5));
        _useSkills.Add(new Strike(6));
        
        base.Start();
    }
}