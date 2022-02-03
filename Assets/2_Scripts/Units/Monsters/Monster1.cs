using System;
using MonsterSkill;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class Monster1 : Monster
{
    protected override void Start()
    {
        // var values = Enum.GetValues(typeof(FootPos));
        // FootPos randomValue = (FootPos) values[Random.Range(0, values.Length)];
        
        _useSkills.Add(new Strike(5, FootPos.Middle));
        _useSkills.Add(new Strike(6, FootPos.Middle));
        
        base.Start();
    }
}