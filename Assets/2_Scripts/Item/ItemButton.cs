using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Item originItem;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI costText;

    private void Start()
    {
        TryGetComponent(out button);
    }

    public void Setup(Item item, UnityAction action)
    {
        originItem = item;
        button.image.sprite = ItemManager.Instance.items.Exists(x => x.id == originItem.id)
            ? originItem.shopSoldSprite : originItem.shopUnSelectSprite;
        nameText.text = originItem.name;
        costText.text = originItem.cost.ToString();
        button.onClick.AddListener(action);
    }

    public void SetSprite(Sprite sprite)
    {
        button.image.sprite = sprite;
    }
}