using System;
using TMPro;
using UnityEngine;

public class ItemUI : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        TryGetComponent(out spriteRenderer);
    }

    public void SetItem(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}