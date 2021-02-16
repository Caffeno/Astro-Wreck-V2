using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    private void Awake()
    {
        instance = this;
    }

    public event Action onPlayerDeathEnter;
    public void PlayerDeathTrigger()
    {
        if (onPlayerDeathEnter != null)
        {
            onPlayerDeathEnter();
        }
    }

    public event Action updateScore;
    public void UpdateScore()
    {
        if (updateScore != null)
        {
            updateScore();
        }
    }

    public event Action<int> waveStartTrigger;
    public void StartWave(int waveNumber)
    {
        if(waveStartTrigger != null)
        {
            waveStartTrigger(waveNumber);
        }
    }
}
