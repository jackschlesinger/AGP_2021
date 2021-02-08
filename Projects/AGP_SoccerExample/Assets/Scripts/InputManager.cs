using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public List<KeyCode> KeysDown { get; private set; }
    public List<KeyCode> KeysUp { get; private set; }
    public List<KeyCode> KeysStay { get; private set; }

    public bool mouseDown  { get; private set; }
    public bool mouseUp  { get; private set; }
    public bool mouseStay { get; private set; }

    public Vector2 MousePositionWorldUnits { get; private set; }

    public enum InputType
    {
        NotSet = 0,
        Keyboard,
        Mouse,
        Controller,
    }

    public void Update()
    {
        mouseDown = false;
        mouseUp = false;
        mouseStay = false;
        
        if (Input.touchSupported)
        {
            for (var i = 0; i < Input.touchCount; i++)
            {
                var touch = Input.GetTouch(i);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        mouseDown = true;
                        break;
                    case TouchPhase.Moved:
                    case TouchPhase.Stationary:
                        mouseStay = true;
                        break;
                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        mouseUp = true;
                        break;
                }
            }
        } 
        else
        {
            if (Input.GetMouseButtonDown(0))
                mouseDown = true;
            if (Input.GetMouseButton(0))
                mouseStay = true;
            if (Input.GetMouseButtonUp(0))                        
                mouseUp = true;
            
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MousePositionWorldUnits = new Vector2(mousePosition.x, mousePosition.y);
        }
        
        KeysDown = new List<KeyCode>();
        KeysUp = new List<KeyCode>();
        KeysStay = new List<KeyCode>();
        
        foreach(KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keycode))
                KeysDown.Add(keycode);
            if (Input.GetKey(keycode))
                KeysStay.Add(keycode);
            if (Input.GetKeyUp(keycode))
                KeysStay.Add(keycode);
        }
    }
}