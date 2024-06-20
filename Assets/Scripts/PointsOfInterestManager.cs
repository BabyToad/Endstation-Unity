using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsOfInterestManager : MonoBehaviour
{
    [System.Serializable]

    public class Region
    {
        [SerializeField] public string name;
        [SerializeField] public bool enabled;
        [SerializeField] public GameObject ui;
        [SerializeField] public CinemachineVirtualCamera virtualCamera;
        [SerializeField] public List<PointOfInterest> pointOfInterests;

    }

    [SerializeField] Region[] regions;

    public void GoToRegion(int k)
    {
        for (int i = 0; i < regions.Length; i++)
        {
            regions[i].enabled = false;
            regions[i].virtualCamera.Priority = 5;
            regions[i].ui.SetActive(true);

            for (int j = 0; j < regions[i].pointOfInterests.Count; j++)
            {
                regions[i].pointOfInterests[j].SetDisplay(false);
            }
        }


        regions[k].enabled = true;
        regions[k].virtualCamera.Priority = 10;
        regions[k].ui.SetActive(false);

        for (int j = 0; j < regions[k].pointOfInterests.Count; j++)
        {
            if (regions[k].pointOfInterests[j].Active)
            {
                regions[k].pointOfInterests[j].SetDisplay(true);
            }
        }
    }
}
