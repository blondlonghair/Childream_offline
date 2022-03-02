using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapScene : MonoBehaviour
{
    [SerializeField] private MatchingDoor matchingDoor;
    
    private void Start()
    {
        if (InGameManager.Instance.CurStage == 0)
        {
            matchingDoor.OpenDoor(null);
        }
    }

    public void LoadScene()
    {
        InGameManager.Instance.LoadScene("Ingame");
    }
}