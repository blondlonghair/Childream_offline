using System;
using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class Item
{
    public int id;
    public string name;
    public int cost;
    public string desc;
    public Sprite shopUnSelectSprite;
    public Sprite shopSelectSprite;
    public Sprite inGameIconSprite;
    
    public virtual void Effect(GameManager.GameState gameState)
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
            name = "수혈팩";
            cost = 100;
            desc = "전투 종료 시 체력을 6 회복합니다";
            shopUnSelectSprite = ItemManager.Instance.sprites[id * 3 - 3];
            shopSelectSprite = ItemManager.Instance.sprites[id * 3 - 2];
            inGameIconSprite = ItemManager.Instance.sprites[id * 3 - 1];
        }

        public override void Effect(GameManager.GameState gameState)
        {
            if (GameManager.Instance.GameStates == GameManager.GameState.GameEnd)
            {
                GameManager.Instance.player.CurHp += 6;
            }
            
            base.Effect(gameState);
        }
    }
    
    public class MonsterBook : Item
    {
        public MonsterBook()
        {
            id = 2;
            name = "몬스터 도감";
            cost = 100;
            desc = "모든 적들이 25% 피해를 입고 전투를 시작합니다";
            shopUnSelectSprite = ItemManager.Instance.sprites[id * 3 - 3];
            shopSelectSprite = ItemManager.Instance.sprites[id * 3 - 2];
            inGameIconSprite = ItemManager.Instance.sprites[id * 3 - 1];
        }
        
        public override void Effect(GameManager.GameState gameState)
        {
            if (GameManager.Instance.GameStates == GameManager.GameState.GameStart)
            {
                foreach (var monster in GameManager.Instance.monsters)
                {
                    monster.CurHp -= (int) ((float) monster.MaxHp * 0.25);
                    monster.MaxHp -= (int) ((float) monster.MaxHp * 0.25);
                }
            }
            
            base.Effect(gameState);
        }
    }
    
    public class Knuckle : Item
    {
        public Knuckle()
        {
            id = 3;
            name = "너클";
            cost = 100;
            desc = "힘을 1 얻은 채로 전투를 시작합니다.";
            shopUnSelectSprite = ItemManager.Instance.sprites[id * 3 - 3];
            shopSelectSprite = ItemManager.Instance.sprites[id * 3 - 2];
            inGameIconSprite = ItemManager.Instance.sprites[id * 3 - 1];
        }
        
        public override void Effect(GameManager.GameState gameState)
        {
            if (GameManager.Instance.GameStates == GameManager.GameState.GameStart)
            {
                GameManager.Instance.player.Strength += 1;
            }
            
            base.Effect(gameState);
        }
    }

    public class SmoothStone : Item
    {
        public SmoothStone()
        {
            id = 4;
            name = "매끄러운 돌";
            cost = 100;
            desc = "민첩을 1 얻은 채로 전투를 시작합니다.";
            shopUnSelectSprite = ItemManager.Instance.sprites[id * 3 - 3];
            shopSelectSprite = ItemManager.Instance.sprites[id * 3 - 2];
            inGameIconSprite = ItemManager.Instance.sprites[id * 3 - 1];
        }
        
        public override void Effect(GameManager.GameState gameState)
        {
            if (GameManager.Instance.GameStates == GameManager.GameState.GameStart)
            {
                GameManager.Instance.player.Agility += 1;
            }
            
            base.Effect(gameState);
        }
    }
    
    public class PlateArmor : Item
    {
        public PlateArmor()
        {
            id = 5;
            name = "갑옷";
            cost = 100;
            desc = "전투 시작시 방어도를 10 얻습니다.";
            shopUnSelectSprite = ItemManager.Instance.sprites[id * 3 - 3];
            shopSelectSprite = ItemManager.Instance.sprites[id * 3 - 2];
            inGameIconSprite = ItemManager.Instance.sprites[id * 3 - 1];
        }
        
        public override void Effect(GameManager.GameState gameState)
        {
            if (GameManager.Instance.GameStates == GameManager.GameState.GameStart)
            {
                GameManager.Instance.player.Armor += 10;
            }
            
            base.Effect(gameState);
        }
    }

    public class MoneyBag : Item
    {
        public MoneyBag()
        {
            id = 6;
            name = "돈봉투";
            cost = 100;
            desc = "전투가 끝날때 얻는 금액이 50% 증가합니다.";
            shopUnSelectSprite = ItemManager.Instance.sprites[id * 3 - 3];
            shopSelectSprite = ItemManager.Instance.sprites[id * 3 - 2];
            inGameIconSprite = ItemManager.Instance.sprites[id * 3 - 1];
        }
        
        public override void Effect(GameManager.GameState gameState)
        {
            if (GameManager.Instance.GameStates == GameManager.GameState.GameEnd)
            {
                // GameManager.Instance.;
            }
            
            base.Effect(gameState);
        }
    }
}