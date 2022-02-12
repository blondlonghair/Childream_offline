using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour
{
    [SerializeField] private LoadingPanel loadingPanel;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (PlayerPrefs.HasKey("Tutorial"))
            {
                loadingPanel.Close("Lobby");
            }
 
            else
            {
                loadingPanel.Close("Tutorial");
                PlayerPrefs.SetInt("Tutorial", 1);
            }    
        }
    }
}