using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : SingletonMono<GameManager>
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

    //내부
    private int _curStage = 1;
    private int _curTurn;
    [SerializeField] private int _saveCurHp = 80;
    [SerializeField] private int _saveMaxHp = 80;
    private int _saveCurMp = 3;
    private int _saveMaxMp = 3;

    [SerializeField] private GameState _gameState;
    private Vector3 _mousePos;
    private CardObject _cardObject;
    private int _curMonster;
    private Coroutine _stateRoutine;

    public Player player;
    public List<Monster> monsters = new List<Monster>();

    //가지고 가는거
    [Header("가지고 가는거")]
    [SerializeField] private float attackTime;
    [SerializeField] private float attackInterval;
    [SerializeField] private MonsterHpBar monsterHpBar;
    [SerializeField] private AtkEffect atkEffect;
    [SerializeField] private LoadingPanel loadingPanel;
    [SerializeField] private GameObject cardSelectPanel;

    //가지고 가지 않는거
    [Header("가지고 가지 않는거")]
    [SerializeField] private Image statePanel;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Button turnEndButton;

    public GameState GameStates
    {
        get => _gameState;
    }

    private void Start()
    {
        OnChangeScene();
    }

    private void OnChangeScene()
    {
        CardManager.Instance.OnChangeScene();
        
        GameObject.FindWithTag("Player").TryGetComponent(out player);
        canvas = GameObject.FindWithTag("Canvas");
        // GameObject.FindWithTag("CardSelectPanel").TryGetComponent(out cardSelectPanel);
        GameObject.Find("TurnPanel").TryGetComponent(out statePanel);
        GameObject.Find("TurnEndButton").TryGetComponent(out turnEndButton);
        turnEndButton.onClick.AddListener(TurnEndButton);
        
        // cardSelectPanel.gameObject.SetActive(false);

        foreach (var monster in monsters)
        {
            var monsterTransform = monster.transform;
            MonsterHpBar hpBar = Instantiate(monsterHpBar, monsterTransform.position + Vector3.up,
                quaternion.identity, canvas.transform);
            AtkEffect atkEft = Instantiate(atkEffect, monsterTransform.position + Vector3.up * 2,
                quaternion.identity);
            monster.hpBar = hpBar;
            monster.atkEffect = atkEft;
        }

        ChangeState(GameState.GameStart);
    }

    private void Update()
    {
        MouseInput();
        SceneCheck();

        ItemManager.Instance.UseEffect();

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
        if (SceneManager.GetActiveScene().name.Contains("Stage") && $"Stage{_curStage}" != SceneManager.GetActiveScene().name)
        {
            string[] scene = SceneManager.GetActiveScene().name.Split("Stage");
            _curStage = int.Parse(scene[1]);
            
            loadingPanel.Open();
            OnChangeScene();
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
        
        print("gamestart");
        WaitChangeState(GameState.PlayerTurnStart);
    }

    private void OnPlayerTurnStart()
    {
        foreach (var monster in monsters)
        {
            monster.ShowAttackPos();
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
            //게임 끝
            ChangeState(GameState.GameEnd);
        }
    }
    
    private void OnPlayerTurnEnd()
    {
        foreach (var monster in monsters)
        {
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

    private void OnEnemyTurn()
    {
        attackTime += Time.deltaTime;
        
        if (attackTime > attackInterval)
        {
            if (_curMonster < monsters.Count)
            {
                monsters[_curMonster].Attack();
                _curMonster++;
            }
            
            else
            {
                ChangeState(GameState.EnemyTurnEnd);
                _curMonster = 0;
            }

            attackTime = 0;
        }
    }

    private void OnEnemyTurnEnd()
    {
        player.Vulnerable -= 1;
        player.Weakness -= 1;
        player.armor = 0;

        foreach (var monster in monsters)
        {
            monster.Vulnerable -= 1;
            monster.Weakness -= 1;
            monster.armor = 0;
        }

        player.stateBar.HpLerp();

        ChangeState(GameState.PlayerTurnStart);
    }

    private void OnGameEnd()
    {
        Instantiate(cardSelectPanel, canvas.transform);
        
        _saveCurHp = player.CurHp;
        _saveMaxHp = player.MaxHp;
        _saveCurMp = player.CurMp;
        _saveMaxMp = player.MaxMp;
        
        CardManager.Instance.ClearBuffer();
        WaitChangeState(GameState.None);
    }

    // private GameState _curState;

    private void ChangeState(GameState gameState)
    {
        _gameState = gameState;
    }
    
    private void WaitChangeState(GameState gameState)
    {
        if (_stateRoutine != null)
        {
            StopCoroutine(_stateRoutine);
            // print(gameState.ToString());
        }
        _gameState = GameState.None;

        // _curState = gameState;
        
        _stateRoutine = StartCoroutine(Co_ChangeState(gameState));
    }

    private IEnumerator Co_ChangeState(GameState gameState)
    {
        Color statePanelColor = statePanel.color;
            
        while (statePanelColor.a < 1)
        {
            statePanelColor.a += 0.02f;
            statePanel.color = statePanelColor;

            yield return YieldCache.WaitForSeconds(0.01f);
        }

        while (statePanelColor.a > 0)
        {
            statePanelColor.a -= 0.02f;
            statePanel.color = statePanelColor;

            yield return YieldCache.WaitForSeconds(0.01f);
        }

        print(gameState.ToString());
        _gameState = gameState;
        yield return null;
    }
    
    private void MouseInput()
    {
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePos.z = 0;
        
        if (Input.GetMouseButton(0))
        {
            if (TryCastRay(out CardObject cardObj))
            {
                // cardObj.CardZoomOut();
                cardObj.transform.position = _mousePos;
            }
        }

        else
        {
            if (TryCastRay(out CardObject cardObj))
            {
                if (_cardObject != cardObj)
                {
                    _cardObject?.CardZoomOut();
                    _cardObject = cardObj;
                    _cardObject?.CardZoomIn();
                }
            }
            else
            {
                _cardObject?.CardZoomOut();
                _cardObject = null;
            }
        }

        //카드 효과 발동
        if (Input.GetMouseButtonUp(0))
        {
            if (player.curMp < _cardObject?.cost)
            {
                return;
            }

            if (_cardObject?.originCard.type == Card.CardType.One)
            {
                if (TryCastRay(out Monster monster))
                {
                    _cardObject?.originCard.Effect(player, monster);

                    if (_cardObject == null)
                        return;
                    
                    CardManager.Instance.DestroyCard(_cardObject);
                    _cardObject = null;
                }
            }

            else
            {
                if (_cardObject?.originCard.type == Card.CardType.All)
                {
                    _cardObject?.originCard.Effect(player, monsters.ToArray());
                }

                else if (_cardObject?.originCard.type == Card.CardType.None)
                {
                    _cardObject?.originCard.Effect(player, null);
                }
                
                if (_cardObject == null)
                    return;
                
                CardManager.Instance.DestroyCard(_cardObject);
                _cardObject = null;
            }

            CardManager.Instance.CardAlignment();
        }
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
        loadingPanel.Close(() => SceneManager.LoadScene($"Stage{_curStage + 1}"));
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