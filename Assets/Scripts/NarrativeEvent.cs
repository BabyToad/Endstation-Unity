using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Narrative Event", menuName = "ScriptableObjects/Narrative Event", order = 1)]
public class NarrativeEvent : ScriptableObject
{
    EventCanvas _eventCanvas;

    int _id;
    [SerializeField]

    string _bodyText;
    [SerializeField]
    TextAsset _text;

    [Header("First Choice")]
    [SerializeField]
    string _upperButtonText;
    [SerializeField]
    float _uCred;
    [SerializeField]
    int _uStress;
    [SerializeField]
    int _uHp;
    [SerializeField]
    bool _uAllExplorers = false;
    [SerializeField]
    string _uUnlockPoI, _uLockPoI;
    UnityAction _upperAction;

    [Header("Second Choice")]
    [SerializeField]
    string _lowerButtonText;
    [SerializeField]
    float _lCred;
    [SerializeField]
    int _lStress;
    [SerializeField]
    int _lHp;
    [SerializeField]
    bool _lAllExplorers = false;
    UnityAction _lowerAction;

    [SerializeField]
    string _name;
    [SerializeField]
    Sprite _sprite;

    [SerializeField]
    bool _overrideAction;
    [SerializeField]
    string _pointOfInterest;
    [SerializeField]
    PointOfInterest.Action _newAction;

    private void OnEnable()
    {
        _upperAction += UpperEventMechanics;
        _lowerAction += LowerEventMechanics;
        if (_text != null)
        {
            _bodyText = _text.text;

        }
    }

    private void OnDisable()
    {
        _upperAction -= UpperEventMechanics;
        _lowerAction -= LowerEventMechanics;


    }
    void UpperEventMechanics()
    {
        Debug.Log(name + ": UpperEvent");

        MasterSingleton.Instance.Guild.AddCred(_uCred);
        if (_uAllExplorers)
        {
            for (int i = 0; i < MasterSingleton.Instance.Guild.Roster.Count; i++)
            {
                MasterSingleton.Instance.Guild.Roster[i].AddStress(_uStress);
                MasterSingleton.Instance.Guild.Roster[i].AddHealth(_uHp);
            }
        }
        else
        {
            MasterSingleton.Instance.Guild.SelectedExplorer.AddStress(_uStress);
            MasterSingleton.Instance.Guild.SelectedExplorer.AddHealth(_uHp);
        }
        if (_uUnlockPoI == "reload")
        {
            Destroy(MasterSingleton.Instance.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (_uUnlockPoI != "")
        {
            GameObject.Find(_uUnlockPoI).GetComponent<PointOfInterest>().SetActive(true);
        }

        if (_uLockPoI != "")
        {
            GameObject.Find(_uLockPoI).GetComponent<PointOfInterest>().SetActive(false);
        }

        if (_overrideAction)
        {
            GameObject.Find(_uUnlockPoI).GetComponent<PointOfInterest>().OverideAction(_newAction);
        }

        MasterSingleton.Instance.EventCanvas.ShowEventCanvas(false);
        MasterSingleton.Instance.UIManger.DisplayExplorerCanvas(true);
        MasterSingleton.Instance.UIManger.DisplayOverworldUI(true);
        MasterSingleton.Instance.UIManger.DisplayPointOfInterestSelectedUI(true);
        if (MasterSingleton.Instance.Guild.IsRosterExhausted())
        {
            foreach (PointOfInterest poi in MasterSingleton.Instance.UIManger.PointsOfInterestList)
            {
                poi.DeSelect();
            }
        }
        MasterSingleton.Instance.StateManager.CurrentState = GameplayStateManager.GameplayState.FreePlay;

    }

    void LowerEventMechanics()
    {
        Debug.Log(name + ": LowerEvent" );
        MasterSingleton.Instance.Guild.AddCred(_lCred);
        if (_lAllExplorers)
        {
            for (int i = 0; i < MasterSingleton.Instance.Guild.Roster.Count; i++)
            {
                MasterSingleton.Instance.Guild.Roster[i].AddStress(_lStress);
                MasterSingleton.Instance.Guild.Roster[i].AddHealth(_lHp);
            }
        }
        else
        {
            MasterSingleton.Instance.Guild.SelectedExplorer.AddStress(_lStress);
            MasterSingleton.Instance.Guild.SelectedExplorer.AddHealth(_lHp);
        }

        if (_overrideAction)
        {
            GameObject.Find(_uUnlockPoI).GetComponent<PointOfInterest>().OverideAction(_newAction);
        }

        MasterSingleton.Instance.EventCanvas.ShowEventCanvas(false);
        MasterSingleton.Instance.UIManger.DisplayExplorerCanvas(true);
        MasterSingleton.Instance.UIManger.DisplayOverworldUI(true);
        MasterSingleton.Instance.UIManger.DisplayPointOfInterestSelectedUI(true);
        if (MasterSingleton.Instance.Guild.IsRosterExhausted())
        {
            foreach (PointOfInterest poi in MasterSingleton.Instance.UIManger.PointsOfInterestList)
            {
                poi.DeSelect();
            }
        }
        MasterSingleton.Instance.StateManager.CurrentState = GameplayStateManager.GameplayState.FreePlay;

    }

    public void Trigger()
    {
        Debug.Log("Triggered "  + name);
        if(name.Contains("NE_Intro"))
        {
            Debug.Log("play tseya");
            AudioManager.instance.PlayOneShot(FMODEvents.instance._endstationIntro);
        }
        else if (name.Contains("NE_Creditorium"))
        {
            Debug.Log("play creditorium");
            AudioManager.instance.PlayOneShot(FMODEvents.instance._creditoriumIntro);
        }
        else if (name.Contains("NE_Oasis"))
        {
            Debug.Log("play oasis");
            AudioManager.instance.PlayOneShot(FMODEvents.instance._oasisIntro);
        }
        else if (name.Contains("NE_OldGods"))
        {
            Debug.Log("play old gods");
            AudioManager.instance.PlayOneShot(FMODEvents.instance._templeIntro);
        }
        else if (name.Contains("NE_Roots"))
        {
            Debug.Log("play roots");
            AudioManager.instance.PlayOneShot(FMODEvents.instance._rootsIntro);
        }


        MasterSingleton.Instance.StateManager.CurrentState = GameplayStateManager.GameplayState.NarrativeEvent;
        MasterSingleton.Instance.EventCanvas.SetEventName(_name);
        MasterSingleton.Instance.EventCanvas.SetBodyText(_bodyText);
        MasterSingleton.Instance.EventCanvas.SetEventImage(_sprite);
        
        MasterSingleton.Instance.EventCanvas.SetUpperButtonText(_upperButtonText);
        MasterSingleton.Instance.EventCanvas.AddUpperButtonAction(_upperAction);

        if (_lowerButtonText != "")
        {
            MasterSingleton.Instance.EventCanvas.ShowLowerButton(true);
            MasterSingleton.Instance.EventCanvas.SetLowerButtonText(_lowerButtonText);
            MasterSingleton.Instance.EventCanvas.AddLowerButtonAction(_lowerAction);
        }
        else
        {
            MasterSingleton.Instance.EventCanvas.ShowLowerButton(false);
        }
        MasterSingleton.Instance.UIManger.DisplayPointOfInterestSelectedUI(false);
        MasterSingleton.Instance.UIManger.DisplayExplorerCanvas(false);
        MasterSingleton.Instance.UIManger.DisplayOverworldUI(false);
        MasterSingleton.Instance.EventCanvas.ShowEventCanvas(true);
    }
}
