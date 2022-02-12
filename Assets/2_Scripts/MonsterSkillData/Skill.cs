﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;

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

        public virtual void Effect(Monster caster, Player target)
        {
            target.stateBar.HpValue = (float)target.curHp / (float)target.maxHp;
        }
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
         
            base.Effect(caster, target);
        }
    }

    public class StrikeRandom : Skill
    {
        public StrikeRandom(int power)
        {
            id = 2;
            name = "랜덤 타격";
            this.power = power;
            this.attackPos = Random.Range(0, 3) switch{0 => FootPos.Left, 1=> FootPos.Middle, 2 => FootPos.Right, _ => FootPos.None};
            desc = $"피해를 {this.power} 줍니다.";
        }

        public override void Effect(Monster caster,  Player target)
        {
            target.curHp -= power;
         
            base.Effect(caster, target);
        }
    }
    
    public class StrikeAll : Skill
    {
        public StrikeAll(int power)
        {
            id = 3;
            name = "타격";
            this.power = power;
            this.attackPos = FootPos.Left | FootPos.Middle | FootPos.Right;
            desc = $"피해를 {this.power} 줍니다.";
        }

        public override void Effect(Monster caster,  Player target)
        {
            target.curHp -= power;
            
            base.Effect(caster, target);
        }
    }

    public class Armor : Skill
    {
        public Armor(int power)
        {
            id = 4;
            name = "방어";
            this.power = power;
        }

        public override void Effect(Monster caster, Player target)
        {
            caster.armor += power;
            
            base.Effect(caster, target);
        }
    }

    public class Strength : Skill
    {
        public Strength(int power)
        {
            id = 5;
            name = "힘 회복";
            this.power = power;
        }

        public override void Effect(Monster caster, Player target)
        {
            caster.Strength += power;
            
            base.Effect(caster, target);
        }
    }
    
    public class Agility : Skill
    {
        public Agility(int power)
        {
            id = 6;
            name = "민첩 회복";
            this.power = power;
        }

        public override void Effect(Monster caster, Player target)
        {
            caster.Agility += power;
            
            base.Effect(caster, target);
        }
    }

    public class Vulnerable : Skill
    {
        public Vulnerable(int power)
        {
            id = 7;
            name = "Vulnerable";
            this.power = power;
            this.attackPos = FootPos.Left | FootPos.Middle | FootPos.Right;
        }

        public override void Effect(Monster caster, Player target)
        {
            target.Vulnerable += power;
            
            base.Effect(caster, target);
        }
    }

    public class Weakness : Skill
    {
        public Weakness(int power)
        {
            id = 8;
            name = "Weakness";
            this.power = power;
            this.attackPos = FootPos.Left | FootPos.Middle | FootPos.Right;
        }

        public override void Effect(Monster caster, Player target)
        {
            target.Weakness += power;
            
            base.Effect(caster, target);
        }
    }
}