using System;
using System.Collections.Generic;
using MonsterSkill;
using UnityEngine;

public class EffectManager : SingletonMono<EffectManager>
{
    [SerializeField] private GameObject[] hitEffects;
    [SerializeField] private GameObject[] gridEffects;
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

    public void InitGridEffect(FootPos footPos)
    {
        switch (footPos)
        {
            case FootPos.Left:
                Instantiate(gridEffects[0], InGameManager.Instance.leftGrid.transform.position, Quaternion.identity);
                break;
            case FootPos.Middle:
                Instantiate(gridEffects[1], InGameManager.Instance.middleGrid.transform.position, Quaternion.identity);
                break;
            case FootPos.Right:
                Instantiate(gridEffects[2], InGameManager.Instance.rightGrid.transform.position, Quaternion.identity);
                break;
        }
    }
}