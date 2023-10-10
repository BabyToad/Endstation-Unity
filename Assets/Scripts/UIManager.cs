using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update



    [SerializeField]
    Button _startExpedition, _endExpedition, _explore, _recover, _indulgeVice, _recruitExplorer;
    [SerializeField]
    Text _cred, _reputation;

    public void DowntimeUI(bool value)
    {
        
        _startExpedition.interactable = value;
        _recover.interactable = value;
        _indulgeVice.interactable = value;
        _recruitExplorer.interactable = value;

        _endExpedition.interactable = !value;
        _explore.interactable = !value;
    }

    public void ExpeditionUI(bool value)
    {

        _startExpedition.interactable = !value;
        _recover.interactable = !value;
        _indulgeVice.interactable = !value;
        _recruitExplorer.interactable = !value;

        _endExpedition.interactable = value;
        _explore.interactable = value;
    }

    public void UpdateCredDisplay(float newValue)
    {
        _cred.text = newValue.ToString();
    }
    public void UpdateReputationDisplay(float newValue)
    {
        _reputation.text = newValue.ToString();
    }

}
