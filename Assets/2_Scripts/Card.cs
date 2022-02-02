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

    public bool isUpgrade;

    public virtual void Effect(Player caster, Monster target)
    {
        caster.curMp -= cost;
    }

    public virtual void Upgrade()
    {
        isUpgrade = true;
    }
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
        private int _drawCard = 1;

        public PommelStrike()
        {
            id = 3;
            name = "폼멜타격";
            cost = 1;
            power = 9;
            desc = $"피해를 {power} 줍니다.\n카드를 {_drawCard}장 뽑습니다.";
        }

        public override void Effect(Player caster, Monster target)
        {
            target.curHp -= power;
            CardManager.Instance.DrawCard();
            
            base.Effect(caster, target);
        }

        public override void Upgrade()
        {
            power = 10;
            _drawCard = 2;
        
            base.Upgrade();
        }
    }

    public class Defend : Card
    {
        public Defend()
        {
            id = 10;
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

    public class ShrugItOff : Card
    {
        private int _drawCard = 1;

        public ShrugItOff()
        {
            id = 11;
            name = "흘려보내기";
            cost = 1;
            power = 8;
            desc = $"방어도를 {power} 얻습니다.\n카드를 {_drawCard}장 뽑습니다.";
        }

        public override void Effect(Player caster, Monster target)
        {
            caster.armor += power;
            CardManager.Instance.DrawCard();
            
            base.Effect(caster, target);
        }

        public override void Upgrade()
        {
            power = 11;
            
            base.Upgrade();
        }
    }

    public class Entrench : Card
    {
        public Entrench()
        {
            id = 12;
            name = "참호";
            cost = 2;
            power = 2;
            desc = $"방어도가 {power}배로 증가합니다.";
        }

        public override void Effect(Player caster, Monster target)
        {
            caster.armor *= power;
            
            base.Effect(caster, target);
        }

        public override void Upgrade()
        {
            cost = 1;
            
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


