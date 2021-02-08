using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Goal")) return;
        
        var gameObjectName = other.gameObject.name;
        // Services.EventManager.Fire(new GoalScored(gameObjectName == "Blue Goal"));
    }
}
