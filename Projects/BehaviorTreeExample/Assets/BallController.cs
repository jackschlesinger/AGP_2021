using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallController : MonoBehaviour
{
    private float _speed = 0.1f;
    public bool hungry;

    private BehaviorTree.Tree<BallController> _tree;

    private void Start()
    {
        var mouseDownTree = new Tree<BallController>
        (
            new Sequence<BallController>
            (
                new IsMouseDown(),
                new Selector<BallController>
                (
                    new Sequence<BallController>
                    (
                        new IsHungry(),
                        new GoTowardsMouse(true)
                    ),
                    new GoTowardsMouse(false)
                )
                
            )
        );

        var spaceBarDown = new Tree<BallController>
        (
            new Sequence<BallController>
            (
                new IsKeyDown(KeyCode.Space),
                new BecomeHungry()
            )
        );
        
        _tree = new Tree<BallController>
        (
            new Selector<BallController>
            (
                mouseDownTree,
                spaceBarDown,
                new Shaking()
            )
        );
    }

    private void Update()
    {
        _tree.Update(this);
    }

    public void Shake()
    {
        var jitter = (_speed * Random.insideUnitSphere);
        jitter.z = 0;
        
        transform.position += jitter;
    }

    public void GoTowardsMouse(bool towards)
    {
        var vectorTowardsMouse = (transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)).normalized;
        vectorTowardsMouse.z = 0;
        
        if (towards) 
            transform.position -= _speed * vectorTowardsMouse;
        else
            transform.position += _speed * vectorTowardsMouse;

    }
    
}

public class IsMouseDown : BehaviorTree.Node<BallController>
{
    public override bool Update(BallController context)
    {
        return Input.GetMouseButton(0);
    }
}

public class IsKeyDown : BehaviorTree.Node<BallController>
{
    private KeyCode keyToCheck;

    public IsKeyDown(KeyCode keyCode)
    {
        keyToCheck = keyCode;
    }

    public override bool Update(BallController context)
    {
        return Input.GetKey(keyToCheck);
    }
}

public class IsHungry : BehaviorTree.Node<BallController>
{
    public override bool Update(BallController context)
    {
        return context.hungry;
    }
}

public class Shaking : BehaviorTree.Node<BallController>
{
    public override bool Update(BallController context)
    {
        context.Shake();
        
        return true;
    }
}

public class GoTowardsMouse : BehaviorTree.Node<BallController>
{
    private bool towards;

    public GoTowardsMouse(bool towards)
    {
        this.towards = towards;
    }
    
    public override bool Update(BallController context)
    {
        context.GoTowardsMouse(towards);

        return true;
    }
}

public class BecomeHungry : BehaviorTree.Node<BallController>
{
    public override bool Update(BallController context)
    {
        context.hungry = true;
        
        return true;
    }
}
