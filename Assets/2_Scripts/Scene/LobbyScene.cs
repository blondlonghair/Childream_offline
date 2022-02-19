using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] private MatchingDoor matchingDoor;
    [SerializeField] private LoadingPanel loadingPanel;

    private void Start()
    {
        loadingPanel.gameObject.transform.position = Vector3.zero;
        loadingPanel.Open();
    }

    public void GameStartButton()
    {
        matchingDoor.CloseDoor(() => SceneManager.LoadScene("Ingame"));
    }

    public void ShopButton()
    {
        loadingPanel.Close(() => SceneManager.LoadScene("Shop"));
    }
}
