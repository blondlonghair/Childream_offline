using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    private Image _image;
    private string _itemText;

    private void Start()
    {
        TryGetComponent(out _image);
        TryGetComponent(out EventTrigger eventTrigger);
        
        EventTrigger.Entry entryPointerDown = new EventTrigger.Entry();
        entryPointerDown.eventID = EventTriggerType.PointerDown;
        entryPointerDown.callback.AddListener((data) => { ItemManager.Instance.itemDesc.TextOn(_itemText); });
        eventTrigger.triggers.Add(entryPointerDown);
        
        EventTrigger.Entry entryPointerUp = new EventTrigger.Entry();
        entryPointerUp.eventID = EventTriggerType.PointerUp;
        entryPointerUp.callback.AddListener((data) => { ItemManager.Instance.itemDesc.TextOff(_itemText); });
        eventTrigger.triggers.Add(entryPointerUp);
    }

    public void SetItem(Sprite sprite, string str)
    {
        Color aColor = Color.white;
        _image.color = aColor;
        _image.sprite = sprite;
        _itemText = str;

        if (sprite == null)
        {
            aColor = Color.clear;
            _image.color = aColor;
        }
    }
}