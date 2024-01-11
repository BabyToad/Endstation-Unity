using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODEvents : MonoBehaviour
{
    public static FMODEvents instance {get; private set;}

    [field:SerializeField]
    public EventReference _test {get ; private set;}

    public EventInstance _testEI; 
    public GameObject _creditorTemple;


    private void Awake()
    {
        if (instance!= null)
    {
        Debug.LogError("Found more than one FMODEvents instance in the scene.");
    }
        instance=this;

        CreateInstances();
        _creditorTemple=GameObject.Find("CreditorTempleV1");


    }

    private void CreateInstances()
    {
        _testEI=RuntimeManager.CreateInstance(_test);
    }

}
