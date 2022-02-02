using System;
using UnityEngine;

namespace MonsterSkill
{
    [Serializable]
    public class Skill
    {
        public int id;
        public string name;
        public int power;
        public string desc;

        public virtual void Effect(Monster caster,  Player target) { }
    }

    public class Strike : Skill
    {
        public Strike(int power)
        {
            id = 1;
            name = "타격";
            this.power = power;
            desc = $"피해를 {this.power} 줍니다.";
        }

        public override void Effect(Monster caster,  Player target)
        {
            target.curHp -= power;
        }
    }

    public class Bash : Skill
    {
        private int weakness = 2;
        
        public Bash(int power)
        {
            id = 2;
            name = "강타";
            this.power = power;
        }

        public override void Effect(Monster caster, Player target)
        {
            target.curHp -= this.power;
            target.weakness += this.weakness;
        }
    }
}