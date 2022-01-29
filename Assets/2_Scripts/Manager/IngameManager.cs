using System;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    enum GameState
    {
        PlayerTurn,
        EnemyTurn,
        GameEnd
    }

    private int _curStage;
    private int _curTurn;

    private GameState _gameState;
    
    private void Start()
    {
    }

    private void Update()
    {
        switch (_gameState)
        {
            case GameState.PlayerTurn: OnPlayerTurn(); break;
            case GameState.EnemyTurn: OnEnemyTurn(); break;
            case GameState.GameEnd: OnGameEnd(); break;
        }
    }

    private void OnPlayerTurn()
    {
        
    }

    private void OnEnemyTurn()
    {
        
    }

    private void OnGameEnd()
    {
        
    }
}