﻿using System.Collections;
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
    ChoiceModifiers upperChoice = new ChoiceModifiers();

    [Header("Second Choice")]
    [SerializeField]
    ChoiceModifiers lowerChoice = new ChoiceModifiers();

    [SerializeField]
    string _name;
    [SerializeField]
    Sprite _sprite;

    private void OnEnable()
    {
        upperChoice.action += UpperEventMechanics;
        lowerChoice.action += LowerEventMechanics;
        if (_text != null)
        {
            _bodyText = _text.text;
        }
    }

    private void OnDisable()
    {
        upperChoice.action -= UpperEventMechanics;
        lowerChoice.action -= LowerEventMechanics;
    }

    void UpperEventMechanics()
    {
        ApplyChoiceModifiers(upperChoice);
    }

    void LowerEventMechanics()
    {
        ApplyChoiceModifiers(lowerChoice);
    }

    void ApplyChoiceModifiers(ChoiceModifiers choice)
    {
        Debug.Log(name + ": " + (choice == upperChoice ? "UpperEvent" : "LowerEvent"));

        MasterSingleton.Instance.Guild.AddCred(choice.cred);
        MasterSingleton.Instance.Guild.AddScrap(choice.scrap);
        MasterSingleton.Instance.Guild.AddArtifact(choice.artefacts);

        if (choice.allExplorers)
        {
            for (int i = 0; i < MasterSingleton.Instance.Guild.Roster.Count; i++)
            {
                MasterSingleton.Instance.Guild.Roster[i].AddStress(choice.stress);
                MasterSingleton.Instance.Guild.Roster[i].AddHealth(choice.hp);
            }
        }
        else
        {
            foreach (Explorer explorer in MasterSingleton.Instance.Guild.SelectedExplorers)
            {
                explorer.AddStress(choice.stress);
                explorer.AddHealth(choice.hp);
            }
        }

        if (choice.unlockPoI == "reload")
        {
            Destroy(MasterSingleton.Instance.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (choice.unlockPoI != "")
        {
            GameObject.Find(choice.unlockPoI).GetComponent<PointOfInterest>().SetActive(true);
        }

        if (choice.lockPoI != "")
        {
            PointOfInterest poi = GameObject.Find(choice.lockPoI).GetComponent<PointOfInterest>();
            poi.DeSelect();
            poi.SetActive(false);
        }

        if (choice.setDetails)
        {
            GameObject.Find(choice.POI).GetComponent<PointOfInterest>().SetGameDetailsActive(choice.details);
        }

        if (choice.overrideAction)
        {
            GameObject.Find(choice.pointOfInterest).GetComponent<PointOfInterest>().OverideAction(choice.newAction, choice.actionIndex);
        }

        if (choice.enableAction)
        {
            GameObject.Find(choice.pointOfInterest).GetComponent<PointOfInterest>().EnableAction(choice.actionIndex, choice.enableAction);
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
        Debug.Log("Triggered " + _name);
        if (_name.Contains("NE_Intro"))
        {
            Debug.Log("play tseya");
            AudioManager.instance.PlayOneShot(FMODEvents.instance._endstationIntro);
        }
        else if (_name.Contains("NE_Creditorium"))
        {
            Debug.Log("play creditorium");
            AudioManager.instance.PlayOneShot(FMODEvents.instance._creditoriumIntro);
        }
        else if (_name.Contains("NE_Oasis"))
        {
            Debug.Log("play oasis");
            AudioManager.instance.PlayOneShot(FMODEvents.instance._oasisIntro);
        }
        else if (_name.Contains("NE_OldGods"))
        {
            Debug.Log("play old gods");
            AudioManager.instance.PlayOneShot(FMODEvents.instance._templeIntro);
        }
        else if (_name.Contains("NE_Roots"))
        {
            Debug.Log("play roots");
            AudioManager.instance.PlayOneShot(FMODEvents.instance._rootsIntro);
        }

        MasterSingleton.Instance.StateManager.CurrentState = GameplayStateManager.GameplayState.NarrativeEvent;
        MasterSingleton.Instance.EventCanvas.SetEventName(_name);
        MasterSingleton.Instance.EventCanvas.SetBodyText(_bodyText);
        MasterSingleton.Instance.EventCanvas.SetEventImage(_sprite);

        string hoverInfo = EventEffectsToStringDescription(upperChoice.cred, upperChoice.scrap, upperChoice.artefacts, upperChoice.stress, upperChoice.hp);
        MasterSingleton.Instance.EventCanvas.SetUpperButtonText(upperChoice.buttonText, hoverInfo);
        MasterSingleton.Instance.EventCanvas.AddUpperButtonAction(upperChoice.action);

        if (lowerChoice.buttonText != "")
        {
            string lhoverInfo = EventEffectsToStringDescription(lowerChoice.cred, lowerChoice.scrap, lowerChoice.artefacts, lowerChoice.stress, lowerChoice.hp);
            MasterSingleton.Instance.EventCanvas.ShowLowerButton(true, lhoverInfo);
            MasterSingleton.Instance.EventCanvas.SetLowerButtonText(lowerChoice.buttonText);
            MasterSingleton.Instance.EventCanvas.AddLowerButtonAction(lowerChoice.action);
        }
        else
        {
            MasterSingleton.Instance.EventCanvas.ShowLowerButton(false, "");
        }
        MasterSingleton.Instance.UIManger.DisplayPointOfInterestSelectedUI(false);
        MasterSingleton.Instance.UIManger.DisplayExplorerCanvas(false);
        MasterSingleton.Instance.UIManger.DisplayOverworldUI(false);
        MasterSingleton.Instance.EventCanvas.ShowEventCanvas(true);
    }

    string EventEffectsToStringDescription(int cred, int scrap, int artefacts, int stress, int hp)
    {
        string description = "";

        if (Mathf.Abs(cred) > 0)
        {
            description += "Cred" + DetermineChangeSymbol(cred);
        }
        if (Mathf.Abs(cred) > 1)
        {
            description += DetermineChangeSymbol(cred);
        }
        if (Mathf.Abs(cred) > 2)
        {
            description += DetermineChangeSymbol(cred);
        }
        description += " ";

        if (Mathf.Abs(scrap) > 0)
        {
            description += "Scrap" + DetermineChangeSymbol(scrap);
        }
        if (Mathf.Abs(scrap) > 1)
        {
            description += DetermineChangeSymbol(scrap);
        }
        if (Mathf.Abs(scrap) > 2)
        {
            description += DetermineChangeSymbol(scrap);
        }
        description += " ";

        if (Mathf.Abs(artefacts) > 0)
        {
            description += "Artefacts" + DetermineChangeSymbol(artefacts);
        }
        if (Mathf.Abs(artefacts) > 1)
        {
            description += DetermineChangeSymbol(artefacts);
        }
        if (Mathf.Abs(artefacts) > 2)
        {
            description += DetermineChangeSymbol(artefacts);
        }
        description += " ";

        if (Mathf.Abs(stress) > 0)
        {
            description += "Stress" + DetermineChangeSymbol(stress);
        }
        if (Mathf.Abs(stress) > 1)
        {
            description += DetermineChangeSymbol(stress);
        }
        if (Mathf.Abs(stress) > 2)
        {
            description += DetermineChangeSymbol(stress);
        }
        description += " ";

        if (Mathf.Abs(hp) > 0)
        {
            description += "Vigor" + DetermineChangeSymbol(hp);
        }
        if (Mathf.Abs(hp) > 1)
        {
            description += DetermineChangeSymbol(hp);
        }
        if (Mathf.Abs(hp) > 2)
        {
            description += DetermineChangeSymbol(hp);
        }
        return description;
    }

    string DetermineChangeSymbol(float changeValue)
    {
        string changeSymbol = "";
        if (changeValue < 0)
        {
            changeSymbol = "↓";
        }
        else if (changeValue > 0)
        {
            changeSymbol = "↑";
        }
        return changeSymbol;
    }
}
