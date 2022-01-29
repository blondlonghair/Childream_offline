using System.Collections.Generic;
using UnityEngine;

namespace OffLine
{
    public enum NextAttackPos
    {
        None = 0,
        Left  = 1,
        Middle = 2,
        Right = 4
    }
    
    public class Monster : Unit
    {
        protected List<MonsterSkill> _monstersBuffer = new List<MonsterSkill>();
        [SerializeField] protected MonsterSkill[] useSkills;

        public NextAttackPos nextAttackPos = NextAttackPos.None;

        public Monster()
        {
            
        }
    }
}