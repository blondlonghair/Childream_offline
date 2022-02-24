using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapGenerator : SingletonMonoDestroy<MapGenerator>
{
    //7*15
    private const int Width = 7;
    private const int Height = 15;

    private int[,] _node = new int[7, 15];
    
    private void Start()
    {
        Generate();
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "Map")
        {
            
        }
    }

    public void Generate()
    {
        
    }

    public void ShowMap()
    {
        
    }
    
    private void CreatNode()
    {
        Generate();
    }

    private void CreatLine()
    {
        
    }
}
