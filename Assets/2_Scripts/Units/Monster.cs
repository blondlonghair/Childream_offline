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
    
    public FootPos attackPos = FootPos.None;

    public MonsterHpBar hpBar;

    protected virtual void Start()
    {
        GameManager.Instance.monsters.Add(this);
        
        SetupSkill();
    }

    public virtual void Destroy()
    {
        GameManager.Instance.monsters.Remove(this);
        Destroy(gameObject);
    }

    public override void Attack()
    {
        if (skillBuffer.Count <= 0)
        {
            SetupSkill();
        }
        
        skillBuffer[0].Effect(this, GameManager.Instance.player);
        skillBuffer.RemoveAt(0);
        
        base.Attack();
    }

    protected override IEnumerator Co_Attack()
    {
        print("Attack");
        Vector3 originPos = transform.position;
        
        while (!Mathf.Approximately(transform.position.y, originPos.y - 2))
        {
            transform.position = Vector3.Lerp(transform.position, originPos + Vector3.down * 2, 0.5f);
            yield return YieldCache.WaitForSeconds(0.01f);
        }

        while (!Mathf.Approximately(transform.position.y, originPos.y))
        {
            transform.position = Vector3.Lerp(transform.position, originPos, 0.5f);
            yield return YieldCache.WaitForSeconds(0.01f);
        }
    }

    protected override IEnumerator Co_GetHit()
    {
        print("GetHit");
        Vector3 originPos = transform.position;

        for (int i = 0; i < 2; i++)
        {
            while (!Mathf.Approximately(transform.position.x, originPos.x - 0.3f))
            {
                transform.position = Vector3.Lerp(transform.position, originPos - Vector3.left * 0.5f, 0.5f);
                yield return YieldCache.WaitForSeconds(0.01f);
            }

            while (!Mathf.Approximately(transform.position.x, originPos.x + 0.3f))
            {
                transform.position = Vector3.Lerp(transform.position, originPos - Vector3.right * 0.5f, 0.5f);
                yield return YieldCache.WaitForSeconds(0.01f);
            }
        }

        while (!Mathf.Approximately(transform.position.x, originPos.x))
        {
            transform.position = Vector3.Lerp(transform.position, originPos, 0.5f);
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
        int nextAttackPos = skillBuffer[0].id;
        // 다음 공격할곳 보여주기
    }
}