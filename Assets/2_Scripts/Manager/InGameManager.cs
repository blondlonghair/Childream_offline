using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameManager : SingletonMono<InGameManager>
{
    public enum GameState
    {
        None,
        GameStart,
        PlayerTurnStart,
        PlayerTurn,
        PlayerTurnEnd,
        EnemyTurnStart,
        EnemyTurn,
        EnemyTurnEnd,
        GameEnd
    }

    [Header("내부")]
    [SerializeField] private int _saveCurHp = 80;
    [SerializeField] private int _saveMaxHp = 80;
    [SerializeField] private int _saveCurMp = 3;
    [SerializeField] private int _saveMaxMp = 3;
    [SerializeField] private float attackTime;
    [SerializeField] private float attackInterval;
    
    [SerializeField] private GameState _gameState;
    private int _curStage = 1;
    private int _curTurn;
    private Vector3 _mousePos;
    private CardObject _cardObject;
    private int _curMonster;
    private Coroutine _stateRoutine;
    private CardObject _mouseOnCard;
    private bool _firstCall = true;
    
    [Header("가지고 가는거")]
    [SerializeField] private MonsterHpBar monsterHpBar;
    [SerializeField] private AtkEffect atkEffect;
    [SerializeField] private LoadingPanel loadingPanel;
    [SerializeField] private GameEndPanel gameEndPanel;

    [Header("가지고 가지 않는거")]
    public Player player;
    public List<Monster> monsters = new List<Monster>();
    public Range leftGrid;
    public Range middleGrid;
    public Range rightGrid;
    
    [SerializeField] private Image statePanel;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Button turnEndButton;
    [SerializeField] private List<Monster> monsterObject;
    [SerializeField] private MatchingDoor matchingDoor;
    [SerializeField] private SpriteRenderer backGround;
    [SerializeField] private SpriteRenderer[] ranges;

    [SerializeField] private Sprite[] backGroundSprite;
    [SerializeField] private Sprite[] rangeSprites;

    public int CurStage
    {
        get => _curStage;
    }
    
    public GameState GameStates
    {
        get => _gameState;
    }

    private void OnChangeStage()
    {
        CardManager.Instance.OnChangeScene();
        // ItemManager.Instance.OnSceneLoaded();
        
        GameObject.FindWithTag("Player").TryGetComponent(out player);
        GameObject.Find("TurnPanel").TryGetComponent(out statePanel);
        GameObject.Find("TurnEndButton").TryGetComponent(out turnEndButton);
        GameObject.Find("LeftRange").TryGetComponent(out leftGrid);
        GameObject.Find("MiddleRange").TryGetComponent(out middleGrid);
        GameObject.Find("RightRange").TryGetComponent(out rightGrid);
        if (GameObject.Find("StageText").TryGetComponent(out TextMeshProUGUI stageText))
        {
            stageText.text = $"{_curStage} 스테이지";
        }
        canvas = GameObject.FindWithTag("Canvas");
        
        turnEndButton.onClick.AddListener(TurnEndButton);
        LoadMonster(_curStage);
        
        foreach (var monster in monsters)
        {
            var monsterTransform = monster.transform;
            MonsterHpBar hpBar = Instantiate(monsterHpBar, monsterTransform.position + Vector3.up * 2,
                Quaternion.identity, canvas.transform);
            AtkEffect atkEft = Instantiate(atkEffect, monsterTransform.position + Vector3.up * 3,
                Quaternion.identity);
            monster.hpBar = hpBar;
            monster.atkEffect = atkEft;
        }

        ChangeState(GameState.GameStart);
    }

    private void Update()
    {
        MouseInput();
        SceneCheck();

        ItemManager.Instance.UseEffect(_gameState);

        switch (_gameState)
        {
            case GameState.GameStart: OnGameStart(); break;
            case GameState.PlayerTurnStart: OnPlayerTurnStart(); break;
            case GameState.PlayerTurn: OnPlayerTurn(); break;
            case GameState.PlayerTurnEnd: OnPlayerTurnEnd(); break;
            case GameState.EnemyTurnStart: OnEnemyTurnStart(); break;
            case GameState.EnemyTurn: OnEnemyTurn(); break;
            case GameState.EnemyTurnEnd: OnEnemyTurnEnd(); break;
            case GameState.GameEnd: OnGameEnd(); break;
        }
    }

    private void SceneCheck()
    {
        if (SceneManager.GetActiveScene().name != "Ingame")
        {
            Destroy(gameObject);
            Destroy(EffectManager.Instance.gameObject);
            Destroy(CardManager.Instance.gameObject);
        }

        GameObject.Find("BackGround").TryGetComponent(out backGround);
        GameObject.Find("LeftRange").TryGetComponent(out ranges[0]);
        GameObject.Find("MiddleRange").TryGetComponent(out ranges[1]);
        GameObject.Find("RightRange").TryGetComponent(out ranges[2]);
        
        if (_firstCall)
        {
            //배경음
            if (_curStage == 1)
            {
                matchingDoor.OpenDoor(null);
                SoundManager.Instance.PlayBGMSound("Stage1~3");
            }
            
            if (_curStage == 4)
            {
                SoundManager.Instance.PlayBGMSound("Stage4~6");
            }
        
            if (_curStage == 7)
            {
                SoundManager.Instance.PlayBGMSound("Stage7~9");
            }

            if (_curStage == 10)
            {
                SoundManager.Instance.PlayBGMSound("BossStage");
            }

            //배경이미지
            if (_curStage >= 1 && _curStage <= 3)
            {
                backGround.sprite = backGroundSprite[0];
                for (int i = 0; i < 3; i++)
                {
                    ranges[i].sprite = rangeSprites[i];
                }
            }

            if (_curStage >= 4 && _curStage <= 6)
            {
                backGround.sprite = backGroundSprite[1];
                for (int i = 0; i < 3; i++)
                {
                    ranges[i].sprite = rangeSprites[i + 3];
                }
            }

            if (_curStage >= 7 && _curStage <= 10)
            {
                backGround.sprite = backGroundSprite[2];
                for (int i = 0; i < 3; i++)
                {
                    ranges[i].sprite = rangeSprites[i + 6];
                }
            }

            //로직
            loadingPanel.Open(null);
            OnChangeStage();

            _firstCall = false;
        }
    }

    private void OnGameStart()
    {
        player.CurHp = _saveCurHp;
        player.MaxHp = _saveMaxHp;
        player.CurMp = _saveCurMp;
        player.MaxMp = _saveMaxMp;
        player.stateBar.HpLerp();
        player.stateBar.MpLerp();
        
        WaitChangeState(GameState.PlayerTurnStart);
    }

    private void OnPlayerTurnStart()
    {
        foreach (var monster in monsters)
        {
            monster.ShowNextAction();
        }

        for (int i = 0; i < 4; i++)
        {
            CardManager.Instance.DrawCard();
        }

        player.CurMp = player.MaxMp;

        WaitChangeState(GameState.PlayerTurn);
    }
    
    private void OnPlayerTurn()
    {
        if (monsters.Count <= 0)
        {
            //게임 클리어
            WaitChangeState(GameState.GameEnd);
        }
    }
    
    private void OnPlayerTurnEnd()
    {
        foreach (var monster in monsters)
        {
            monster.Vulnerable -= 1;
            monster.Weakness -= 1;

            if (monster.Armor > 0)
            {
                monster.Armor = 0;
            }

            monster.hpBar.Lerp();
        }

        for (int i = CardManager.Instance.cards.Count - 1; i >= 0; i--)
        {
            CardManager.Instance.DestroyCard(CardManager.Instance.cards[i]);
        }
        
        ChangeState(GameState.EnemyTurnStart);
    }

    private void OnEnemyTurnStart()
    {
        WaitChangeState(GameState.EnemyTurn);
    }

    private bool isShowAttackGrid = true;
    
    private void OnEnemyTurn()
    {
        if (isShowAttackGrid && monsters.Count > _curMonster)
        {
            monsters[_curMonster].ShowAttackGrid();
            isShowAttackGrid = false;
        }
        
        if (attackTime > attackInterval)
        {
            if (_curMonster < monsters.Count)
            {
                monsters[_curMonster].Attack();
                _curMonster++;
            }
            
            else if (_curMonster >= monsters.Count)
            {
                ChangeState(GameState.EnemyTurnEnd);
                _curMonster = 0;
            }

            isShowAttackGrid = true;
            attackTime = 0;
        }
        
        attackTime += Time.deltaTime;
    }

    private void OnEnemyTurnEnd()
    {
        if (player.CurHp <= 0)
        {
            //게임 클리어 실패시
            Instantiate(gameEndPanel, canvas.transform).Lose();
            // GameEndPanelLose();
        }
        
        player.Vulnerable -= 1;
        player.Weakness -= 1;
        if (player.Armor > 0)
        {
            player.Armor = 0;
        }

        player.stateBar.HpLerp();

        ChangeState(GameState.PlayerTurnStart);
    }

    private void OnGameEnd()
    {
        CardManager.Instance.ClearBuffer();
        // ItemManager.Instance.HideItem();
        ChangeState(GameState.None);
        
        if (_curStage == 10)
        {
            Instantiate(gameEndPanel, canvas.transform).Win();
            // GameEndPanelWin();
            PlayerPrefs.SetInt("ClearOnce", 1);
            return;
        }
        
        Instantiate(gameEndPanel, canvas.transform).StageClear();
        // Instantiate(cardSelectPanel, canvas.transform);
        
        _saveCurHp = player.CurHp;
        _saveMaxHp = player.MaxHp;
        _saveCurMp = player.MaxMp;
        _saveMaxMp = player.MaxMp;
    }

    private void ChangeState(GameState gameState)
    {
        SoundManager.Instance.PlaySFXSound("ChangeTurn");
        
        _gameState = gameState;
    }
    
    private void WaitChangeState(GameState gameState)
    {
        if (_stateRoutine != null)
        {
            StopCoroutine(_stateRoutine);
        }
        SoundManager.Instance.PlaySFXSound("ChangeTurn");

        _gameState = GameState.None;

        _stateRoutine = StartCoroutine(Co_ChangeState(gameState));
    }

    private IEnumerator Co_ChangeState(GameState gameState)
    {
        Color statePanelColor = statePanel.color;
        TextMeshProUGUI stateText = statePanel.GetComponentInChildren<TextMeshProUGUI>();
        stateText.text = gameState switch
        {
            GameState.PlayerTurnStart => $"스테이지 {_curStage}",
            GameState.PlayerTurn => "플레이어 턴",
            GameState.EnemyTurn => "적 턴",
            GameState.GameEnd => "끝"
        };
        
        while (statePanelColor.a < 1)
        {
            statePanelColor.a += 0.03f;
            statePanel.color = statePanelColor;
            stateText.color = statePanelColor;

            yield return YieldCache.WaitForSeconds(0.01f);
        }

        yield return YieldCache.WaitForSeconds(1);
        
        while (statePanelColor.a > 0)
        {
            statePanelColor.a -= 0.03f;
            statePanel.color = statePanelColor;
            stateText.color = statePanelColor;

            yield return YieldCache.WaitForSeconds(0.01f);
        }

        _gameState = gameState;
        yield return null;
    }
    
    /// <summary>
    /// 마우스 인풋에 대한 모든것을 처리해주는 함수
    /// </summary>
    private void MouseInput()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePos.z = 0;

        // if (EventSystem.current.IsPointerOverGameObject())
        // {
        //     _cardObject?.CardZoomOut();
        //     _cardObject = null;
        //     return;
        // }
        
        if (Input.GetMouseButtonDown(0))
        {
            TryCastRay(out _cardObject);
        }
        
        else if (Input.GetMouseButton(0))
        {
            if (_cardObject == null)
            {
                return;
            }

            switch (_cardObject.originCard.type)
            {
                case Card.CardType.One:
                case Card.CardType.Grid:
                    if (TryCastRay("CardRange"))
                    {
                        _cardObject.CardZoomOut();
                        _cardObject.UpdateLine(_mousePos);
                    }
                    else
                    {
                        _cardObject.CardZoomIn();
                        _cardObject.CloseLine();
                    }
                    break;
                case Card.CardType.All:
                case Card.CardType.None:
                    if (TryCastRay("CardRange"))
                    {
                        _cardObject.StopCoroutine();
                        _cardObject.transform.rotation = Quaternion.Euler(0,0,0);
                        _cardObject.transform.position = _mousePos;
                    }
                    else
                    {
                        _cardObject.CardZoomIn();
                    }
                    break;
            }
        }

        #region OnMouseOver
        // //마우스 위에 있을때
        // else
        // {
        //     if (TryCastRay(out CardObject cardObj))
        //     {
        //         if (_cardObject != cardObj)
        //         {
        //             _cardObject?.CardZoomOut();
        //             _cardObject = cardObj;
        //             _cardObject?.CardZoomIn();
        //         }
        //     }
        //     else
        //     {
        //         _cardObject?.CardZoomOut();
        //         // _cardObject = null;
        //     }
        // }
        #endregion
        
        //카드 효과 발동
        else if (Input.GetMouseButtonUp(0))
        {
            if (player.CurMp < _cardObject?.cost)
            {
                return;
            }

            switch (_cardObject?.originCard.type)
            {
                case Card.CardType.One:
                    if (TryCastRay(out Monster monster))
                    {
                        _cardObject?.originCard.Effect(player, monster);
                    
                        goto default;
                    }
                    break;
                case Card.CardType.Grid:
                    if (TryCastRay(out Range range))
                    {
                        _cardObject?.originCard.Effect(player, null);
                        player.Move(range.rangeType);
                        goto default;
                    }
                    break;
                case Card.CardType.All:
                    if (TryCastRay("CardRange"))
                    {
                        _cardObject?.originCard.Effect(player, monsters.ToArray());
                        goto default;
                    }
                    break;
                case Card.CardType.None:
                    if (TryCastRay("CardRange"))
                    {
                        _cardObject?.originCard.Effect(player, null);
                        goto default;
                    }
                    break;
                default:
                    CardDestroy(_cardObject);
                    break;
            }
            
            _cardObject?.CardZoomOut();
            _cardObject?.CloseLine();
        }
    }

    private void CardDestroy(CardObject cardObject)
    {
        if (cardObject == null)
            return;
        
        CardManager.Instance.DestroyCard(cardObject);
        cardObject?.CloseLine();
        cardObject?.CardZoomOut();
        cardObject = null;
        CardManager.Instance.CardAlignment();
    }

    public void TurnEndButton()
    {
        if (_gameState == GameState.PlayerTurn)
        {
            ChangeState(GameState.PlayerTurnEnd);
        }
    }

    public void NextStage()
    {
        loadingPanel.Close(() =>
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            _firstCall = true;
            _curStage++;
        });
    }

    public void LoadScene(string scene)
    {
        loadingPanel.Close(() => SceneManager.LoadScene(scene));
    }

    public void LoadMonster(int pattern)
    {
        Monster monster = Instantiate(monsterObject[pattern - 1], new Vector3(0, 5, 0), Quaternion.identity);
        monsters.Add(monster);
    }

    private bool TryCastRay(string tag)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(_mousePos, Vector2.zero, 0f);
        
        foreach (var hit2D in hits)
        {
            if (hit2D.collider.gameObject.CompareTag(tag))
            {
                return true;
            }
        }

        return false;
    }
    
    private bool TryCastRay<T>(string tag, out T component) where T : class?
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(_mousePos, Vector2.zero, 0f);
        
        foreach (var hit2D in hits)
        {
            if (hit2D.collider.gameObject.CompareTag(tag))
            {
                component = hit2D.collider.gameObject.GetComponent<T>();
                return true;
            }
        }

        component = null;
        return false;
    }
    
    /// <summary>
    /// raycast를 해서 오브젝트의 컴포넌트를 가져온다
    /// </summary>
    /// <param name="component">out될 컴포넌트</param>
    /// <typeparam name="T">컴포넌트 타입</typeparam>
    /// <returns></returns>
    private bool TryCastRay<T>(out T component) where T : class?
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(_mousePos, Vector2.zero, 0f);
        
        foreach (var hit2D in hits)
        {
            if (hit2D.collider.gameObject.TryGetComponent(out T hi))
            {
                component = hit2D.collider.gameObject.GetComponent<T>();
                return true;
            }
        }

        component = null;
        return false;
    }
}