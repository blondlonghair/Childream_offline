using System.Collections.Generic;
using Items;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopScene : MonoBehaviour
{
    [SerializeField] private ItemButton[] itemButtons;
    private Dictionary<int, Item> _itemDictionary = new Dictionary<int, Item>();

    [SerializeField] private LoadingPanel loadingPanel;

    private void Start()
    {
        loadingPanel.gameObject.transform.position = Vector3.zero;
        loadingPanel.Open();

        _itemDictionary.Add(0, new BloodPack());
        _itemDictionary.Add(1, new MonsterBook());
        _itemDictionary.Add(2, new Knuckle());
        _itemDictionary.Add(3, new SmoothStone());
        _itemDictionary.Add(4, new Sail());
        _itemDictionary.Add(5, new MoneyBag());

        for (int i = 0; i < _itemDictionary.Count; i++)
        {
            var i1 = i;
            itemButtons[i].originItem = _itemDictionary[i];
            itemButtons[i].button.onClick.AddListener(() => ItemManager.Instance.items.Add(itemButtons[i1].originItem));
        }
    }

    public void LobbyButton()
    {
        loadingPanel.Close(() => SceneManager.LoadScene("Lobby"));
    }
}