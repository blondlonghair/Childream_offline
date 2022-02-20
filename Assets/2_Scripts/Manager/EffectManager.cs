using System;
using System.Collections.Generic;
using MonsterSkill;
using UnityEngine;

public class EffectManager : SingletonMono<EffectManager>
{
    [SerializeField] private GameObject[] hitEffects;
    private Dictionary<string, GameObject> _hitEffect = new Dictionary<string, GameObject>();

    private void Start()
    {
        foreach (var hiteffect in hitEffects)
        {
            _hitEffect.Add(hiteffect.name, hiteffect);
        }
    }

    public void InitEffect(string effect, Transform target)
    {
        Instantiate(_hitEffect[effect], target.position, Quaternion.identity);
    }
}