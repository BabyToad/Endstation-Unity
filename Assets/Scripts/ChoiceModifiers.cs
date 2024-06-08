using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ChoiceModifiers
{
    public string buttonText;
    [Header("Stat Changes")]
    public int cred;
    public int scrap;
    public int artefacts;
    public int stress;
    public int hp;
    public bool allExplorers = false;
    [Header("Lock/Unlock POIs")]
    public string unlockPoI, lockPoI;
    [Header("Change POI Details")]
    public bool setDetails;
    public string POI;
    public int[] details;
    [Header("Change POI Action")]
    public UnityAction action;
    public bool enableAction;
    public bool overrideAction;
    public int actionIndex;
    public string pointOfInterest;
    public PointOfInterest.Action newAction;
}
