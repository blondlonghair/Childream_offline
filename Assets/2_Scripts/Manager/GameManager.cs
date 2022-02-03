using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;

public class GameManager : SingletonMonoDestroy<GameManager>
{
    enum GameState
    {
        GameStart,
        PlayerTurnStart,
        PlayerTurn,
        EnemyTurnStart,
        EnemyTurn,
        GameEnd
    }

    //내부
    private int _curStage;
    private int _curTurn;
    private GameState _gameState = GameState.GameStart;
    private Vector3 _mousePos;
    private CardObject _cardObject;

    public Player player;
    public List<Monster> monsters = new List<Monster>();

    //외부
    [SerializeField] private float attackTime;
    [SerializeField] private float attackInterval;

    private void Start()
    {
        GameObject.FindWithTag("Player").TryGetComponent(out player);
    }

    private void Update()
    {
        MouseInput();
        _mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePos.z = 0;

        switch (_gameState)
        {
            case GameState.GameStart: OnGameStart(); break;
            case GameState.PlayerTurnStart: OnPlayerTurnStart(); break;
            case GameState.PlayerTurn: OnPlayerTurn(); break;
            case GameState.EnemyTurnStart: OnEnemyTurnStart(); break;
            case GameState.EnemyTurn: OnEnemyTurn(); break;
            case GameState.GameEnd: OnGameEnd(); break;
        }
    }

    private void OnGameStart()
    {
        _gameState = GameState.PlayerTurnStart;
    }

    private void OnPlayerTurnStart()
    {
        foreach (var monster in monsters)
        {
            // monster.ShowAttackPos();
        }

        for (int i = 0; i < 3; i++)
        {
            CardManager.Instance.DrawCard();
        }

        player.curMp = player.maxMp;

        _gameState = GameState.PlayerTurn;
    }
    
    private void OnPlayerTurn()
    {
    }

    private void OnEnemyTurnStart()
    {
        _gameState = GameState.EnemyTurn;
    }

    private int t;
    
    private void OnEnemyTurn()
    {
        attackTime += Time.deltaTime;
        
        if (attackTime > attackInterval)
        {
            if (t < monsters.Count)
            {
                monsters[t].Attack();
                t++;
                attackTime = 0;
            }
            
            else
            {
                _gameState = GameState.PlayerTurnStart;
                t = 0;
            }
        }
    }

    private void OnGameEnd()
    {
    }

    private void MouseInput()
    {
        if (Input.GetMouseButton(0))
        {
            if (TryCastRay(out CardObject cardObj))
            {
                cardObj.CardZoomOut();
                cardObj.transform.position = _mousePos;
            }
        }

        else
        {
            if (TryCastRay(out CardObject cardObj))
            {
                if (cardObj != _cardObject && _cardObject != null)
                {
                    _cardObject.CardZoomOut();
                }

                _cardObject = cardObj;
                _cardObject.CardZoomIn();
            }

            else if (_cardObject != null)
            {
                _cardObject.CardZoomOut();
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (TryCastRay(out Monster monster))
            {
                _cardObject.originCard.Effect(player, monster);
                Destroy(_cardObject.gameObject);
            }
        }
    }

    public void TurnEndButton()
    {
        if (_gameState == GameState.PlayerTurn)
        {
            _gameState = GameState.EnemyTurnStart;
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