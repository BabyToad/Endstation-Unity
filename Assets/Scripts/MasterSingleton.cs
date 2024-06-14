using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSingleton : MonoBehaviour
{
    public static MasterSingleton Instance { get; private set; }
    public GameplayStateManager StateManager { get => _stateManager; set => _stateManager = value; }
    public Guild Guild { get => _guild; set => _guild = value; }
    public UIManager UIManger { get => _uIManger; set => _uIManger = value; }
    public InputManager InputManager { get => _inputManager; set => _inputManager = value; }
    public EventCanvas EventCanvas { get => _eventCanvas; set => _eventCanvas = value; }
    public PointsOfInterestManager PointsOfInterestManager { get => _pointsOfInterestManager; set => _pointsOfInterestManager = value; }

    [SerializeField]
    GameplayStateManager _stateManager;
    [SerializeField]
    UIManager _uIManger;
    [SerializeField]
    EventCanvas _eventCanvas;
    [SerializeField]
    InputManager _inputManager;
    [SerializeField]
    Guild _guild;
    [SerializeField]
    PointsOfInterestManager _pointsOfInterestManager;



    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }

}
