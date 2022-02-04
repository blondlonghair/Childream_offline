using System;
using System.Collections.Generic;
using MonsterSkill;
using UnityEngine;
using Random = UnityEngine.Random;

public class Monster : Unit
{
    [SerializeField] protected List<Skill> _skillBuffer = new List<Skill>();
    [SerializeField] protected List<Skill> _useSkills = new List<Skill>();
    
    public FootPos attackPos = FootPos.None;

    protected virtual void Start()
    {
        GameManager.Instance.monsters.Add(this);
        SetupCard();
    }

    public virtual void Destroy()
    {
        GameManager.Instance.monsters.Remove(this);
        Destroy(gameObject);
    }

    public virtual void Attack()
    {
        if (_skillBuffer.Count <= 0)
        {
            SetupCard();
        }
        
        _skillBuffer[0].Effect(this, GameManager.Instance.player);
        _skillBuffer.RemoveAt(0);
    }
    
    protected virtual void SetupCard()
    {
        for (int i = 0; i < _useSkills.Count; i++)
        {
            Skill card = _useSkills[i];
            _skillBuffer.Add(card);
        }

        for (int i = 0; i < _skillBuffer.Count - 1; i++)
        {
            int rand = Random.Range(i, _skillBuffer.Count);
            (_skillBuffer[i], _skillBuffer[rand]) = (_skillBuffer[rand], _skillBuffer[i]);
        }
    }

    public virtual void ShowAttackPos()
    {
        int nextAttackPos = _skillBuffer[0].id;
        // 다음 공격할곳 보여주기
    }
}