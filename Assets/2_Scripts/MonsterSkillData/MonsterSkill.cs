using UnityEngine;

namespace OffLine
{
    public class MonsterSkill : MonoBehaviour
    {
        public int id;
        public string name;
        public int power;

        public virtual void SkillEffect(Monster caster,  Player target)
        {
            
        }
    }

    public class MonsterSkill1 : MonsterSkill
    {
        MonsterSkill1()
        {
            id = 1;
            name = "공격";
            power = 6;
        }

        public override void SkillEffect(Monster caster,  Player target)
        {
            target.curHp -= power;
        }
    }
}