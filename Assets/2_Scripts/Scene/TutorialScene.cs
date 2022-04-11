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

    [SerializeField] private Image bubbleImage;
    [SerializeField] private TextMeshProUGUI bubbleText;
    [SerializeField] private Image backGround;
    [SerializeField] private Sprite[] bgSprite;
    
    private TutorialState _tutorialState = TutorialState.Looby;
    private bool _isWriting;
    private bool _isSkip;
    private Coroutine _coroutine;
    
    void Start()
    {
        InitBubble("kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk");
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

            switch (_tutorialState)
            {
                case TutorialState.Looby:
                    InitBubble("dddddddddddddddddddddddddddddddddddddddddddddddddddddddddddd");
                    _tutorialState = TutorialState.Shop;
                    break;
                case TutorialState.Shop:
                    InitBubble("ccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccc");
                    break;
                case TutorialState.InGame:
                    break;
            }
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
            yield return YieldCache.WaitForSeconds(0.02f);
        }

        _isWriting = false;
        yield return null;
    }
}
