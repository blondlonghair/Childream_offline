using UnityEngine;

namespace MonsterSkill
{
    [CreateAssetMenu(fileName = "MonsterSkill", menuName = "MonsterSkill", order = 0)]
    public class MonsterSkill : ScriptableObject
    {
        public int id;
        public string name;
        public int power;
        public string desc;
        public FootPos attackPos; 
    }
}