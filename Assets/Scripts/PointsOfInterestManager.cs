using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PointsOfInterestManager : MonoBehaviour
{
    [System.Serializable]
    public class Region
    {
        public string name;
        public bool unlocked;
        public GameObject ui;
        public CinemachineVirtualCamera virtualCamera;
        public List<PointOfInterest> pointOfInterests;
        public List<int> connectedRegionIndexes;
        public List<Region> connectedRegions;
    }

    public Region[] regions;
    private CinemachineBrain _cmBrain;
    private Region _activeRegion;

    private void Start()
    {
        _cmBrain = Camera.main.GetComponent<CinemachineBrain>();
        InitializeRegions();

        foreach (Region rg in regions)
        {
            foreach (int index in rg.connectedRegionIndexes)
            {
                rg.connectedRegions.Add(regions[index]);
            }
        }

        _activeRegion = regions[0];
        UpdatePOIDisplay(); // Initialize POI display
    }

    private void Update()
    {
        UpdateRegionUI();
    }

    private void InitializeRegions()
    {
        foreach (Region region in regions)
        {
            region.ui.SetActive(false);
        }
    }

    private void UpdateRegionUI()
    {
        if (_cmBrain.ActiveVirtualCamera.VirtualCameraGameObject.CompareTag("Region Cam") && !_cmBrain.IsBlending)
        {
            foreach (Region cReg in _activeRegion.connectedRegions)
            {
                if (cReg.unlocked)
                {
                    cReg.ui.SetActive(true);
                }
            }
        }
        else
        {
            foreach (Region reg in regions)
            {
                reg.ui.SetActive(false);
            }
        }
    }

    void UpdateRegionCam()
    {
        foreach (Region region in regions)
        {
            region.virtualCamera.m_Priority = 0;
        }
        _activeRegion.virtualCamera.m_Priority = 100;
    }

    void UpdatePOIDisplay()
    {
        foreach (Region region in regions)
        {
            bool isActiveRegion = region == _activeRegion;
            foreach (PointOfInterest poi in region.pointOfInterests)
            {
                poi.Display = isActiveRegion && poi.Active;
            }
        }
    }

    public void GoToRegion(int index)
    {
        if (index < 0 || index >= regions.Length)
        {
            Debug.LogError($"Invalid region index: {index}");
            return;
        }

        if (regions[index].unlocked)
        {
            _activeRegion = regions[index];
        }
        else
        {
            Debug.LogError($"Region locked");
            return;
        }

        UpdateRegionCam();
        UpdatePOIDisplay(); // Update POIs in a single frame
    }

    public void UnlockRegion(int index)
    {
        if (index >= 0 && index < regions.Length)
        {
            UnlockRegion(regions[index]);
        }
        else
        {
            Debug.LogError($"Invalid region index: {index}");
        }
    }

    public void UnlockRegion(Region region)
    {
        region.unlocked = true;
        // UI visibility is handled in UpdateRegionUI
    }
}
