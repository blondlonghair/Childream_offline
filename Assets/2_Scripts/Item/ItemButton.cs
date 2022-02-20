using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Item originItem;
    [HideInInspector] public Button button;

    private void Start()
    {
        TryGetComponent(out button);
    }

    public void Setup()
    {
        button.image.sprite = originItem.inGameSprite;
        button.onClick.AddListener(() =>
        {
            if (ItemManager.Instance.Gold < originItem.cost || 
                ItemManager.Instance.items.Any((x) => x.id == originItem.id))
                return;
                
            ItemManager.Instance.items.Add(originItem);
            ItemManager.Instance.Gold -= originItem.cost;
        });
    }
}