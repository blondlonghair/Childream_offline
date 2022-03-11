using System;
using System.Collections.Generic;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ItemManager : SingletonMono<ItemManager>
{
    [Header("Data")]
    [SerializeField] private int gold = 100;
    public List<Item> items = new List<Item>();
    [SerializeField] private ItemUI[] itemUI;

    [Header("Sprite")]
    public List<Sprite> sprites = new List<Sprite>();
    
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

    private void Start()
    {
        // items.Add(new Knuckle());
    }

    private void Update()
    {
        if (SceneCheck("Lobby", "Shop", "Map", "Ingame"))
        {
            if (GameObject.Find("GoldText").TryGetComponent(out _goldText))
            {
                _goldText.text = gold.ToString();
            }
        }

        if (SceneCheck("Lobby", "Map", "Ingame"))
        {
            ShowItem();
        }

        if (SceneCheck("Shop", "Tutorial"))
        {
            HideItem();
        }
    }

    private bool SceneCheck( params string[] targetScene)
    {
        foreach (var s in targetScene)
        {
            if (SceneManager.GetActiveScene().name.Contains(s) && _curScene != SceneManager.GetActiveScene().name)
            {
                return true;
            }
        }

        _curScene = SceneManager.GetActiveScene().name;
        return false;
    }

    public void UseEffect(InGameManager.GameState gameState)
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