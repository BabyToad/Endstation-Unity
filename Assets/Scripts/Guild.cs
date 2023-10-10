using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Guild : MonoBehaviour
{
    [SerializeField]
    List<Explorer> _roster;
    ToggleGroup _rosterTG;
    [SerializeField]
    Explorer _selectedExplorer;

    [SerializeField]
    float _cred;
    [SerializeField]
    float _rep;
    [SerializeField]
    float _upkeep = 2;

    [SerializeField]
    int _recruitCost = 3;


    int _downtimeActions = 2;

    public Explorer SelectedExplorer { get => _selectedExplorer; set => _selectedExplorer = value; }
    public float Cred { get => _cred; set => _cred = value; }
    public ToggleGroup RosterTG { get => _rosterTG; set => _rosterTG = value; }
    private void Awake()
    {
        RosterTG = GetComponent<ToggleGroup>();

    }

    private void Start()
    {

        AddCred(9);

        RecruitExplorer();
        RecruitExplorer();
        RecruitExplorer();
    }
    public void RecruitExplorer(string name)
    {
        if (_cred >= 3)
        {
            Explorer explorer = new Explorer(name, 3, Random.Range(0, 3), Random.Range(0, 3), Random.Range(0, 3), this);
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

    public void ReturnFromExpedition()
    {
        _downtimeActions += 2;
        MasterSingleton.Instance.UIManger.DowntimeUI(true);
    }

    public void Recover()
    {
        if (_downtimeActions > 0)
        {
            foreach (Explorer exp in _roster)
            {
                exp.AddHealth(1);
            }
            _downtimeActions--;
            Debug.Log("Explorers recovered 1 HP. Used 1 Downtime Action.");
        }
        else
        {
            Debug.LogWarning("No Downtime Actions left.");
        }
    }

    public void IndulgeVice()
    {
        if (_downtimeActions > 0)
        {
            foreach (Explorer exp in _roster)
            {
                exp.AddStress(-1 * exp.RollDice(exp.GetLowestAttributeStat()));
            }
            _downtimeActions--;
            Debug.Log("Indulged Vice. Used 1 Downtime Action.");
        }
        else
        {
            Debug.LogWarning("No Downtime Actions left.");

        }
    }

    public void EndCycle()
    {
        for (int i = 0; i < _roster.Count; i++)
        {
            _roster[i].Rest();
        }
        AddCred(-_upkeep);
        // tick clocks
    }

    public void AddCred(float credAmount)
    {
        _cred += credAmount;
        MasterSingleton.Instance.UIManger.UpdateCredDisplay(_cred);
    }

    public void AddRep(float repAmount)
    {
        _rep += repAmount;
        MasterSingleton.Instance.UIManger.UpdateReputationDisplay(_rep);
    }
}
