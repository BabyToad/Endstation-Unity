using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    InputActions _inputActions;

    public InputActions InputActions { get => _inputActions; set => _inputActions = value; }

    private void Awake()
    {
        InputActions = new InputActions();
    }
    private void OnEnable()
    {
        InputActions.Enable();

    }

   

    private void OnDisable()
    {
        InputActions.Disable();
    }
}
