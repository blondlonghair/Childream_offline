using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Items;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopScene : MonoBehaviour
{
    [SerializeField] private ItemButton[] itemButtons;
    [SerializeField] private LoadingPanel loadingPanel;
    [SerializeField] private Image bubbleImage;
    [SerializeField] private TextMeshProUGUI bubbleText;

    private Dictionary<int, Item> _itemDictionary = new Dictionary<int, Item>();
    private Coroutine _bubbleCoroutine;
    private ItemButton _curSelectedItem;

    private void Start()
    {
        loadingPanel.gameObject.transform.position = Vector3.zero;
        loadingPanel.Open();

        _itemDictionary.Add(0, new BloodPack());
        _itemDictionary.Add(1, new MonsterBook());
        _itemDictionary.Add(2, new Knuckle());
        _itemDictionary.Add(3, new SmoothStone());
        _itemDictionary.Add(4, new PlateArmor());
        _itemDictionary.Add(5, new MoneyBag());

        for (int i = 0; i < _itemDictionary.Count; i++)
        {
            var i1 = i;

            itemButtons[i].Setup(_itemDictionary[i], () =>
            {
                _curSelectedItem = itemButtons[i1];
                SetBubble(itemButtons[i1].originItem.desc);

                foreach (var itemButton in itemButtons)
                {
                    itemButton.SetSprite(itemButton.originItem.shopUnSelectSprite);
                }

                itemButtons[i1].SetSprite(itemButtons[i1].originItem.shopSelectSprite);
            });
        }
        
        ItemManager.Instance.ShowItem();
    }

    public void BuyItem()
    {
        if (ItemManager.Instance.items.Any((x) => x.id == _curSelectedItem.originItem.id))
        {
            SetBubble("이미 가지고 있는 아이템이지 않은가..?");
            return;
        }
        
        if (ItemManager.Instance.Gold < _curSelectedItem.originItem.cost)
        {
            SetBubble("돈이 부족한지 않은지 생각해보게...");
            return;
        }

        SetBubble("구매해줘서 고맙다네");
        _curSelectedItem.SetSprite(_curSelectedItem.originItem.shopSoldSprite);
        ItemManager.Instance.items.Add(_curSelectedItem.originItem);
        ItemManager.Instance.Gold -= _curSelectedItem.originItem.cost;
        ItemManager.Instance.UpdateGoldText();
    }

    public void LobbyButton()
    {
        loadingPanel.Close(() => SceneManager.LoadScene("Lobby"));
    }

    public void SetBubble(string desc)
    {
        if (_bubbleCoroutine != null)
        {
            StopCoroutine(_bubbleCoroutine);
        }

        bubbleText.text = desc;
        _bubbleCoroutine = StartCoroutine(Co_SetBubble());
    }

    private IEnumerator Co_SetBubble()
    {
        Color color = bubbleImage.color;

        while (color.a <= 1)
        {
            color.a += 0.05f;
            bubbleImage.color = color;
            bubbleText.color = color;
            yield return YieldCache.WaitForSeconds(0.01f);
        }

        yield return YieldCache.WaitForSeconds(2);

        while (color.a >= 0)
        {
            color.a -= 0.05f;
            bubbleImage.color = color;
            bubbleText.color = color;
            yield return YieldCache.WaitForSeconds(0.01f);
        }
    }
}