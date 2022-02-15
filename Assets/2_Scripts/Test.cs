using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : SingletonMono<Test>
{
    private int _curStageInt;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SceneManager.LoadScene($"Stage{_curStageInt + 1}");
        }

        if (SceneManager.GetActiveScene().name.Contains("Stage") && $"Stage{_curStageInt}" != SceneManager.GetActiveScene().name)
        {
            string[] scene = SceneManager.GetActiveScene().name.Split("Stage");
            _curStageInt = int.Parse(scene[1]);
            changeScene();
        }
    }

    void changeScene()
    {
        print(_curStageInt);
    }
}
