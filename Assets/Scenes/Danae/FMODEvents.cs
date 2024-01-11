using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents instance {get; private set;}

    [field:SerializeField]
    public EventReference _dice {get ; private set;}

    [field:SerializeField]
    public EventReference _music {get ; private set;}

    public EventInstance _musicEI;

    [field:SerializeField]
    public EventReference _uiClick {get ; private set;}



    private void Awake()
    {
        if (instance!= null)
    {
        Debug.LogError("Found more than one FMODEvents instance in the scene.");
    }
        instance=this;

        CreateInstances();


    }

    private void CreateInstances()
    {
        _musicEI = RuntimeManager.CreateInstance(_music);
    }

}
