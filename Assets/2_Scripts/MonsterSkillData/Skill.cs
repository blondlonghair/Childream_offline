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
        public FootPos attackPos; 

        public virtual void Effect(Monster caster,  Player target) { }
    }

    public class Strike : Skill
    {
        public Strike(int power, FootPos attackPos)
        {
            id = 1;
            name = "타격";
            this.power = power;
            this.attackPos = attackPos;
            desc = $"피해를 {this.power} 줍니다.";
        }

        public override void Effect(Monster caster,  Player target)
        {
            
            target.curHp -= power;
        }
    }
    
    public class StrikeAll : Skill
    {
        public StrikeAll(int power)
        {
            id = 1;
            name = "타격";
            this.power = power;
            this.attackPos = FootPos.Left | FootPos.Middle | FootPos.Right;
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
        
        public Bash(int power, FootPos attackPos)
        {
            id = 2;
            name = "강타";
            this.power = power;
            this.attackPos = attackPos;
        }

        public override void Effect(Monster caster, Player target)
        {
            target.curHp -= this.power;
            target.weakness += this.weakness;
        }
    }
}