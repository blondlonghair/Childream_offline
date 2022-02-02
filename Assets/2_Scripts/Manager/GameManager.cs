using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMono<GameManager>
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
    private GameState _gameState = GameState.PlayerTurnStart;

    public Player player;
    public List<Monster> monsters = new List<Monster>();

    //외부
    [SerializeField] private float attackTime;

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
        
    }
    
    private void OnEnemyTurn()
    {
        StartCoroutine(Co_AttackMonster());
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
}