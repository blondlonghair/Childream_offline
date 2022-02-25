using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyScene : MonoBehaviour
{
    [SerializeField] private MatchingDoor matchingDoor;
    [SerializeField] private LoadingPanel loadingPanel;
    [SerializeField] private SpriteRenderer backGround;
    [SerializeField] private Sprite[] backGroundSprite;

    private void Start()
    {
        backGround.sprite = PlayerPrefs.HasKey("ClearOnce") ? backGroundSprite[0] : backGroundSprite[1];

        loadingPanel.gameObject.transform.position = Vector3.zero;
        loadingPanel.Open();
    }

    public void GameStartButton()
    {
        matchingDoor.CloseDoor(() => SceneManager.LoadScene("Map"));
    }

    public void ShopButton()
    {
        loadingPanel.Close(() => SceneManager.LoadScene("Shop"));
    }
}
