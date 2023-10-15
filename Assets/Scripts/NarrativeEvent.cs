using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "Narrative Event", menuName = "ScriptableObjects/Narrative Event", order = 1)]
public class NarrativeEvent : ScriptableObject
{
    EventCanvas _eventCanvas;

    int _id;
    [SerializeField]
    string _name;
    [SerializeField]
    string _bodyText;
    [SerializeField]
    string _upperButtonText;
    [SerializeField]
    string _lowerButtonText;
    [SerializeField]
    Sprite _sprite;


   


    public void Trigger()
    {
        MasterSingleton.Instance.EventCanvas.SetBodyText(_bodyText);
        MasterSingleton.Instance.EventCanvas.SetEventImage(_sprite);
        MasterSingleton.Instance.EventCanvas.SetUpperButtonText(_upperButtonText);
        MasterSingleton.Instance.EventCanvas.SetLowerButtonText(_lowerButtonText);
        MasterSingleton.Instance.EventCanvas.ShowEventCanvas(true);
    }
}
