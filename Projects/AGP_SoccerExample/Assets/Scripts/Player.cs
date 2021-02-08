using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using Object = System.Object;

public abstract class Player
{
    #region Variables
    
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }

    private const float MovementSpeed = 3.0f;

    public bool playerTeam = false;

    protected GameObject _gameObject;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private Vector3 _startingPosition;
    public Vector3 position => _gameObject.transform.position;

    #endregion
    
    #region Lifecycle Management
    
    protected Player(GameObject gameObject)
    {
        _gameObject = gameObject;
        _spriteRenderer = _gameObject.GetComponent<SpriteRenderer>();
        _rigidbody2D = _gameObject.GetComponent<Rigidbody2D>();
    }

    public abstract void Update();

    public void Destroy()
    {
        UnityEngine.Object.Destroy(_gameObject);
    }
    
    #endregion
    
    #region Core Functionality

    public Player SetTeam(bool playerTeam)
    {
        this.playerTeam = playerTeam;
        _spriteRenderer.color = playerTeam ? Services.GameController.playerTeam : Services.GameController.opposingTeam;

        return this;
    }

    public Player SetPosition(float x, float y, bool isStartingPosition = false)
    {
        _gameObject.transform.position = new Vector3(x, y);

        if (isStartingPosition)
        {
            _startingPosition = new Vector3(x, y);
        }
        
        return this;
    }

    public void SetToStartingPosition()
    {
        _gameObject.transform.position = _startingPosition;
    }

    protected void MoveInDirection(params Direction[] directions)
    {
        var newPosition = _gameObject.transform.position;

        if (directions.Contains(Direction.Up))
        {
            newPosition += Time.deltaTime * MovementSpeed * Vector3.up;
        }
        if (directions.Contains(Direction.Down))
        {
            newPosition += Time.deltaTime * MovementSpeed * Vector3.down;
        }
        if (directions.Contains(Direction.Left))
        {
            newPosition += Time.deltaTime * MovementSpeed * Vector3.left;
        }
        if (directions.Contains(Direction.Right))
        {
            newPosition += Time.deltaTime * MovementSpeed * Vector3.right;
        }

        _rigidbody2D.MovePosition(newPosition);
    }

    protected Direction[] GetDirections(Vector2 toMoveTowards)
    {
        return GetDirections(toMoveTowards.x, toMoveTowards.y);
    }
    
    protected Direction[] GetDirections(GameObject toMoveTowards)
    {
        return GetDirections(toMoveTowards.transform.position.x, toMoveTowards.transform.position.y);
    }
    
    protected Direction[] GetDirections(float x, float y)
    {
        var ballDirection = new List<Direction>();

        ballDirection.Add(x < _gameObject.transform.position.x ? Direction.Left : Direction.Right);

        ballDirection.Add(y < _gameObject.transform.position.y ? Direction.Down : Direction.Up);

        return ballDirection.ToArray();
    }
    
    #endregion
}

public class AIPlayer : Player
{
    private FiniteStateMachine<AIPlayer> _fsm;
    
    #region Lifecycle Management
    public AIPlayer(GameObject gameObject) : base(gameObject)
    {
        _fsm = new FiniteStateMachine<AIPlayer>(this);
        _fsm.TransitionTo<Offense>();
    }

    public override void Update()
    {
        _fsm.Update();
        // MoveInDirection(_GetDirectionFromBallPosition());
    }

    #endregion

    #region Core Functionality
    
    private Direction[] _GetDirectionFromBallPosition()
    {
        return GetDirections(Services.GameController.ball);
    }

    #endregion

    #region States

    private abstract class AIPlayerState : FiniteStateMachine<AIPlayer>.State { }

    private class Offense : AIPlayerState
    {
        public override void OnEnter()
        {
            // change my expression to be angry
            // pick a defender
        }

        public override void Update()
        {
            base.Update();
            // try to foul that defender
            
            if (Services.GameController.ball.transform.position.x < 0 && Context.playerTeam ||
                Services.GameController.ball.transform.position.x > 0 && !Context.playerTeam)
            {
                TransitionTo<Defense>();
            }
        }

        public override void OnExit()
        {
            // get rid of angry face
        }
    }

    private class Defense : AIPlayerState
    {
        
    }

    private class NearBall : AIPlayerState
    {
        
    }

    #endregion
}

public class UserControlledPlayer : Player
{
    #region Variables
    
    private readonly KeyCode[] _directionalControls;
    private readonly InputManager.InputType inputType;
    
    #endregion

    #region Lifecycle Management

    public UserControlledPlayer(GameObject gameObject, KeyCode[] directionalControls) : base(gameObject)
    {
        _directionalControls = directionalControls;
        inputType = InputManager.InputType.Keyboard;
    }
    
    public UserControlledPlayer(GameObject gameObject) : base(gameObject)
    {
        inputType = InputManager.InputType.Mouse;
    }
    
    public UserControlledPlayer(GameObject gameObject, KeyCode[] directionalControls, InputManager.InputType inputType) : base(gameObject)
    {
        _directionalControls = directionalControls;
        this.inputType = inputType;
    }

    public override void Update()
    {
        switch (inputType)
        {
            case InputManager.InputType.Keyboard:
                MoveInDirection(_GetDirectionsFromKeyPresses());
                break;
            case InputManager.InputType.Mouse:
                MoveInDirection(_GetDirectionsFromMousePosition());
                break;
            case InputManager.InputType.Controller:
                Debug.Log("Controller input is not yet supported, use mouse or keyboard.");
                break;
            case InputManager.InputType.NotSet:
            default:
                Debug.Log("Input type is not set, use either keyboard or mouse.");
                break;
        }
    }

    #endregion
    
    #region Core Functionality

    private Direction[] _GetDirectionsFromMousePosition()
    {
        return GetDirections(Services.Input.MousePositionWorldUnits);
    }

    private Direction[] _GetDirectionsFromKeyPresses()
    {
        var keysPressed = new List<Direction>();
        
        foreach(Direction direction in Enum.GetValues(typeof(Direction)))
            if (Services.Input.KeysDown.Contains(_directionalControls[(int) direction]) ||
                Services.Input.KeysStay.Contains(_directionalControls[(int) direction]))
                keysPressed.Add(direction);

        return keysPressed.ToArray();
    }
    
    #endregion
}