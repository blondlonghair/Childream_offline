using System;
using System.Collections.Generic;
using MonsterSkill;
using UnityEngine;

public class EffectManager : SingletonMono<EffectManager>
{
    [SerializeField] private GameObject[] hitEffects;
    [SerializeField] private GameObject[] attackEffects;
    private Dictionary<string, GameObject> _hitEffect = new Dictionary<string, GameObject>();
    private Dictionary<string, GameObject> _attackEffect = new Dictionary<string, GameObject>();
    [SerializeField] private AtkEffect atkEffect;

    private void Start()
    {
        foreach (var hiteffect in hitEffects)
        {
            _hitEffect.Add(hiteffect.name, hiteffect);
        }

        foreach (var attackEffect in attackEffects)
        {
            _attackEffect.Add(attackEffect.name, attackEffect);
        }
    }

    public void InitEffect(string effect, Transform target)
    {
        Instantiate(_hitEffect[effect], target.position, Quaternion.identity);
    }

    public void MonsterEffect(Skill skill, Transform target)
    {
        // if (TryGetComponent(out SpriteRenderer))
        // target.transform.position + Vector3.up * (target.gameObject.spriteRenderer.sprite.rect.y / 2);
        Instantiate(_attackEffect[skill.name], target);
    }
}