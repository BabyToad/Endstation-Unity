using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameplayStateManager : MonoBehaviour
{
    public enum GameplayState
    {
        NarrativeEvent,
        FreePlay,
        Cutscene
    }
    [SerializeField]
    GameplayState _currentState;

   
    public GameplayState CurrentState { get => _currentState; set => _currentState = value; }

}
