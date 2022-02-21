using System.Collections;
using MonsterSkill;
using UnityEngine;

public class Boss1 : Monster
{
    protected override void Start()
    {
        useSkills.Add(new Strike(6, FootPos.Middle));

        base.Start();
    }

    protected override IEnumerator Co_Attack()
    {
        _animator.SetTrigger("isAttack");

        yield return null;
    }

    public override void OnDeath()
    {
        
        
        base.OnDeath();
    }
}