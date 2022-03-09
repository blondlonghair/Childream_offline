using System;
using System.Collections;
using System.Collections.Generic;
using MonsterSkill;
using UnityEngine;
using Random = UnityEngine.Random;

public class Monster : Unit
{
    [SerializeField] protected List<Skill> skillBuffer = new List<Skill>();
    [SerializeField] protected List<Skill> useSkills = new List<Skill>();
    
    [HideInInspector] public MonsterHpBar hpBar;
    [HideInInspector] public AtkEffect atkEffect;
    
    protected Animator _animator;

    public int CurHp
    {
        get => curHp;
        set
        { 
            curHp = value;
            hpBar.SetValue(armor, curHp, maxHp);
        }
    }

    public int MaxHp
    {
        get => maxHp;
        set
        {
            maxHp = value; 
            hpBar.SetValue(armor, curHp, maxHp);
        }
    }

    public int Armor
    {
        get => armor;
        set
        {
            armor = value;
            hpBar.SetValue(armor, curHp, maxHp);
            if (value <= 0)
                EffectManager.Instance.InitEffect("Defence_Broke", transform);
        }
    }
    
    protected virtual void Start()
    {
        TryGetComponent(out _animator);

        hpBar.SetValue(armor, curHp, maxHp);

        SetupSkill();
    }

    public override void Attack()
    {
        skillBuffer[0].Effect(this, InGameManager.Instance.player);
        skillBuffer.RemoveAt(0);
        
        base.Attack();
    }

    public override void GetHit()
    {
        hpBar.SetValue(armor, curHp, maxHp);
        
        base.GetHit();
    }
    
    public void GetDamage(int damage)
    {
        int getDamage = (int)((float)damage * (Vulnerable > 0 ? 1.5f : 1));
        
        for (int i = 0; i < getDamage; i++)
        {
            if (armor > 0)
            {
                armor--;
            }
            else
            {
                curHp--;
            }
        }
        
        GetHit();
    }

    public override void OnDeath()
    {
        StartCoroutine(Co_OnDeath());
        
        base.OnDeath();
    }

    protected override IEnumerator Co_Attack()
    {
        Vector3 originPos = transform.position;
        
        while (transform.position.y > originPos.y - 1.5)
        {
            transform.position = Vector3.Lerp(transform.position, originPos + Vector3.down * 2, 0.1f);
            yield return YieldCache.WaitForSeconds(0.01f);
        }

        while (!Mathf.Approximately(transform.position.y, originPos.y))
        {
            transform.position = Vector3.Lerp(transform.position, originPos, 0.1f);
            yield return YieldCache.WaitForSeconds(0.01f);
        }
    }

    protected override IEnumerator Co_GetHit()
    {
        Vector3 originPos = transform.position;

        for (int i = 0; i < 2; i++)
        {
            while (!Mathf.Approximately(transform.position.x, originPos.x - 0.5f))
            {
                transform.position = Vector3.Lerp(transform.position, originPos + Vector3.left * 0.5f, 0.9f);
                yield return YieldCache.WaitForSeconds(0.01f);
            }

            while (!Mathf.Approximately(transform.position.x, originPos.x + 0.5f))
            {
                transform.position = Vector3.Lerp(transform.position, originPos + Vector3.right * 0.5f, 0.9f);
                yield return YieldCache.WaitForSeconds(0.01f);
            }
        }

        while (!Mathf.Approximately(transform.position.x, originPos.x))
        {
            transform.position = Vector3.Lerp(transform.position, originPos, 0.9f);
            yield return YieldCache.WaitForSeconds(0.01f);
        }
    }

    protected virtual IEnumerator Co_OnDeath()
    {
        InGameManager.Instance.monsters.Remove(this);
        
        Destroy(hpBar.gameObject);
        Destroy(atkEffect.gameObject);
        
        _animator.SetTrigger("isDie");
        yield return YieldCache.WaitForSeconds(2f);

        Destroy(gameObject);
        
        yield return null;
    }

    protected virtual void SetupSkill()
    {
        for (int i = 0; i < useSkills.Count; i++)
        {
            Skill card = useSkills[i];
            skillBuffer.Add(card);
        }

        for (int i = 0; i < skillBuffer.Count - 1; i++)
        {
            int rand = Random.Range(i, skillBuffer.Count);
            (skillBuffer[i], skillBuffer[rand]) = (skillBuffer[rand], skillBuffer[i]);
        }
    }
    
    public virtual void ShowAttackPos()
    {
        if (skillBuffer.Count <= 0)
        {
            SetupSkill();
        }
        
        atkEffect.Effect(skillBuffer[0]);
    }
}