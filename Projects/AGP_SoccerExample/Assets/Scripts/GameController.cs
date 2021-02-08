using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour
{
    public GameObject ball;
    public Color playerTeam;
    public Color opposingTeam;
    public const int PlayersPerTeam = 2;
    
    private void Awake()
    {
        _InitializeServices();
    }

    private void _InitializeServices()
    {
        Services.GameController = this;
        
        Services.EventManager = new EventManager();
        
        Services.EventManager.Register<GoalScored>(OnGoalScored);

        var playerGameObject = Instantiate(Resources.Load<GameObject>("Player"));
        // Services.Players = new[] {new UserControlledPlayer(playerGameObject, new [] {KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D })};
        Services.Players = new[] {new UserControlledPlayer(playerGameObject)};

        /*
        // The above is the same as doing this:
        Services.Players = new UserControlledPlayer[1];
        Services.Players[0] = new UserControlledPlayer(playerGameObject, new []{KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D  });
        */
        
        Services.AIManager = new AIController();
        Services.AIManager.Initialize();
        
        Services.Input = new InputManager();
    }

    public void OnGoalScored(AGPEvent e)
    {
        var goalScoredWasBlue = ((GoalScored) e).blueTeamScored;
        
        Debug.Log("Goal was blue: " + goalScoredWasBlue);
    }

    private void Update()
    {
        Services.Input.Update();
        Services.Players[0].Update();
        Services.AIManager.Update();
    }

    private void OnDestroy()
    {
        Services.AIManager.Destroy();
    }

    public class GameStarted : AGPEvent { }

    public class GameTimedOut : AGPEvent
    {
        public readonly int blueScore, redScore;

        public GameTimedOut(int blueScore, int redScore)
        {
            this.blueScore = blueScore;
            this.redScore = redScore;
        }

    }
}
