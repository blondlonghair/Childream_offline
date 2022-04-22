using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    [SerializeField] private TextMeshProUGUI bubbleText;
    [SerializeField] private Image backGround;
    [SerializeField] private Sprite[] bgSprite;
    [SerializeField] private float nextTextTime;
    
    private TutorialState _tutorialState = TutorialState.Looby;
    private bool _isWriting;
    private bool _isSkip;
    private Coroutine _coroutine;
    private int _curCount;
    
    void Start()
    {
        loadingPanel.gameObject.transform.position = Vector3.zero;
        loadingPanel.Open(() => InitBubble("안녕? 나는 너의 게임시작을 위해 나타난 도우미야!"));
    }

    void Update()
    {
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
                break;
            case 3:
                break;
            case 4:
                InitBubble("우측 상단에 홈버튼을 누르면 다시 로비화면으로 갈 수 있어");
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
        bubbleText.text = "";
        
        for (int i = 0; i < str.Length; i++)
        {
            if (_isSkip)
            {
                bubbleText.text = str;
                _isSkip = false;
                break;
            }
            
            bubbleText.text += str[i];
            yield return YieldCache.WaitForSeconds(nextTextTime);
        }

        _isWriting = false;
        yield return null;
    }
}
