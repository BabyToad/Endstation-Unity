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
    List<Button> _advancementButtons;
    [SerializeField]
    Image _background, _backgroundSelected;
    [SerializeField]
    Color _normalColor, _selectedColor, _exhaustedColor, _selectedExhaustedColor;
    public Button SelectExplorer { get => _selectExplorer; set => _selectExplorer = value; }

    private void Awake()
    {
        _normalColor = _background.color;
        _selectedColor = _backgroundSelected.color;
        ShowAdvancementButtons(false);
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
    public void SetInsight(int insight, bool isInjured, bool isTrauma)
    {
        SetStatColor(_insight, isInjured, isTrauma);
        _insight.text = "Insight: " + insight;
    }
    public void SetProwess(int prowess, bool isInjured, bool isTrauma)
    {
        SetStatColor(_prowess, isInjured, isTrauma);

        _prowess.text = "Prowess: " + prowess;
    }
    public void SetResolve(int resolve, bool isInjured, bool isTrauma)
    {
        SetStatColor(_resolve, isInjured, isTrauma);

        _resolve.text = "Resolve: " + resolve;
    }


    public void ShowAdvancementButtons(bool value)
    {
        for (int i = 0; i < _advancementButtons.Count; i++)
        {
            _advancementButtons[i].gameObject.SetActive(value);
        }
    }

    void SetStatColor(Text stat, bool isInjured, bool isTrauma)
    {
        if (isInjured && isTrauma)
        {
            stat.color = Color.red;
        }
        else if (isInjured || isTrauma)
        {
            stat.color = Color.yellow;
        }
        else if (!isInjured && !isTrauma)
        {
            stat.color = Color.white;
        }
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
