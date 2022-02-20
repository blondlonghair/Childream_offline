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
    private Item _curSelectedItem;

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

            itemButtons[i].originItem = _itemDictionary[i];
            itemButtons[i].button.image.sprite = itemButtons[i].originItem.inGameSprite;
            itemButtons[i].button.onClick.AddListener(() =>
            {
                _curSelectedItem = itemButtons[i1].originItem;
                SetBubble(itemButtons[i1].originItem.desc);
            });
        }
    }

    public void BuyItem()
    {
        if (ItemManager.Instance.Gold < _curSelectedItem.cost ||
            ItemManager.Instance.items.Any((x) => x.id == _curSelectedItem.id))
            return;

        ItemManager.Instance.items.Add(_curSelectedItem);
        ItemManager.Instance.Gold -= _curSelectedItem.cost;
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
        StartCoroutine(Co_SetBubble());
    }

    private IEnumerator Co_SetBubble()
    {
        Color color = bubbleImage.color;

        while (color.a <= 1)
        {
            color.a += 0.01f;
            bubbleImage.color = color;
            bubbleText.color = color;
            yield return YieldCache.WaitForSeconds(0.01f);
        }

        while (color.a >= 0)
        {
            color.a -= 0.01f;
            bubbleImage.color = color;
            bubbleText.color = color;
            yield return YieldCache.WaitForSeconds(0.01f);
        }
    }
}