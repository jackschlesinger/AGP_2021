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

    public GameObject titleScreen;
    public InGameObjects inGame;
    public GameOverObjects gameOver;

    public FiniteStateMachine<GameController> _fsm;
    
    private void Awake()
    {
        _InitializeServices();
    }

    private void _InitializeServices()
    {
        Services.GameController = this;
        _fsm = new FiniteStateMachine<GameController>(this);
        _fsm.TransitionTo<TitleScreen>();
        
        Services.EventManager = new EventManager();

        Services.Input = new InputManager();
    }

    private void Update()
    {
        Services.Input.Update();
        _fsm.Update();
    }

    private abstract class GameState : FiniteStateMachine<GameController>.State
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Context.titleScreen.SetActive(false);
            Context.inGame.gameObject.SetActive(false);
            Context.gameOver.gameObject.SetActive(false);
        }
    }

    private class TitleScreen : GameState
    {
        public override void OnEnter()
        {
            base.OnEnter();

            Context.titleScreen.SetActive(true);
        }

        public override void Update()
        {
            base.Update();
            
            if (Services.Input.KeyDownThisFrame)
                TransitionTo<InGame>();
        }
    }

    private class InGame : GameState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            
            Context.inGame.gameObject.SetActive(true);
            
            var playerGameObject = Instantiate(Resources.Load<GameObject>("Player"));
            Services.Players = new[] {new UserControlledPlayer(playerGameObject, new [] {KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D })};
        
            Services.AIManager = new AIController();
            Services.AIManager.Initialize();
            
            Services.Score = new ScoreController();

            Services.EventManager.Register<TimedOut>(GameTimedOut);
            Services.EventManager.Register<GoalScored>(OnGoalScored);
        }

        public override void Update()
        {
            base.Update();

            foreach (var player in Services.Players) player.Update();

            Services.AIManager.Update();
            Services.Score.Update();
        }

        public override void OnExit()
        {
            base.OnExit();
            
            Services.EventManager.Unregister<TimedOut>(GameTimedOut);
            Services.EventManager.Unregister<GoalScored>(OnGoalScored);

            Services.Score.Destroy();
        }

        private void GameTimedOut(AGPEvent e)
        {
            var timedOut = (TimedOut) e;
            
            Context.gameOver.SetWinnerMessage(timedOut.blueScore > timedOut.redScore ? "Blue won!" : "Red won!");
            
            TransitionTo<GameOver>();
        }

        public void OnGoalScored(AGPEvent e)
        {
            Context.ball.transform.position = Vector3.zero;
        }
    }

    private class GameOver : GameState
    {
        private const float timeBeforeAllowReturnToTitle = 1.0f;
        private float timeInGameOver = 0;
        
        public override void OnEnter()
        {
            base.OnEnter();
            
            Context.gameOver.gameObject.SetActive(true);
            timeInGameOver = 0;
            Context.gameOver.returnToTitleMessage.SetActive(false);
            
            Services.AIManager.Destroy();
        }

        public override void Update()
        {
            base.Update();

            timeInGameOver += Time.deltaTime;

            if (timeInGameOver < timeBeforeAllowReturnToTitle) return;

            Context.gameOver.returnToTitleMessage.SetActive(true);
            
            if (Services.Input.KeysDown.Contains(KeyCode.Space)) 
                TransitionTo<TitleScreen>();
        }
    }
}
