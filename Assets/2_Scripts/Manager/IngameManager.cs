using System;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    enum GameState
    {
        PlayerTurnStart,
        PlayerTurn,
        EnemyTurnStart,
        EnemyTurn,
        GameEnd
    }

    private int _curStage;
    private int _curTurn;

    private GameState _gameState = GameState.PlayerTurnStart;
    
    private void Update()
    {
        switch (_gameState)
        {
            case GameState.PlayerTurnStart: OnPlayerTurnStart(); break;
            case GameState.PlayerTurn: OnPlayerTurn(); break;
            case GameState.EnemyTurnStart: OnEnemyTurnStart(); break;
            case GameState.EnemyTurn: OnEnemyTurn(); break;
            case GameState.GameEnd: OnGameEnd(); break;
        }
    }

    private void OnPlayerTurnStart()
    {
        CardManager.Instance.DrowCard();
        CardManager.Instance.DrowCard();
        CardManager.Instance.DrowCard();

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
        
    }

    private void OnGameEnd()
    {
        
    }
}