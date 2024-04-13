using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

public class Guild : MonoBehaviour
{
    [SerializeField]
    List<Explorer> _roster;
    ToggleGroup _rosterTG;
    //[SerializeField]
    Explorer _selectedExplorer;

    [SerializeField]
    float _cred;
    [SerializeField]
    float _rep;
    [SerializeField]
    float _upkeepPerExplorer = 1;

    [SerializeField]
    int _recruitCost = 3;

    [SerializeField]
    NarrativeEvent _bankruptNE;
    bool _isBankrupt;
    [SerializeField]
    NarrativeEvent _diceNE;
    bool _diceNEHasTriggerd;



    public delegate void NewCycle();
    public static event NewCycle OnNewCycle;


    int _downtimeActions = 2;

    public Explorer SelectedExplorer { get => _selectedExplorer; set => _selectedExplorer = value; }
    public float Cred { get => _cred; set => _cred = value; }
    public ToggleGroup RosterTG { get => _rosterTG; set => _rosterTG = value; }
    public List<Explorer> Roster { get => _roster; set => _roster = value; }
    public bool DiceNEHasTriggerd { get => _diceNEHasTriggerd; set => _diceNEHasTriggerd = value; }
    public NarrativeEvent DiceNE { get => _diceNE; set => _diceNE = value; }

    private void Awake()
    {
        RosterTG = GetComponent<ToggleGroup>();
    }

    private void Start()
    {
        if (_roster.Count < 2)
        {
            AddCred(6);
            RecruitExplorer(2, 1, 1);
            RecruitExplorer(1, 2, 1);
        }
        
    }

    private void OnEnable()
    {
        MasterSingleton.Instance.InputManager.InputActions.Debug.Restart.performed += Restart_performed;

    }

    private void OnDisable()
    {
        MasterSingleton.Instance.InputManager.InputActions.Debug.Restart.performed -= Restart_performed;
    }
    private void Restart_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Destroy(MasterSingleton.Instance.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void Update()
    {
        if (MasterSingleton.Instance.StateManager.CurrentState == GameplayStateManager.GameplayState.FreePlay && _isBankrupt)
        {
            TriggerBankrupt();
        }
    }
    public void RecruitExplorer(string name)
    {
        if (_cred >= 3)
        {
            Explorer explorer = new Explorer(name, 3, Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3), this);         
            _roster.Add(explorer);
            AddCred(-_recruitCost);
            Debug.Log("Recruited and Explorer. Spend " + _recruitCost + " Cred.");
        }
        else
        {
            Debug.LogWarning("Need at least " + _recruitCost + " Cred to recruit an Explorer.");
        }
    }


    public void RecruitExplorer(int insight, int prowess, int resolve)
    {
        if (_cred >= 3)
        {
            Explorer explorer = new Explorer(Resources.Load<ExplorerNameList>("NameList").GenerateName(), 3, insight, prowess, resolve, this);
            _roster.Add(explorer);
            AddCred(-_recruitCost);
            Debug.Log("Recruited and Explorer. Spend " + _recruitCost + " Cred.");
        }
        else
        {
            Debug.LogWarning("Need at least " + _recruitCost + " Cred to recruit an Explorer.");
        }
    }
    public void RecruitExplorer()
    {
        string name = Resources.Load<ExplorerNameList>("NameList").GenerateName();
        RecruitExplorer(name);
    }

    public void AdvanceInsight()
    {
        _selectedExplorer.AdvanceInsight();
    }
    
    public void EndCycleButton()
    {
        for (int i = 0; i < _roster.Count; i++)
        {
            _roster[i].Rest();
        }
        AddCred(-_upkeepPerExplorer * _roster.Count);
        MasterSingleton.Instance.UIManger.HighlightEndCycle(MasterSingleton.Instance.Guild.IsRosterExhausted());

        foreach (PointOfInterest poi in MasterSingleton.Instance.UIManger.PointsOfInterestList)
        {
            poi.DeSelect();
        }
       
        AudioManager.instance.PlayOneShot(FMODEvents.instance._uiClick);
        AudioManager.instance.PlayOneShot(FMODEvents.instance._endTurn);
        MasterSingleton.Instance.StateManager.CurrentState = GameplayStateManager.GameplayState.Cutscene;
        // tick clocks
    }

    public void StartNewCycle()
    {
        OnNewCycle();
        MasterSingleton.Instance.StateManager.CurrentState = GameplayStateManager.GameplayState.FreePlay;
    }

    public void AddCred(float credAmount)
    {
        _cred += credAmount;

        if (_cred < 0)
        {
            _isBankrupt  = true;
            _cred = 0;
        }

        MasterSingleton.Instance.UIManger.UpdateCredDisplay(_cred);
    }


    public static void AddEventCred(float credAmount)
    {
        MasterSingleton.Instance.Guild.Cred += credAmount;
        MasterSingleton.Instance.UIManger.UpdateCredDisplay(MasterSingleton.Instance.Guild.Cred);
    }
    public void AddRep(float repAmount)
    {
        _rep += repAmount;
        MasterSingleton.Instance.UIManger.UpdateReputationDisplay(_rep);
    }

    public bool IsRosterExhausted()
    {
        bool isExhausted = false;

        for (int i = 0; i < _roster.Count; i++)
        {
            if (_roster[i].Exhausted)
            {
                isExhausted = true;
            }
            else
            {
                isExhausted = false;
                return isExhausted;
            }
        }

        return isExhausted;
    }

    void TriggerBankrupt()
    {
        _bankruptNE.Trigger();
        _isBankrupt = false;
    }

    public void SelectAvailableExplorer()
    {
        foreach (Explorer explorer in Roster)
        {
            if (!explorer.Exhausted)
            {
                explorer.SelectExplorer(true);
                return;
            }
        }
    }
}
