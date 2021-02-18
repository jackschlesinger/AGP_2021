using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverObjects : MonoBehaviour
{
    public GameObject returnToTitleMessage;
    public TextMeshPro winnerMessage;

    public void SetWinnerMessage(string message)
    {
        winnerMessage.text = message;
    }
}
