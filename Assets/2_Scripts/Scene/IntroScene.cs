using System;
using Firebase.Analytics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    [SerializeField] private LoadingPanel loadingPanel;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // if (PlayerPrefs.HasKey("Tutorial"))
            // {
                loadingPanel.Close(() => SceneManager.LoadScene("Lobby"));
            // }
            //
            // else
            // {
            // loadingPanel.Close(() => SceneManager.LoadScene("Tutorial"));
            //     PlayerPrefs.SetInt("Tutorial", 1);
            // }   
        }
    }
}