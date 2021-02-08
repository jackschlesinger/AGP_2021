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
        
        var playerGameObject = Instantiate(Resources.Load<GameObject>("Player"));
        Services.Players = new[] {new UserControlledPlayer(playerGameObject, 
            new [] {KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D })};

        Services.AIManager = new AIController();
        Services.AIManager.Initialize();
        
        Services.Input = new InputManager();
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
}
