using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance {get; private set;}

     //Volume controls

    [Header("Volume")]
    [Range(0,1)]
    public float masterVolume=1;
    [Range(0,1)]

    public float musicVolume=1;
    [Range(0,1)]

    public float sfxVolume=1;
    [Range(0,1)]

    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;

    private void Awake()
    {
        if (instance!= null)
    {
        Debug.LogError("Found more than one AudioManager instance in the scene.");
    }
        instance=this;

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    private void Start()
    {
        Play(FMODEvents.instance._musicEI);
    }

    //*****One shot***** //uses event references
    public void PlayOneShot(EventReference eventReference)
    {
        EventInstance e = RuntimeManager.CreateInstance(eventReference);
        e.start();
    }

    public void PlayOneShot(EventReference eventReference, GameObject gameObject)
    {
        EventInstance e = RuntimeManager.CreateInstance(eventReference);
        RuntimeManager.PlayOneShotAttached(eventReference, gameObject);
    }


    //*****Play looping sounds***** //uses event instances to retrieve them later on
    public void Play(EventInstance eventInstance)
    {
        eventInstance.start();
    }

    //*****Parameters*****
    public void SetEventParameter(EventInstance eventInstance, string parameterName, float value)
    {
        eventInstance.setParameterByName(parameterName, value);
    }

        //Change the value of a Global Parameter
    public void SetGlobalParameter(string parameterName, float parameterValue)
    {
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName(parameterName, parameterValue);
    }

}
