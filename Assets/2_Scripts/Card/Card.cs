using System;
using System.Collections.Generic;
using Mono.CompilerServices.SymbolWriter;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class Card
{
    public enum CardType
    {
        None = 0,
        One = 1,
        Grid = 2,
        All = 4
    }
    public int id;
    public string name;
    public int cost;
    public int power;
    public string desc;

    public bool isUpgrade;
    public CardType type;

    public virtual void Effect(Player caster, params Monster[] target)
    {
        if (caster.curMp < cost)
        {
            return;
        }

        caster.CurMp -= cost;

        if (target != null)
        {
            foreach (var monster in target)
            {
                monster.GetHit();
            }

            EffectManager.Instance.InitEffect(/*this.GetType().Name*/"Strike", target[0]?.transform);
        }
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
            type = CardType.One;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].curHp -= power;

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
            type = CardType.One;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].curHp -= power;
            target[0].Weakness += _weakness;

            base.Effect(caster, target);
        }

        public override void Upgrade()
        {
            power = 10;
            _weakness = 3;
            
            base.Upgrade();
        }
    }

    public class BodySlam : Card
    {
        public BodySlam()
        {
            id = 3;
            name = "몸통박치기";
            cost = 1;
            desc = $"현재 방어도만큼 피해를 줍니다.";
            type = CardType.One;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].curHp -= caster.armor;

            base.Effect(caster, target);
        }
    }

    public class Cleave : Card
    {
        public Cleave()
        {
            id = 4;
            name = "절단";
            cost = 1;
            power = 8;
            desc = $"적 전체에게 피해를 {power} 줍니다.";
            type = CardType.All;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            foreach (var monster in target)
            {
                monster.curHp -= power;
            }

            base.Effect(caster, target);
        }
    }

    public class Hemokinesis : Card
    {
        private int _loseHealth = 2;
        
        public Hemokinesis()
        {
            id = 5;
            name = "혈류";
            cost = 1;
            power = 15;
            desc = $"체력을 {_loseHealth} 잃습니다. 피해를 {power} 줍니다.";
            type = CardType.One;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].curHp -= power;
            caster.curHp -= _loseHealth;

            base.Effect(caster, target);
        }
    }

    public class HeavyBlade : Card
    {
        private int _powerUp;
        
        public HeavyBlade()
        {
            id = 6;
            name = "대검";
            cost = 2;
            power = 14;
            desc = $"피해 {power} 줍니다. 힘의 효과가 {_powerUp}배로 적용됩니다.";
            type = CardType.One;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].curHp -= power + caster.Strength * _powerUp;

            base.Effect(caster, target);
        }
    }

    public class IronWave : Card
    {
        private int _armor;

        public IronWave()
        {
            id = 7;
            name = "철의 파동";
            cost = 2;
            power = 5;
            desc = $"방어도를 {power} 얻습니다. 피해를 {_armor} 줍니다.";
            type = CardType.One;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].curHp -= power;
            caster.armor += _armor;

            base.Effect(caster, target);
        }
    }

    public class PommelStrike : Card
    {
        private int _drawCard = 1;

        public PommelStrike()
        {
            id = 8;
            name = "폼멜타격";
            cost = 1;
            power = 9;
            desc = $"피해를 {power} 줍니다.\n카드를 {_drawCard}장 뽑습니다.";
            type = CardType.One;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].curHp -= power;
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

    public class ThunderClap : Card
    {
        private int _vulnerable = 1;

        public ThunderClap()
        {
            id = 9;
            name = "천둥";
            cost = 1;
            power = 4;
            desc = $"적 전체에게 피해를 {power} 주고 취약을 {_vulnerable} 부여합니다.";
            type = CardType.All;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            foreach (var monster in target)
            {
                monster.curHp -= power;
                monster.Vulnerable -= _vulnerable;
            }
            
            base.Effect(caster, target);
        }
    }

    public class Clothesline : Card
    {
        private int _weakness = 2;

        public Clothesline()
        {
            id = 10;
            name = "클로스라인";
            cost = 2;
            power = 12;
            desc = $"피해 {power} 줍니다. 약화를 {_weakness} 부여합니다.";
            type = CardType.One;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].curHp -= power;
            
            base.Effect(caster, target);
        }
    }

    public class Dropkick : Card
    {
        private int _energy;

        public Dropkick()
        {
            id = 11;
            name = "드롭킥";
            cost = 1;
            power = 5;
            desc = $"피해를 {power} 줍니다. 적이 취약 상태라면 {_energy} 에너지를 얻고 카드 1장을 뽑습니다.";
            type = CardType.One;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            if (target[0].Vulnerable > 0)
            {
                caster.CurMp += _energy;
                CardManager.Instance.DrawCard();
            }

            target[0].curHp -= power;
            
            base.Effect(caster, target);
        }
    }

    public class ServerSoul : Card
    {
        public ServerSoul()
        {
            id = 12;
            name = "영혼 절단";
            cost = 2;
            power = 16;
            desc = $"손에 있는 모든 카드를 소멸시킵니다. 피해를 {power} 줍니다.";
            type = CardType.One;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            foreach (var card in CardManager.Instance.cards)
            {
                CardManager.Instance.DestroyCard(card);
            }

            target[0].curHp -= power;
            
            base.Effect(caster, target);
        }
    }

    public class Upercut : Card
    {
        private int _vulnerable = 1;
        private int _weakness = 1;

        public Upercut()
        {
            id = 13;
            name = "어퍼컷";
            cost = 2;
            power = 13;
            desc = $"피해를 {power} 줍니다. 약화를 {_vulnerable} 부여합니다. 취약을 {_weakness} 부여합니다.";
            type = CardType.One;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].curHp -= power;
            target[0].Vulnerable += _vulnerable;
            target[0].Weakness += _weakness;
            
            base.Effect(caster, target);
        }
    }

    public class Bludgeon : Card
    {
        public Bludgeon()
        {
            id = 14;
            name = "몽둥이질";
            cost = 3;
            power = 32;
            desc = $"피해를 {power} 줍니다.";
            type = CardType.One;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].curHp -= power;
            
            base.Effect(caster, target);
        }
    }
    
    public class Defend : Card
    {
        public Defend()
        {
            id = 15;
            name = "수비";
            cost = 1;
            power = 5;
            desc = $"방어도를 {power} 얻습니다.";
            type = CardType.None;
        }

        public override void Effect(Player caster, params Monster[] target)
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
            id = 16;
            name = "흘려보내기";
            cost = 1;
            power = 8;
            desc = $"방어도를 {power} 얻습니다. 카드를 {_drawCard}장 뽑습니다.";
            type = CardType.None;
        }

        public override void Effect(Player caster, params Monster[] target)
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

    public class TrueGrit : Card
    {
        public TrueGrit()
        {
            id = 17;
            name = "진정한 끈기";
            cost = 1;
            power = 7;
            desc = $"방어도를 {power} 얻습니다. 무작위 카드를 1장 소멸시킵니다.";
            type = CardType.None;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            caster.armor += power;
            CardManager.Instance.DestroyCard(CardManager.Instance.cards[Random.Range(0, CardManager.Instance.cards.Count)]);
            
            base.Effect(caster, target);
        }
    }

    public class Entrench : Card
    {
        public Entrench()
        {
            id = 18;
            name = "참호";
            cost = 2;
            power = 2;
            desc = $"방어도가 {power}배로 증가합니다.";
            type = CardType.None;
        }

        public override void Effect(Player caster, params Monster[] target)
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

    public class SecondWind : Card
    {
        public SecondWind()
        {
            id = 19;
            name = "기사회생";
            cost = 1;
            power = 5;
            desc = $"모든 카드를 소멸시키고 그 수만큼 방어도를 {power} 얻습니다.";
            type = CardType.None;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            caster.armor += power * CardManager.Instance.cards.Count;
            
            foreach (var card in CardManager.Instance.cards)
            {
                CardManager.Instance.DestroyCard(card);
            }
            
            base.Effect(caster, target);
        }
    }

    public class Move : Card
    {
        public Move()
        {
            id = 20;
            name = "이동";
            cost = 1;
            power = 0;
            desc = "한칸을 선택해서 이동합니다.";
            type = CardType.Grid;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            base.Effect(caster, target);
        }
    }

    public class BloodLetting : Card
    {
        private int _loseHealth = 3;
        
        public BloodLetting()
        {
            id = 21;
            name = "사혈";
            cost = 0;
            power = 2;
            desc = $"체력을 {_loseHealth} 잃습니다. {power}에너지를 얻습니다.";
            type = CardType.None;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            caster.curHp -= _loseHealth;
            caster.CurMp += power;
            
            base.Effect(caster, target);
        }
    }

    public class Disarm : Card
    {
        public Disarm()
        {
            id = 22;
            name = "무장 헤제";
            cost = 1;
            power = 2;
            desc = $"적의 힘을 {power} 감소시킵니다.";
            type = CardType.One;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].Strength -= 2;
            
            base.Effect(caster, target);
        }
    }

    public class Intimidate : Card
    {
        public Intimidate()
        {
            id = 23;
            name = "위압";
            cost = 0;
            power = 1;
            desc = $"적 전체에게 약화를 {power} 부여합니다.";
            type = CardType.All;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            foreach (var monster in target)
            {
                monster.Weakness += 1;
            }
            
            base.Effect(caster, target);
        }
    }

    public class LimitBreak : Card
    {
        public LimitBreak()
        {
            id = 24;
            name = "한계돌파";
            cost = 2;
            power = 2;
            desc = $"힘이 {power}배로 증가합니다.";
            type = CardType.None;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            caster.Strength *= power;
            
            base.Effect(caster, target);
        }
    }

    public class Offering : Card
    {
        private int _loseHealth = 6;
        
        public Offering()
        {
            id = 25;
            name = "제물";
            cost = 1;
            power = 2;
            desc = $"체력을 {_loseHealth} 잃습니다. 에너지를 {power} 얻습니다. 카드를 2장 뽑습니다.";
            type = CardType.None;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            caster.curHp -= _loseHealth;
            caster.CurMp += power;
            
            CardManager.Instance.DrawCard();
            CardManager.Instance.DrawCard();
            
            base.Effect(caster, target);
        }
    }

    public class Inflame : Card
    {
        public Inflame()
        {
            id = 26;
            name = "발화";
            cost = 2;
            power = 2;
            desc = $"힘을 {power} 얻습니다.";
            type = CardType.None;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            caster.Strength += power;
            
            base.Effect(caster, target);
        }
    }

    public class BurningPact : Card
    {
        public BurningPact()
        {
            id = 27;
            name = "불타는 조약";
            cost = 1;
            power = 2;
            desc = $"카드를 1장 소멸시킵니다. 카드를 {power} 뽑습니다.";
            type = CardType.None;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            CardManager.Instance.DestroyCard(CardManager.Instance.cards[Random.Range(0, CardManager.Instance.cards.Count)]);
            
            CardManager.Instance.DrawCard();
            CardManager.Instance.DrawCard();
            
            base.Effect(caster, target);
        }
    }

    public class BandageUp : Card
    {
        public BandageUp()
        {
            id = 28;
            name = "붕대 감기";
            cost = 0;
            power = 3;
            desc = $"체력을 {power} 회복합니다.";
            type = CardType.None;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            caster.curHp += power;
            
            base.Effect(caster, target);
        }
    }
    
    public class Blind : Card
    {
        public Blind()
        {
            id = 29;
            name = "실명";
            cost = 0;
            power = 2;
            desc = $"약화를 {power} 부여합니다.";
            type = CardType.None;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].Vulnerable += power;
            
            base.Effect(caster, target);
        }
    }
    
    public class Trip : Card
    {
        public Trip()
        {
            id = 30;
            name = "헛디딤";
            cost = 0;
            power = 2;
            desc = $"취약을 {power} 부여합니다.";
            type = CardType.None;
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].Weakness += power;
            
            base.Effect(caster, target);
        }
    }
}


