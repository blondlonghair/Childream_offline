using System;
using System.Collections.Generic;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemManager : SingletonMono<ItemManager>
{
    public List<Item> items = new List<Item>();
    public List<Sprite> sprites = new List<Sprite>();
    [SerializeField] private ItemUI[] itemUI;

    [SerializeField] private int gold = 100;
    private string _curScene;
    private TextMeshProUGUI _goldText;
    
    public int Gold
    {
        get => gold;
        set
        {
            if (gold < 0)
                return;

            gold = value;
        }
    }

    private void Update()
    {
        SceneCheck(() =>
        {
            if (GameObject.Find("GoldText").TryGetComponent(out _goldText))
            {
                _goldText.text = gold.ToString();
            }
        }, "Lobby", "Shop", "Map", "Ingame");
    }

    private void SceneCheck(Action action, params string[] targetScene)
    {
        foreach (var s in targetScene)
        {
            if (SceneManager.GetActiveScene().name.Contains(s) && _curScene != SceneManager.GetActiveScene().name)
            {
                action.Invoke();
            }
        }

        _curScene = SceneManager.GetActiveScene().name;
    }

    public void UseEffect(GameManager.GameState gameState)
    {
        foreach (var item in items)
        {
            item.Effect(gameState);
        }
    }

    public void ShowItem()
    {
        for (int i = 0; i < items.Count; i++)
        {
            itemUI[i].gameObject.SetActive(true);
            itemUI[i].SetItem(items[i].inGameIconSprite);
        }
    }

    public void HideItem()
    {
        for (int i = 0; i < 6; i++)
        {
            itemUI[i].gameObject.SetActive(false);
        }
    }

    public void UpdateGoldText()
    {
        _goldText.text = gold.ToString();
    }
}