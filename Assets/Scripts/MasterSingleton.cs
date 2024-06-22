using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSingleton : MonoBehaviour
{
    private static MasterSingleton _instance;
    private static readonly object _lock = new object();
    private static bool _applicationIsQuitting = false;

    public static MasterSingleton Instance
    {
        get
        {
            if (_applicationIsQuitting)
            {
                Debug.LogWarning("[MasterSingleton] Instance already destroyed. Returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<MasterSingleton>();
                    if (_instance == null)
                    {
                        GameObject singletonObject = new GameObject();
                        _instance = singletonObject.AddComponent<MasterSingleton>();
                        singletonObject.name = typeof(MasterSingleton).ToString() + " (Singleton)";

                        DontDestroyOnLoad(singletonObject);
                    }
                }
                return _instance;
            }
        }
    }

    public GameplayStateManager StateManager { get => _stateManager; set => _stateManager = value; }
    public Guild Guild { get => _guild; set => _guild = value; }
    public UIManager UIManager { get => _uiManager; set => _uiManager = value; }
    public InputManager InputManager { get => _inputManager; set => _inputManager = value; }
    public EventCanvas EventCanvas { get => _eventCanvas; set => _eventCanvas = value; }
    public PointsOfInterestManager PointsOfInterestManager { get => _pointsOfInterestManager; set => _pointsOfInterestManager = value; }

    [SerializeField]
    private GameplayStateManager _stateManager;
    [SerializeField]
    private UIManager _uiManager;
    [SerializeField]
    private EventCanvas _eventCanvas;
    [SerializeField]
    private InputManager _inputManager;
    [SerializeField]
    private Guild _guild;
    [SerializeField]
    private PointsOfInterestManager _pointsOfInterestManager;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        Debug.Log(name + " Awake");

    }

    private void OnApplicationQuit()
    {
        _applicationIsQuitting = true;
    }


    
}
