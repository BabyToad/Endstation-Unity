using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ChoiceModifiers
{
    public string buttonText;
    public int cred;
    public int scrap;
    public int artefacts;
    public int stress;
    public int hp;
    public bool allExplorers = false;
    public string unlockPoI, lockPoI;
    public bool setDetails;
    public string POI;
    public int[] details;
    public UnityAction action;
    public bool enableAction;
    public bool overrideAction;
    public int actionIndex;
    public string pointOfInterest;
    public PointOfInterest.Action newAction;
}
