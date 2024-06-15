using MoreMountains.Feedbacks;
using System.Collections.Generic;
using UnityEngine;

public class DayTimeManager : MonoBehaviour
{
    float lightIntensity = 1;
    [SerializeField][Range(0, 1)] float timeTarget;
    [SerializeField] AnimationCurve curve;
    [SerializeField][Range(0, 1)] public float dawn, morning, noon, dusk, night;
    [SerializeField] float sunIntensity;
    [SerializeField] float skyIntensity;
    Color ogSkyColor, ogEquatorColor, ogGroundColor;
    [SerializeField] Light _sun;
    [SerializeField] Material _skyboxMat;
    [SerializeField] List<Renderer> _cloudDisks;

    [SerializeField] float speed = 0.1f;
    private float targetTime;

    // Start is called before the first frame update
    void Start()
    {
        _skyboxMat = RenderSettings.skybox;
        ogSkyColor = RenderSettings.ambientSkyColor;
        ogEquatorColor = RenderSettings.ambientEquatorColor;
        ogGroundColor = RenderSettings.ambientGroundColor;
        targetTime = timeTarget; // Initialize targetTime with the initial timeTarget value
    }

    // Update is called once per frame
    void Update()
    {
        // Gradually move timeTarget towards targetTime
        if (timeTarget != targetTime)
        {
            timeTarget = Mathf.MoveTowards(timeTarget, targetTime, speed * Time.deltaTime);
        }

        lightIntensity = curve.Evaluate(timeTarget);

        SetSunIntensity();
        SetSkyboxIntensity();
        SetCloudIntensity();
        SetAmbientLight();
    }

    private void OnDestroy()
    {
        RestoreDefaults();
    }

    private void SetSunIntensity()
    {
        _sun.intensity = Mathf.Lerp(0, sunIntensity, lightIntensity);
    }

    private void SetSkyboxIntensity()
    {
        _skyboxMat.SetFloat("_Intensity", Mathf.Lerp(0, skyIntensity, lightIntensity));
    }

    private void SetCloudIntensity()
    {
        _cloudDisks[0].material.SetFloat("_DayTime", Mathf.Lerp(1, 0, lightIntensity));
        _cloudDisks[1].material.SetFloat("_DayTime", Mathf.Lerp(1, 0, lightIntensity));
    }

    private void SetAmbientLight()
    {
        RenderSettings.ambientSkyColor = ogSkyColor * curve.Evaluate(timeTarget);
        RenderSettings.ambientEquatorColor = ogEquatorColor * curve.Evaluate(timeTarget);
        RenderSettings.ambientGroundColor = ogGroundColor * curve.Evaluate(timeTarget);
        RenderSettings.reflectionIntensity = curve.Evaluate(timeTarget);
    }

    private void RestoreDefaults()
    {
        RenderSettings.ambientSkyColor = ogSkyColor;
        RenderSettings.ambientEquatorColor = ogEquatorColor;
        RenderSettings.ambientGroundColor = ogGroundColor;
    }

    public void SetTime(float t)
    {
        targetTime = Mathf.Clamp(t, 0, 1);
    }
}
