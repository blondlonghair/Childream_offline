using UnityEngine;

namespace OffLine
{
    public class OffCardData : MonoBehaviour
    {
        public int id;
        public string name;
        public int power;

        public virtual void Effect(Player caster, Monster target)
        {
            // caster.
        }
    }

    public class Card_1 : OffCardData
    {
        Card_1()
        {
            id = 1;
            name = "타격";
            power = 6;
        }
        
        public override void Effect(Player caster, Monster target)
        {
            target.curHp -= power;
        }
    }
}