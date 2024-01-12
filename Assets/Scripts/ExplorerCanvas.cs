using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplorerCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Text _name;
    [SerializeField]
    Slider _health, _stress;
    [SerializeField]
    Text _insight, _prowess, _resolve;
    [SerializeField]
    Button _selectExplorer;
    [SerializeField]
    Image _background, _backgroundSelected;
    [SerializeField]
    Color _normalColor, _selectedColor, _exhaustedColor, _selectedExhaustedColor;
    public Button SelectExplorer { get => _selectExplorer; set => _selectExplorer = value; }

    private void Awake()
    {
        _normalColor = _background.color;
        _selectedColor = _backgroundSelected.color;
    }
    public void SetName(string name)
    {
        _name.text = name;
    }

    public void SetHealth(int health)
    {
        _health.value = (float)health;
    }
    public void SetStress(int stress)
    {
        _stress.value = (float)stress;
    }
    public void SetInsight(string insight)
    {
        _insight.text = "Insight: " + insight;
    }
    public void SetProwess(string prowess)
    {
        _prowess.text = "Prowess: " + prowess;
    }
    public void SetResolve(string resolve)
    {
        _resolve.text = "Resolve: " + resolve;
    }

    

    public void DisplayExhaustion(bool value)
    {
        if (value)
        {
            _background.color = _exhaustedColor;
            _backgroundSelected.color = _selectedExhaustedColor;
        }
        else
        {
            _background.color = _normalColor;
            _backgroundSelected.color = _selectedColor;

        }
    }
    
    
}
