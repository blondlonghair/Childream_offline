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

    public int CurHp
    {
        get => curHp;
        set
        {
            curHp = value;
            GetHit();
        }
    }

    protected virtual void Start()
    {
        GameManager.Instance.monsters.Add(this);
        
        SetupSkill();
    }

    public override void Attack()
    {
        skillBuffer[0].Effect(this, GameManager.Instance.player);
        skillBuffer.RemoveAt(0);
        
        base.Attack();
    }

    public override void GetHit()
    {
        hpBar.Value = (float)curHp / (float)maxHp;
        
        base.GetHit();
    }

    public override void OnDeath()
    {
        GameManager.Instance.monsters.Remove(this);
        
        Destroy(hpBar.gameObject);
        Destroy(atkEffect.gameObject);
        Destroy(gameObject);
        
        base.OnDeath();
    }

    protected override IEnumerator Co_Attack()
    {
        print("Attack");
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
        
        // if (TryGetComponent(out SpriteRenderer spriteRenderer))
        // {
            // EffectManager.Instance.MonsterEffect(skillBuffer[0], transform);
        // }
        
        // 다음 공격할곳 보여주기
    }
}