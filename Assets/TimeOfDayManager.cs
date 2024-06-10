using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOfDayManager : MonoBehaviour
{
    [SerializeField][Range(0, 1)] float master = 1;
    [SerializeField] float sunIntensity;
    [SerializeField] float skyIntensity;

    [SerializeField] Light _sun;
    [SerializeField] Material _skyboxMat;
    // Start is called before the first frame update
    void Start()
    {
        sunIntensity = _sun.intensity;
        _skyboxMat = RenderSettings.skybox;
        skyIntensity = _skyboxMat.GetFloat("_Intensity");
        
    }

    // Update is called once per frame
    void Update()
    {
        SetSunIntensity();
        SetSkyboxIntensity();
    }


    private void SetSunIntensity()
    {
        _sun.intensity = Mathf.Lerp(0, sunIntensity, master) ;
    }

    private void SetSkyboxIntensity()
    {
        _skyboxMat.SetFloat("_Intensity", Mathf.Lerp(0, skyIntensity, master));

    }
}
