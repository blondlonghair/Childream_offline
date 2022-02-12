using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonMono<GameManager>
{
    enum GameState
    {
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
    private int _curStage;
    private int _curTurn;
    private int _saveCurHp;
    private int _saveMaxHp;
    private int _saveCurMp;
    private int _saveMaxMp;
    
    
    [SerializeField] private GameState _gameState = GameState.GameStart;
    private Vector3 _mousePos;
    private CardObject _cardObject;
    private int _curMonster;
    private Coroutine _stateRoutine;

    public Player player;
    public List<Monster> monsters = new List<Monster>();

    //외부
    [SerializeField] private float attackTime;
    [SerializeField] private float attackInterval;
    [SerializeField] private Image statePanel;
    [SerializeField] private MonsterHpBar monsterHpBar;
    [SerializeField] private GameObject canvas;

    private void Start()
    {
        GameObject.FindWithTag("Player").TryGetComponent(out player);

        foreach (var monster in monsters)
        {
            var monsterTransform = monster.transform;
            MonsterHpBar hpBar = Instantiate(monsterHpBar, monsterTransform.position + Vector3.up * 2,
                quaternion.identity, canvas.transform);
            monster.hpBar = hpBar;
        }
    }

    private void Update()
    {
        MouseInput();

        if (monsters.Count <= 0)
        {
            
        }

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

    private void OnGameStart()
    {
        ChangeState(GameState.PlayerTurnStart);
    }

    private void OnPlayerTurnStart()
    {
        foreach (var monster in monsters)
        {
            // monster.ShowAttackPos();
        }

        for (int i = 0; i < 4; i++)
        {
            CardManager.Instance.DrawCard();
        }

        player.curMp = player.maxMp;

        ChangeState(GameState.PlayerTurn);
    }
    
    private void OnPlayerTurn()
    {
    }
    
    private void OnPlayerTurnEnd()
    {
        foreach (var monster in monsters)
        {
            monster.hpBar.Lerp();
        }

        foreach (var card in CardManager.Instance.cards)
        {
            CardManager.Instance.DestroyCard(card);
        }
        
        ChangeState(GameState.EnemyTurnStart);
    }

    private void OnEnemyTurnStart()
    {
        ChangeState(GameState.EnemyTurn);
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

        foreach (var monster in monsters)
        {
            monster.Vulnerable -= 1;
            monster.Weakness -= 1;
        }

        player.stateBar.HpLerp();

        ChangeState(GameState.PlayerTurnStart);
    }

    private void OnGameEnd()
    {
        _saveCurHp = player.curHp;
        _saveMaxHp = player.maxHp;
        _saveCurMp = player.curMp;
        _saveMaxMp = player.maxMp;
    }

    private void ChangeState(GameState gameState)
    {
        if (_stateRoutine != null)
        {
            StopCoroutine(_stateRoutine);
            _gameState = gameState;
        }

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
            if (_cardObject.originCard.type == Card.CardType.One)
            {
                if (TryCastRay(out Monster monster))
                {
                    _cardObject.originCard.Effect(player, monster);
                    
                    CardManager.Instance.DestroyCard(_cardObject);
                    _cardObject = null;
                }
            }

            else
            {
                if (_cardObject.originCard.type == Card.CardType.All)
                {
                    _cardObject.originCard.Effect(player, monsters.ToArray());
                }

                else if (_cardObject.originCard.type == Card.CardType.None)
                {
                    _cardObject.originCard.Effect(player, null);
                }
                
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