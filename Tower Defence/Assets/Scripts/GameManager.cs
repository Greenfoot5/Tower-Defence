﻿using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isGameOver;

    public GameObject gameOverUI;

    void Start()
    {
        isGameOver = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            return;
        }
        
        if (GameStats.lives <= 0)
        {
            EndGame();
        }
    }
    
    // Called when we reach 0 lives
    private void EndGame()
    {
        isGameOver = true;
        
        gameOverUI.SetActive(true);
    }
}
