using System;
using System.Collections.Generic;
using MonsterSkill;
using Random = UnityEngine.Random;

public enum NextAttackPos
{
    None = 0,
    Left = 1,
    Middle = 2,
    Right = 4
}

public class Monster : Unit
{
    protected List<Skill> _skillBuffer = new List<Skill>();
    protected List<Skill> _useSkills = new List<Skill>();
    
    public NextAttackPos nextAttackPos = NextAttackPos.None;

    protected void Start()
    {
        GameManager.Instance.monsters.Add(this);
    }

    protected void OnDestroy()
    {
        GameManager.Instance.monsters.Remove(this);
    }

    public void Attack()
    {
        if (_skillBuffer.Count <= 0)
        {
            SetupCard();
        }
        
        _skillBuffer[0].Effect(this, GameManager.Instance.player);
        _skillBuffer.RemoveAt(0);
    }
    
    protected void SetupCard()
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
}