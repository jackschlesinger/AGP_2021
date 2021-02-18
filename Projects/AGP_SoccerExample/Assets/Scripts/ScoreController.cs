using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreController
{
    private int redScore, blueScore;

    public float timer = 0;
    public const float TimeForGame = 30.0f;

    public ScoreController()
    {
        Services.EventManager.Register<GoalScored>(IncrementTeamScore);
        Services.EventManager.Register<GameStarted>(OnGameStart);
    }

    public void Destroy()
    {
        Services.EventManager.Unregister<GoalScored>(IncrementTeamScore);
        Services.EventManager.Unregister<GameStarted>(OnGameStart);
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if (timer >= TimeForGame)
        {
            Services.EventManager.Fire(new TimedOut(blueScore, redScore));
        }

        Services.GameController.inGame.redScore.text = "Red: " + redScore;
        Services.GameController.inGame.blueScore.text = "Blue: " + blueScore;
        Services.GameController.inGame.timer.text = ((int) (TimeForGame - timer)).ToString();
    }

    private void OnGameStart(AGPEvent e)
    {
        timer = 0;
        blueScore = 0;
        redScore = 0;
    }

    private void IncrementTeamScore(AGPEvent e)
    {
        var goalScoredEvent = (GoalScored) e;
        
        if (goalScoredEvent.blueTeamScored)
        {
            blueScore++;
        }
        else
        {
            redScore++;
        }
            
    }
}
