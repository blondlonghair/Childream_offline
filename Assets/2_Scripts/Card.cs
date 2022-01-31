using System;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    public int id;
    public string name;
    public int cost;
    public int power;
    public string desc;
    
    public virtual void Effect(Player caster, Monster target) { }
    public virtual void Upgrade() { }
}

namespace Cards
{
    public class Strike : Card
    {
        public Strike()
        {
            id = 1;
            name = "타격";
            cost = 1;
            power = 6;
            desc = $"피해를 {power} 줍니다.";
        }

        public override void Effect(Player caster, Monster target)
        {
            target.curHp -= power;
            caster.curMp -= cost;
            
            base.Effect(caster, target);
        }

        public override void Upgrade()
        {
            power = 8;
        
            base.Upgrade();
        }
    }

    public class Bash : Card
    {
        private int _weakness = 2;

        public Bash()
        {
            id = 2;
            name = "강타";
            cost = 2;
            power = 8;
            desc = $"피해를 {power} 줍니다.\n취약을 {_weakness} 부여합니다.";
        }

        public override void Effect(Player caster, Monster target)
        {
            target.curHp -= power;
            target.weakness += _weakness;

            caster.curMp -= cost;
            
            base.Effect(caster, target);
        }

        public override void Upgrade()
        {
            power = 10;
            _weakness = 3;
            
            base.Upgrade();
        }
    }

    public class PommelStrike : Card
    {
        private int _drowCard = 1;

        public PommelStrike()
        {
            id = 3;
            name = "폼멜타격";
            cost = 1;
            power = 9;
            desc = $"피해를 {power} 줍니다.\n카드를 {_drowCard}장 뽑습니다.";
        }

        public override void Effect(Player caster, Monster target)
        {
            target.curHp -= power;
            //카드 드로우
            
            base.Effect(caster, target);
        }

        public override void Upgrade()
        {
            power = 10;
            _drowCard = 2;
        
            base.Upgrade();
        }
    }

    public class Defend : Card
    {
        public Defend()
        {
            id = 4;
            name = "수비";
            cost = 1;
            power = 5;
            desc = $"방어도를 {power} 얻습니다.";
        }

        public override void Effect(Player caster, Monster target)
        {
            caster.armor += power;
            
            base.Effect(caster, target);
        }

        public override void Upgrade()
        {
            power = 8;
            
            base.Upgrade();
        }
    }
}

// public class CardData : SingletonMono<CardData>
// {
//     public Dictionary<string, Card> data = new Dictionary<string, Card>();
//
//     
//     private void Awake()
//     {
//         data.Add(c1.name, c1);
//         data.Add(c2.name, c1);
//         data.Add(c3.name, c1);
//     }
// }


