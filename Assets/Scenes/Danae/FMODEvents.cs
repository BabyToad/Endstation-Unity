using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents instance {get; private set;}

    [field: Header("Music")]
    [field:SerializeField]
    public EventReference _music {get ; private set;}
    public EventInstance _musicEI;

    [field: Header("UI")]

    [field:SerializeField]
    public EventReference _cameraIn {get ; private set;}

    [field:SerializeField]
    public EventReference _cameraOut {get ; private set;}

    [field:SerializeField]
    public EventReference _dice {get ; private set;}

    [field:SerializeField]
    public EventReference _uiClick {get ; private set;}


    [field: Header("Environment, punctual")]
    [field:SerializeField]
    public EventReference _creditoriumIntro {get ; private set;}

    [field:SerializeField]
    public EventReference _endstationIntro {get ; private set;}

    [field:SerializeField]
    public EventReference _oasisIntro {get ; private set;}

    [field:SerializeField]
    public EventReference _rootsIntro {get ; private set;}

    [field:SerializeField]
    public EventReference _templeIntro {get ; private set;}

    
    //[field: Header("Narrative")]

    [field: Header("Controllers")]
    [field:SerializeField]
    public EventReference _endTurn {get ; private set;}
    [field:SerializeField]
    public EventReference _menuStart {get ; private set;}
    [field:SerializeField]
    public EventReference _menuStop {get ; private set;}


    private void Awake()
    {
        if (instance!= null)
    {
        Debug.LogError("Found more than one FMODEvents instance in the scene.");
    }
        instance=this;

        CreateInstances();
    }

    //creates instances for unique, loopable events. Allows to manipulate their local parameters more easily.
    private void CreateInstances()
    {
        _musicEI = RuntimeManager.CreateInstance(_music);
    }

}
