using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialScene : MonoBehaviour
{
    private enum TutorialState
    {
        Looby,
        Shop,
        InGame
    }
    [SerializeField] private LoadingPanel loadingPanel;

    [SerializeField] private Image bubbleImage;
    [SerializeField] private Image playerImage;
    [SerializeField] private TextMeshProUGUI bubbleText;
    [SerializeField] private Image backGround;
    [SerializeField] private Sprite[] bgSprite;
    [SerializeField] private float nextTextTime;
    
    private TutorialState _tutorialState = TutorialState.Looby;
    private bool _isWriting;
    private bool _isSkip;
    private Coroutine _coroutine;
    private int _curCount;
    private Vector3 _playerPos;

    private StringBuilder _stringBuilder = new StringBuilder();
    
    void Start()
    {
        loadingPanel.gameObject.transform.position = Vector3.zero;
        loadingPanel.Open(() => InitBubble("안녕? 나는 너의 게임시작을 위해 나타난 도우미야!"));
        _playerPos = playerImage.transform.position;
    }

    void Update()
    {
        playerImage.transform.position = _playerPos + Vector3.up * 0.5f * Mathf.Sin(Time.time);
        
        if (Input.GetMouseButtonDown(0))
        {
            if (_isWriting)
            {
                _isSkip = true;
                return;
            }

            _curCount++;

            switch (_tutorialState)
            {
                case TutorialState.Looby: OnLobby(); break;
                case TutorialState.Shop: OnShop(); break;
                case TutorialState.InGame: OnIngame(); break;
            }
        }
    }

    private void OnLobby()
    {
        switch (_curCount)
        {
            case 1:
                InitBubble("지금 너가 보고있는 이 화면은 게임을 시작하면 볼수 있는 로비화면이야");
                break;
            case 2:
                InitBubble("여기에서는 게임을 시작하기 전에 상점에 들어가거나 설정을 변경할 수 있어");
                break;
            case 3:
                InitBubble("일단 상점부터 들어가보자");
                _curCount = 0;
                _tutorialState = TutorialState.Shop;
                break;
        }

    }

    private void OnShop()
    {
        switch (_curCount)
        {
            case 1:
                backGround.sprite = bgSprite[1];
                InitBubble("여기는 상점화면이야");
                break;
            case 2:
                InitBubble("던전에 들어가기 전에 아이템을 구매해서 던전에서 사용할 수 있어");
                break;
            case 3:
                InitBubble("아이템을 터치하고 구매버튼을 누르면 아이템을 살수 있어");
                break;
            case 4:
                InitBubble("우측 상단에 홈버튼을 누르면 다시 로비화면으로 갈 수 있어");
                _curCount = 0;
                _tutorialState = TutorialState.InGame;
                break;
        }
    }

    private void OnIngame()
    {
        switch (_curCount)
        {
            case 1:
                backGround.sprite = bgSprite[2];
                InitBubble("로비화면에서 도전버튼을 누르면 던전으로 입장 할 수 있어");
                break;
            case 2:
                InitBubble("너가 가지고 있는 카드들을 이용해서 적을 공격할 수 있어");
                break;
            case 3:
                InitBubble("그리고 적을 처치하면 다음 스테이지로 갈 수 있어");
                break;
            case 4:
                InitBubble("너의 목표는 사악한 마왕을 처치하는거야");
                break;
            case 5:
                InitBubble("행운을 빌어!");
                break;
            case 6:
                loadingPanel.Close(() => SceneManager.LoadScene("Lobby"));
                _curCount = 0;
                break;
        }
    }

    private void InitBubble(string str)
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        
        _coroutine = StartCoroutine(Co_InitBubble(str));
    }

    private IEnumerator Co_InitBubble(string str)
    {
        _isWriting = true;
        bubbleImage.gameObject.SetActive(true);
        _stringBuilder.Clear();
        
        for (int i = 0; i < str.Length; i++)
        {
            if (_isSkip)
            {
                _stringBuilder.Clear();
                _stringBuilder.Append(str);
                
                bubbleText.text = _stringBuilder.ToString();
                _isSkip = false;
                break;
            }

            _stringBuilder.Append(str[i]);
            bubbleText.text = _stringBuilder.ToString();
            yield return YieldCache.WaitForSeconds(nextTextTime);
        }

        _isWriting = false;
        yield return null;
    }
}
