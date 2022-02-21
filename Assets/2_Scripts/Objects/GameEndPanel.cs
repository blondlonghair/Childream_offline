using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndPanel : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI descText;

    [SerializeField] private int stageClearGold = 5;
    [SerializeField] private int gameClearGold = 30;

    public void Win(int stage)
    {
        int gold = (stage - 1) * stageClearGold + gameClearGold + (ItemManager.Instance.items.Any((x) => x.id == 6) ? ((stage - 1) * stageClearGold + gameClearGold) / 2 : 0);
        
        winPanel.SetActive(true);
        losePanel.SetActive(false);
        goldText.text = gold.ToString();
        descText.text = $"클리어한 스테이지({stage}) = {(stage - 1) * stageClearGold}\n클리어 보상(1) = " +
                        $"{gameClearGold}{(ItemManager.Instance.items.Any((x) => x.id == 6) ? $"\n유물:돈봉투 = {((stage - 1) * stageClearGold + gameClearGold) / 2}" : ' ')}";
        
        ItemManager.Instance.Gold += gold;
    }

    public void Lose(int stage)
    {
        int gold = (stage - 1) * stageClearGold +  + (ItemManager.Instance.items.Any((x) => x.id == 6) ? ((stage - 1) * stageClearGold + gameClearGold) / 2 : 0);
        
        winPanel.SetActive(false);
        losePanel.SetActive(true);
        goldText.text = gold.ToString();
        descText.text = $"클리어한 스테이지({stage}) = {(stage - 1) * stageClearGold}" +
                        $"{(ItemManager.Instance.items.Any((x) => x.id == 6) ? $"\n유물:돈봉투 = {(stage - 1) * stageClearGold / 2}" : ' ')}";

        ItemManager.Instance.Gold += gold;
    }

    public void LobbyButton()
    {
        GameManager.Instance.LoadScene("Lobby");
    }
}