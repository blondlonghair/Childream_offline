using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemButton : MonoBehaviour
{
    public Item originItem;
    [HideInInspector] public Button button;

    private void Start()
    {
        TryGetComponent(out button);
        button.image.sprite = originItem.sprite;
    }
}