using UnityEngine;

namespace MonsterSkill
{
    public class Skill : MonoBehaviour
    {
        public int id;
        public string name;
        public int power;
        public string desc;

        public virtual void Effect(Monster caster,  Player target) { }
    }

    public class MonsterSkill1 : Skill
    {
        MonsterSkill1()
        {
            id = 1;
            name = "타격";
            power = 6;
            desc = $"피해를 {power} 줍니다.";
        }

        public override void Effect(Monster caster,  Player target)
        {
            target.curHp -= power;
        }
    }

    public class MonsterSkill2 : Skill
    {
        private int weakness = 2;
        
        MonsterSkill2()
        {
            id = 2;
            name = "강타";
            power = 8;
        }

        public override void Effect(Monster caster, Player target)
        {
            target.curHp -= power;
            target.weakness += this.weakness;
        }
    }
}