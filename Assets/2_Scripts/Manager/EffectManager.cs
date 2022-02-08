using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : SingletonMono<EffectManager>
{
    [SerializeField] private GameObject[] _effects;
    private Dictionary<string, GameObject> _effect = new Dictionary<string, GameObject>();

    private void Start()
    {
        foreach (var o in _effects)
        {
            _effect.Add(o.name, o);
        }
    }

    public void InitEffect(string effect, Transform target)
    {
        Instantiate(_effect[effect], target.position, Quaternion.identity);
    }
}