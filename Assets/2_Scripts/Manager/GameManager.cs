using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
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

    [Header("내부")]
    [SerializeField] private int _saveCurHp = 80;
    [SerializeField] private int _saveMaxHp = 80;
    [SerializeField] private int _saveCurMp = 3;
    [SerializeField] private int _saveMaxMp = 3;
    [SerializeField] private float attackTime;
    [SerializeField] private float attackInterval;
    
    [SerializeField] private GameState _gameState;
    private int _curStage = 0;
    private string _curScene;
    private int _curTurn;
    private Vector3 _mousePos;
    private CardObject _cardObject;
    private int _curMonster;
    private Coroutine _stateRoutine;
    
    [Header("가지고 가는거")]
    [SerializeField] private MonsterHpBar monsterHpBar;
    [SerializeField] private AtkEffect atkEffect;
    [SerializeField] private LoadingPanel loadingPanel;
    [SerializeField] private GameObject cardSelectPanel;
    [SerializeField] private GameEndPanel gameEndPanel;

    [Header("가지고 가지 않는거")]
    public Player player;
    public List<Monster> monsters = new List<Monster>();
    [SerializeField] private Image statePanel;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Button turnEndButton;
    [SerializeField] private List<Monster> monsterObject;

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
        
        GameObject.FindWithTag("Player").TryGetComponent(out player);
        GameObject.Find("TurnPanel").TryGetComponent(out statePanel);
        GameObject.Find("TurnEndButton").TryGetComponent(out turnEndButton);
        
        canvas = GameObject.FindWithTag("Canvas");
        turnEndButton.onClick.AddListener(TurnEndButton);

        LoadMonster(_curStage);
        
        foreach (var monster in monsters)
        {
            var monsterTransform = monster.transform;
            MonsterHpBar hpBar = Instantiate(monsterHpBar, monsterTransform.position + Vector3.up * 2,
                quaternion.identity, canvas.transform);
            AtkEffect atkEft = Instantiate(atkEffect, monsterTransform.position + Vector3.up * 3,
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
        //TODO 씬 변경방법 변경
        // if (SceneManager.GetActiveScene().name.Contains("Stage") && $"Stage{_curStage}" != SceneManager.GetActiveScene().name)
        // {
        //     string[] scene = SceneManager.GetActiveScene().name.Split("Stage");
        //     _curStage = int.Parse(scene[1]);
        //     
        //     loadingPanel.Open();
        //     OnChangeScene();
        // }

        if (SceneManager.GetActiveScene().name == "Ingame" && _curScene != SceneManager.GetActiveScene().name)
        {
            _curStage++;
            loadingPanel.Open();
            OnChangeStage();

            _curScene = SceneManager.GetActiveScene().name;
        }

        if (SceneManager.GetActiveScene().name == "Map" && _curScene != SceneManager.GetActiveScene().name)
        {
            loadingPanel.Open();

            _curScene = SceneManager.GetActiveScene().name;
        }
        
        if (SceneManager.GetActiveScene().name != "Ingame" && SceneManager.GetActiveScene().name != "Map")
        {
            Destroy(gameObject);
            Destroy(EffectManager.Instance.gameObject);
            Destroy(CardManager.Instance.gameObject);
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
        
        ItemManager.Instance.ShowItem();

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
            WaitChangeState(GameState.GameEnd);
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
        if (player.CurHp <= 0)
        {
            //게임 클리어 실패시
            Instantiate(gameEndPanel, canvas.transform).Lose(_curStage);
        }
        
        player.Vulnerable -= 1;
        player.Weakness -= 1;
        if (player.Armor > 0)
        {
            player.Armor = 0;
        }

        foreach (var monster in monsters)
        {
            monster.Vulnerable -= 1;
            monster.Weakness -= 1;
            monster.Armor = 0;
        }

        player.stateBar.HpLerp();

        ChangeState(GameState.PlayerTurnStart);
    }

    private void OnGameEnd()
    {
        Instantiate(cardSelectPanel, canvas.transform);
        
        _saveCurHp = player.CurHp;
        _saveMaxHp = player.MaxHp;
        _saveCurMp = player.MaxMp;
        _saveMaxMp = player.MaxMp;
        
        CardManager.Instance.ClearBuffer();
        ItemManager.Instance.HideItem();
        ChangeState(GameState.None);
    }

    private void ChangeState(GameState gameState)
    {
        _gameState = gameState;
    }
    
    private void WaitChangeState(GameState gameState)
    {
        if (_stateRoutine != null)
        {
            StopCoroutine(_stateRoutine);
        }
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
    
    private void MouseInput()
    {
        if (_curScene != "Ingame")
            return;

        if (EventSystem.current.IsPointerOverGameObject())
        {
            _cardObject?.CardZoomOut();
            _cardObject = null;
            return;
        }
        
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePos.z = 0;
        
        if (Input.GetMouseButton(0))
        {
            if (TryCastRay(out CardObject cardObj))
            {
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
            if (player.CurMp < _cardObject?.cost)
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
            
            else if (_cardObject?.originCard.type == Card.CardType.Grid)
            {
                if (TryCastRay(out Range range))
                {
                    _cardObject?.originCard.Effect(player, null);
                    player.Move(range.rangeType);
                    
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
        //TODO 씬 변경
        loadingPanel.Close(() =>
        {
            SceneManager.LoadScene("Map");
            // _curStage++;
            // OnChangeStage();

            // loadingPanel.Open();
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