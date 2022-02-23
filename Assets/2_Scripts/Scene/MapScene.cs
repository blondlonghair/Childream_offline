using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapScene : MonoBehaviour
{
    [SerializeField] private MatchingDoor matchingDoor;
    
    private void Start()
    {
        if (GameManager.Instance.CurStage == 0)
        {
            matchingDoor.OpenDoor(null);
        }
    }

    public void LoadScene()
    {
        GameManager.Instance.LoadScene("Ingame");
    }
}