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








}
