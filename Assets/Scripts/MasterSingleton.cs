using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterSingleton : MonoBehaviour
{
    public static MasterSingleton Instance { get; private set; }
    public PhaseManager PhaseManager { get => _phaseManager; set => _phaseManager = value; }
    public Guild Guild { get => _guild; set => _guild = value; }
    public Dungeon Dungeon { get => _dungeon; set => _dungeon = value; }
    public UIManager UIManger { get => _uIManger; set => _uIManger = value; }
    public InputManager InputManager { get => _inputManager; set => _inputManager = value; }

    [SerializeField]
    PhaseManager _phaseManager;
    [SerializeField]
    UIManager _uIManger;
    [SerializeField]
    InputManager _inputManager;
    [SerializeField]
    Guild _guild;
    [SerializeField]
    Dungeon _dungeon;



    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

}
