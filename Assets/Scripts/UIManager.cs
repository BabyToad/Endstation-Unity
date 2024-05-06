using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Button _endCycle;
    [SerializeField]

    Canvas _overworldUI;
    [SerializeField]

    Text _cred, _reputation;
    [SerializeField]
    Canvas _explorerCanvas;
    [SerializeField]
    List<PointOfInterest> _pointsOfInterestList;

    public List<PointOfInterest> PointsOfInterestList { get => _pointsOfInterestList; set => _pointsOfInterestList = value; }

    

    public void DisplayOverworldUI(bool value)
    {
        _overworldUI.enabled = value;
       
       HighlightEndCycle(MasterSingleton.Instance.Guild.IsRosterExhausted());
    }
    public void UpdateCredDisplay(float newValue)
    {
        _cred.text = newValue.ToString();
    }
    public void UpdateReputationDisplay(float newValue)
    {
        _reputation.text = newValue.ToString();
    }

    public void DisplayExplorerCanvas(bool value)
    {
        _explorerCanvas.enabled = value;
    }

    public void DisplayPointOfInterestSelectedUI(bool value)
    {
        for (int i = 0; i < PointsOfInterestList.Count; i++)
        {
            if (PointsOfInterestList[i].IsSelected)
            {
                PointsOfInterestList[i].DisplaySelectUI(value);
            }
        }
    }

    public void HighlightEndCycle(bool value)
    {
        _endCycle.GetComponent<Animator>().SetBool("RosterIsExhausted", value);
    }

    public void EnableEndCycleButton(bool value)
    {
        _endCycle.interactable = value;
    }
}
