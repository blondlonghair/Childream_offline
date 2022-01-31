using System.Collections.Generic;
using UnityEngine;
using MonsterSkill;

public enum NextAttackPos
{
    None = 0,
    Left = 1,
    Middle = 2,
    Right = 4
}

public class Monster : Unit
{
    protected List<Skill> _monstersBuffer = new List<Skill>();
    [SerializeField] protected Skill[] useSkills;

    public NextAttackPos nextAttackPos = NextAttackPos.None;

    public Monster()
    {
    }
}