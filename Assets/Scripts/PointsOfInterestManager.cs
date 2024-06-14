using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsOfInterestManager : MonoBehaviour
{
    [SerializeField] List<PointOfInterest> pointOfInterests;

    public List<PointOfInterest> PointOfInterests { get => pointOfInterests; set => pointOfInterests = value; }

    public void Register(PointOfInterest poi)
    {
        if (pointOfInterests.Contains(poi))
        {
            Debug.LogWarning( poi.name + " is already registered.");
            return;
        }
        pointOfInterests.Add(poi);
    }

    public void Deregister(PointOfInterest poi)
    {
        if (!pointOfInterests.Contains(poi))
        {
            Debug.LogWarning(poi.name + " was already not registered.");
            return;
        }
        pointOfInterests.Remove(poi);
    }
}
