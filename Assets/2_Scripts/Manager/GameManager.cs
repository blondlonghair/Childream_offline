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
        CardManager.Instance.DrawCard();
        CardManager.Instance.DrawCard();
        CardManager.Instance.DrawCard();

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

    private IEnumerator Co_AttackMonster()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            monsters[i].Attack();
            yield return new WaitForSeconds(attackTime);
        }

        _gameState = GameState.PlayerTurn;
    }

    public void TurnEndButton()
    {
        if (_gameState == GameState.PlayerTurn)
        {
            _gameState = GameState.EnemyTurnStart;
        }
    }
}