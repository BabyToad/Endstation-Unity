using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Button _endCycle;
    [SerializeField] Canvas _overworldUI;
    [SerializeField] TextMeshProUGUI _cred, _reputation, _scrap, _artifact;
    [SerializeField] Canvas _explorerCanvas;
    [SerializeField] List<PointOfInterest> _pointsOfInterestList;

    public List<PointOfInterest> PointsOfInterestList { get => _pointsOfInterestList; set => _pointsOfInterestList = value; }
    [SerializeField] List<Sprite> _clockSprites;
    [SerializeField] Image _cycleClock;

    [SerializeField] GameObject startScreen;
    private void Start()
    {
        LoadClockSprites(4);
        UpdateCycleClock();
        EnableStartScreen();
    }


    public void DisplayOverworldUI(bool value)
    {
        _overworldUI.enabled = value;
       
       HighlightEndCycle(MasterSingleton.Instance.Guild.IsRosterExhausted());
    }
    public void UpdateCredDisplay(float newValue)
    {
        _cred.text = newValue.ToString();
    }
    public void UpdateScrapDisplay(float newValue)
    {
        _scrap.text = newValue.ToString();
    }
    public void UpdateArtifactDisplay(float newValue)
    {
        _artifact.text = newValue.ToString();
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

    public void UpdateCycleClock()
    {
        
        _cycleClock.sprite = _clockSprites[MasterSingleton.Instance.Guild.Cycle];
    }

    void LoadClockSprites(int segments)
    {
        _clockSprites.Clear();

        if (segments == 0)
        {

            foreach (Sprite sprite in Resources.LoadAll<Sprite>("UI/Progress Clocks/New0Clock"))
            {
                _clockSprites.Add(sprite);
            }
            _clockSprites.Reverse();
        }

        if (segments == 4)
        {

            foreach (Sprite sprite in Resources.LoadAll<Sprite>("UI/Progress Clocks/New4Clock"))
            {
                _clockSprites.Add(sprite);
            }

        }
        if (segments == 6)
        {

            foreach (Sprite sprite in Resources.LoadAll<Sprite>("UI/Progress Clocks/New6Clock"))
            {
                _clockSprites.Add(sprite);
            }

        }
        if (segments == 8)
        {
            foreach (Sprite sprite in Resources.LoadAll<Sprite>("UI/Progress Clocks/New8Clock"))
            {
                _clockSprites.Add(sprite);
            }
        }
        _clockSprites.Reverse();
    }

    private void EnableStartScreen()
    {
        startScreen.SetActive(true);
    }
}
