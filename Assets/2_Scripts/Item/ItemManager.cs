using System;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ItemManager : SingletonMono<ItemManager>
{
    public List<Item> items = new List<Item>();

    // private void Update()
    // {
    //     if (SceneManager.GetActiveScene().name.Contains("Stage") || SceneManager.GetActiveScene().name == "Ingame")
    //     {
    //         UseEffect();
    //     }
    // }

    private void Start()
    {
        items.Add(new MonsterBook());
        items.Add(new Knuckle());
    }

    public void UseEffect()
    {
        foreach (var item in items)
        {
            item.Effect();
        }
    }
}

[Serializable]
public class Item
{
    public int id;
    public string name;
    public int cost;
    public string desc;
    public Sprite sprite;

    public virtual void Effect()
    {
        
    }
}

namespace Items
{
    public class BloodPack : Item
    {
        public BloodPack()
        {
            id = 1;
            name = "BloodPack";
            cost = 100;
            desc = "전투 종료 시 체력을 6 회복합니다";
        }
        
        public override void Effect()
        {
            if (GameManager.Instance.GameStates == GameManager.GameState.GameEnd)
            {
                GameManager.Instance.player.CurHp += 6;
            }
            
            base.Effect();
        }
    }
    
    public class MonsterBook : Item
    {
        public MonsterBook()
        {
            id = 2;
            name = "MonsterBook";
            cost = 100;
            desc = "모든 적들이 25% 피해를 입고 전투를 시작합니다";
        }
        
        public override void Effect()
        {
            if (GameManager.Instance.GameStates == GameManager.GameState.GameStart)
            {
                Debug.Log("지랄");
                foreach (var monster in GameManager.Instance.monsters)
                {
                    monster.curHp -= (int) ((float) monster.maxHp * 0.25);
                    monster.maxHp -= (int) ((float) monster.maxHp * 0.25);
                }
            }
            
            base.Effect();
        }
    }
    
    public class Knuckle : Item
    {
        public Knuckle()
        {
            id = 3;
            name = "Knuckle";
            cost = 100;
            desc = "힘을 1 얻은 채로 전투를 시작합니다.";
        }
        
        public override void Effect()
        {
            if (GameManager.Instance.GameStates == GameManager.GameState.GameStart)
            {
                GameManager.Instance.player.Strength += 1;
            }
            
            base.Effect();
        }
    }

    public class SmoothStone : Item
    {
        public SmoothStone()
        {
            id = 4;
            name = "SmoothStone";
            cost = 100;
            desc = "민첩을 1 얻은 채로 전투를 시작합니다.";
        }
        
        public override void Effect()
        {
            if (GameManager.Instance.GameStates == GameManager.GameState.GameStart)
            {
                GameManager.Instance.player.Agility += 1;
            }
            
            base.Effect();
        }
    }
    
    public class Sail : Item
    {
        public Sail()
        {
            id = 5;
            name = "Sail";
            cost = 100;
            desc = "전투 시작시 방어도를 10 얻습니다.";
        }
        
        public override void Effect()
        {
            if (GameManager.Instance.GameStates == GameManager.GameState.GameStart)
            {
                GameManager.Instance.player.armor += 10;
            }
            
            base.Effect();
        }
    }

    public class MoneyBag : Item
    {
        public MoneyBag()
        {
            id = 6;
            name = "MoneyBag";
            cost = 100;
            desc = "전투가 끝날때 얻는 금액이 50% 증가합니다.";
        }
        
        public override void Effect()
        {
            if (GameManager.Instance.GameStates == GameManager.GameState.GameEnd)
            {
                // GameManager.Instance.;
            }
            
            base.Effect();
        }
    }
}