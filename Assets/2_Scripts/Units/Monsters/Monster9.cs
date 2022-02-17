﻿using System.Collections;
using MonsterSkill;
using UnityEngine;

public class Monster9 : Monster
{
    private Animator _animator;
    
    protected override void Start()
    {
        TryGetComponent(out _animator);

        useSkills.Add(new Strike(6, FootPos.Middle));
        
        base.Start();
    }

    protected override IEnumerator Co_OnDeath()
    {
        _animator.SetTrigger("isDie");
        yield return YieldCache.WaitForSeconds(2f);
        
        yield return base.Co_OnDeath();
    }
}