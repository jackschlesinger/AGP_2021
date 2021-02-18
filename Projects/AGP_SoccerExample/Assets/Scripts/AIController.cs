using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AIController
{
    private List<Player> _players;

    #region Lifecycle Management
    
    public void Initialize()
    {
        _players = new List<Player>();
        _CreateAIPlayers();
        
        Services.EventManager.Register<GoalScored>(OnGoalScored);
    }

    public Player GetClosestAI(Ball ball)
    {
        if (_players.Count == 0) return null;

        var closest = _players[0];
        var distance = float.MaxValue;

        foreach (var player in _players)
        {
            var distanceToPlayer = Vector3.Distance(ball.transform.position, player.position);
            if (distanceToPlayer < distance)
            {
                closest = player;
                distance = distanceToPlayer;
            }
        }

        return closest;
    }

    public void Update()
    {
        foreach (var player in _players)
        {
            player.Update();
        }
    }

    public void Destroy()
    {
        Services.EventManager.Unregister<GoalScored>(OnGoalScored);
        
        foreach (var player in _players)
        {
            player.Destroy();
        }
    }

    #endregion

    private void _CreateAIPlayers()
    {
        // Make blue players
        for (var i = Services.Players.Count(player => player.playerTeam);
            i < GameController.PlayersPerTeam;
            i++)
        {
            var playerGameObject = Object.Instantiate(Resources.Load<GameObject>("Player"));
            _players.Add(new AIPlayer(playerGameObject).SetTeam(true).SetPosition(Random.Range(0.0f, 8.0f), Random.Range(-4.0f, 4.0f), true));
        }
        
        // Make red players
        for (var i = Services.Players.Count(player => !player.playerTeam);
            i < GameController.PlayersPerTeam;
            i++)
        {
            var playerGameObject = Object.Instantiate(Resources.Load<GameObject>("Player"));
            _players.Add(new AIPlayer(playerGameObject).SetTeam(false).SetPosition(Random.Range(0.0f, -8.0f), Random.Range(-4.0f, 4.0f), true));
        }
    }

    private void OnGoalScored(AGPEvent e)
    {
        foreach (var player in _players) 
            player.SetToStartingPosition();
    }
}
