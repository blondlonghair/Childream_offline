using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] private MatchingDoor matchingDoor;
    
    public void GameStartButton()
    {
        matchingDoor.CloseDoor(() => SceneManager.LoadScene("Ingame"));
    }
}
