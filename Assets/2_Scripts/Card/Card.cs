using System;
using System.Collections.Generic;
using System.Linq;
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
    public Sprite sprite;
    public Sprite backGround;

    public bool isUpgrade;
    public CardType type;

    public virtual void Effect(Player caster, params Monster[] target)
    {
        if (caster.CurMp < cost)
        {
            return;
        }

        caster.CurHp = caster.CurHp;
        caster.CurMp -= cost;
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
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].GetDamage((int)((float)(power + caster.Strength) * (caster.Weakness > 0 ? 0.75f : 1f)));
            EffectManager.Instance.InitEffect("Strike", target[0]?.transform);

            base.Effect(caster, target);
        }

        public override void Upgrade()
        {
            power = 8;
        
            base.Upgrade();
        }
    }

    public class RepeatedHit : Card
    {
        private int _vulnerable = 2;

        public RepeatedHit()
        {
            id = 2;
            name = "연타";
            cost = 2;
            power = 8;
            desc = $"피해를 {power} 줍니다.\n취약을 {_vulnerable} 부여합니다.";
            type = CardType.One;
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].GetDamage((power + caster.Strength) * (caster.Weakness > 0 ? 3/4 : 1));
            target[0].Vulnerable += _vulnerable;
            EffectManager.Instance.InitEffect("Strike", target[0]?.transform);

            base.Effect(caster, target);
        }

        public override void Upgrade()
        {
            power = 10;
            _vulnerable = 3;
            
            base.Upgrade();
        }
    }

    public class Squash : Card
    {
        public Squash()
        {
            id = 3;
            name = "짓누르기";
            cost = 1;
            desc = $"현재 방어도만큼 피해를 줍니다.";
            type = CardType.One;
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].GetDamage((power + caster.Strength) * (caster.Weakness > 0 ? 3/4 : 1));
            EffectManager.Instance.InitEffect("Squash", target[0]?.transform);

            base.Effect(caster, target);
        }
    }

    public class DeathFault : Card
    {
        public DeathFault()
        {
            id = 4;
            name = "데스폴트";
            cost = 1;
            power = 8;
            desc = $"적 전체에게 피해를 {power} 줍니다.";
            type = CardType.All;
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            foreach (var monster in target)
            {
                monster.GetDamage((power + caster.Strength) * (caster.Weakness > 0 ? 3/4 : 1));
                EffectManager.Instance.InitEffect("DeathFault", monster.transform);
            }

            base.Effect(caster, target);
        }
    }

    public class Brimstone : Card
    {
        private int _loseHealth = 2;
        
        public Brimstone()
        {
            id = 5;
            name = "혈사포";
            cost = 1;
            power = 15;
            desc = $"체력을 {_loseHealth} 잃습니다. 피해를 {power} 줍니다.";
            type = CardType.One;
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].GetDamage((power + caster.Strength) * (caster.Weakness > 0 ? 3/4 : 1));
            caster.GetDamage(_loseHealth);
            EffectManager.Instance.InitEffect("Strike", target[0]?.transform);

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
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].GetDamage((power + caster.Strength) * (caster.Weakness > 0 ? 3/4 : 1));
            EffectManager.Instance.InitEffect("Bash", target[0]?.transform);

            base.Effect(caster, target);
        }
    }

    public class Wave : Card
    {
        private int _armor;

        public Wave()
        {
            id = 7;
            name = "파동";
            cost = 2;
            power = 5;
            desc = $"방어도를 {power} 얻습니다. 피해를 {_armor} 줍니다.";
            type = CardType.One;
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].GetDamage((power + caster.Strength) * (caster.Weakness > 0 ? 3/4 : 1));

            caster.Armor += _armor;
            EffectManager.Instance.InitEffect("Strike", target[0]?.transform);

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
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].GetDamage((power + caster.Strength) * (caster.Weakness > 0 ? 3/4 : 1));

            CardManager.Instance.DrawCard();
            EffectManager.Instance.InitEffect("Strike", target[0]?.transform);

            base.Effect(caster, target);
        }

        public override void Upgrade()
        {
            power = 10;
            _drawCard = 2;
        
            base.Upgrade();
        }
    }

    public class Voltage : Card
    {
        private int _vulnerable = 1;

        public Voltage()
        {
            id = 9;
            name = "전류";
            cost = 1;
            power = 4;
            desc = $"적 전체에게 피해를 {power} 주고 취약을 {_vulnerable} 부여합니다.";
            type = CardType.All;
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            foreach (var monster in target)
            {
                monster.GetDamage((power + caster.Strength) * (caster.Weakness > 0 ? 3/4 : 1));
                EffectManager.Instance.InitEffect("Voltage", monster.transform);

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
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].GetDamage((power + caster.Strength) * (caster.Weakness > 0 ? 3/4 : 1));
            EffectManager.Instance.InitEffect("Strike", target[0].transform);

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
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            if (target[0].Vulnerable > 0)
            {
                caster.CurMp += _energy;
                CardManager.Instance.DrawCard();
            }

            target[0].GetDamage((power + caster.Strength) * (caster.Weakness > 0 ? 3/4 : 1));
            EffectManager.Instance.InitEffect("Strike", target[0].transform);

            base.Effect(caster, target);
        }
    }

    public class SoulCutter : Card
    {
        public SoulCutter()
        {
            id = 12;
            name = "소울커터";
            cost = 2;
            power = 16;
            desc = $"손에 있는 모든 카드를 소멸시킵니다. 피해를 {power} 줍니다.";
            type = CardType.One;
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            int cardCount = CardManager.Instance.cards.Count;
            
            for (int i = cardCount - 1; i >= 0; i--)
            {
                CardManager.Instance.DestroyCard(CardManager.Instance.cards[i]);
            }

            target[0].GetDamage((power + caster.Strength) * (caster.Weakness > 0 ? 3/4 : 1));
            EffectManager.Instance.InitEffect("Strike", target[0].transform);

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
            desc = $"피해를 {power} 줍니다. 약화를 {_weakness} 부여합니다. 취약을 {_vulnerable} 부여합니다.";
            type = CardType.One;
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].GetDamage((power + caster.Strength) * (caster.Weakness > 0 ? 3/4 : 1));

            target[0].Vulnerable += _vulnerable;
            target[0].Weakness += _weakness;
            EffectManager.Instance.InitEffect("Strike", target[0].transform);
         
            base.Effect(caster, target);
        }
    }

    public class Bash : Card
    {
        public Bash()
        {
            id = 14;
            name = "강타";
            cost = 3;
            power = 32;
            desc = $"피해를 {power} 줍니다.";
            type = CardType.One;
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].GetDamage((power + caster.Strength) * (caster.Weakness > 0 ? 3/4 : 1));
            EffectManager.Instance.InitEffect("Squash", target[0].transform);

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
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            caster.Armor += power;
            EffectManager.Instance.InitEffect("Defence", caster.transform);

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
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            caster.Armor += power;
            CardManager.Instance.DrawCard();
            EffectManager.Instance.InitEffect("Defence", caster.transform);
      
            base.Effect(caster, target);
        }

        public override void Upgrade()
        {
            power = 11;
            
            base.Upgrade();
        }
    }

    public class Grit : Card
    {
        public Grit()
        {
            id = 17;
            name = "끈기";
            cost = 1;
            power = 7;
            desc = $"방어도를 {power} 얻습니다. 무작위 카드를 1장 소멸시킵니다.";
            type = CardType.None;
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            caster.Armor += power;
            CardManager.Instance.DestroyCard(CardManager.Instance.cards[Random.Range(0, CardManager.Instance.cards.Count)]);
            EffectManager.Instance.InitEffect("Defence", caster.transform);

            base.Effect(caster, target);
        }
    }

    public class Duplication : Card
    {
        public Duplication()
        {
            id = 18;
            name = "분신술";
            cost = 2;
            power = 2;
            desc = $"방어도가 {power}배로 증가합니다.";
            type = CardType.None;
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            caster.Armor *= power;
            EffectManager.Instance.InitEffect("Defence", caster.transform);

            base.Effect(caster, target);
        }

        public override void Upgrade()
        {
            cost = 1;
            
            base.Upgrade();
        }
    }

    public class Sacrifice : Card
    {
        public Sacrifice()
        {
            id = 19;
            name = "희생";
            cost = 1;
            power = 5;
            desc = $"모든 카드를 소멸시키고 그 수만큼 방어도를 {power} 얻습니다.";
            type = CardType.None;
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            caster.Armor += power * CardManager.Instance.cards.Count;
            EffectManager.Instance.InitEffect("Defence", caster.transform);

            int cardCount = CardManager.Instance.cards.Count;
            
            for (int i = cardCount - 1; i >= 0; i--)
            {
                CardManager.Instance.DestroyCard(CardManager.Instance.cards[i]);
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
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
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
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            caster.CurHp -= _loseHealth;
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
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].Strength -= 2;
            EffectManager.Instance.InitEffect("StrengthDown", target[0].transform);

            base.Effect(caster, target);
        }
    }

    public class Fear : Card
    {
        public Fear()
        {
            id = 23;
            name = "공포";
            cost = 0;
            power = 1;
            desc = $"적 전체에게 약화를 {power} 부여합니다.";
            type = CardType.All;
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            foreach (var monster in target)
            {
                monster.Weakness += 1;
                EffectManager.Instance.InitEffect("Weakness", monster.transform);
            }

            base.Effect(caster, target);
        }
    }

    public class Adrenalin : Card
    {
        public Adrenalin()
        {
            id = 24;
            name = "아드레날린";
            cost = 2;
            power = 2;
            desc = $"힘이 {power}배로 증가합니다.";
            type = CardType.None;
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            caster.Strength *= power;
            EffectManager.Instance.InitEffect("Strength", caster.transform);

            base.Effect(caster, target);
        }
    }

    public class Adjustment : Card
    {
        private int _loseHealth = 6;
        
        public Adjustment()
        {
            id = 25;
            name = "조정";
            cost = 1;
            power = 2;
            desc = $"체력을 {_loseHealth} 잃습니다. 에너지를 {power} 얻습니다. 카드를 2장 뽑습니다.";
            type = CardType.None;
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            caster.CurHp -= _loseHealth;
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
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            caster.Strength += power;
            EffectManager.Instance.InitEffect("Strength", caster.transform);

            base.Effect(caster, target);
        }
    }

    public class Contract : Card
    {
        public Contract()
        {
            id = 27;
            name = "계약체결";
            cost = 1;
            power = 2;
            desc = $"무작위 카드를 1장 소멸시킵니다. 카드를 {power} 뽑습니다.";
            type = CardType.None;
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            CardManager.Instance.DestroyCard(CardManager.Instance.cards[Random.Range(0, CardManager.Instance.cards.Count)]);
            
            CardManager.Instance.DrawCard();
            CardManager.Instance.DrawCard();
            
            base.Effect(caster, target);
        }
    }

    public class Hemostasis : Card
    {
        public Hemostasis()
        {
            id = 28;
            name = "지혈";
            cost = 0;
            power = 3;
            desc = $"체력을 {power} 회복합니다.";
            type = CardType.None;
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            caster.CurHp += power;
            EffectManager.Instance.InitEffect("Heal", caster.transform);

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
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].Weakness += power;
            EffectManager.Instance.InitEffect("Weakness", target[0].transform);

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
            CardManager.Instance.GetSprite(id, out sprite, out backGround);
        }

        public override void Effect(Player caster, params Monster[] target)
        {
            target[0].Vulnerable += power;
            EffectManager.Instance.InitEffect("Vulnerable", target[0].transform);

            base.Effect(caster, target);
        }
    }
}


