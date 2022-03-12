using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameEndPanel : MonoBehaviour
{
    [SerializeField] private Image panelImage;
    [SerializeField] private Image goldImage;
    [SerializeField] private Button panelButton;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private VideoPlayer videoPlayer;

    [SerializeField] private int stageClearGold = 5;
    [SerializeField] private int gameClearGold = 30;

    [SerializeField] private CardSelectPanel cardSelectPanel;

    [SerializeField] private Sprite[] panelSprite;
    [SerializeField] private Sprite[] goldSprite;
    [SerializeField] private Sprite[] buttonSprite;
    
    public void Win()
    {
        bool hasItem = false;
        foreach (var x in ItemManager.Instance.items)
        {
            if (x.id == 6)
            {
                hasItem = true;
                break;
            }
        }

        int gold = gameClearGold + (hasItem ? gameClearGold / 2 : 0); 
            
        panelImage.sprite = panelSprite[1];
        goldImage.sprite = goldSprite[1];
        panelButton.image.sprite = buttonSprite[1];
        goldText.text = gold.ToString();
        descText.text = $"게임 클리어 = {gold}";
        
        panelButton.onClick.AddListener(CutScene);
        
        ItemManager.Instance.Gold += gold;
    }

    public void Lose()
    {
        panelImage.sprite = panelSprite[0];
        goldImage.sprite = goldSprite[0];
        panelButton.image.sprite = buttonSprite[0];
        goldText.text = "0";
        descText.text = "게임 클리어 실패";
        
        panelButton.onClick.AddListener(() => InGameManager.Instance.LoadScene("Lobby"));
    }

    public void StageClear()
    {
        bool hasItem = false;
        foreach (var x in ItemManager.Instance.items)
        {
            if (x.id == 6)
            {
                hasItem = true;
                break;
            }
        }

        int gold = stageClearGold + (hasItem ? (stageClearGold / 2) : 0);
        
        panelImage.sprite = panelSprite[1];
        goldImage.sprite = goldSprite[1];
        panelButton.image.sprite = buttonSprite[1];
        goldText.text = gold.ToString();
        descText.text = $"스테이지 클리어 = {gold}";
        
        panelButton.onClick.AddListener(() => Instantiate(cardSelectPanel, GameObject.Find("Canvas").transform));

        ItemManager.Instance.Gold += gold;
    }

    private void CutScene()
    {
        videoPlayer.gameObject.SetActive(true);
        videoPlayer.targetCamera = Camera.main;

        StartCoroutine(Co_PlayCutScene());
    }

    private IEnumerator Co_PlayCutScene()
    {
        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        print("Rmx");
        InGameManager.Instance.LoadScene("Lobby");
        yield return null;
    }
}