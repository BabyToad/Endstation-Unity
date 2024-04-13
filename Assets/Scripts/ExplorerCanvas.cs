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
    public List<Button> AdvancementButtons { get => _advancementButtons; set => _advancementButtons = value; }

    public float animationDuration = 0.5f;
    private float currentValue;

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
        //_health.value = (float)health;
        StartCoroutine(AnimateSlider((float)health, _health));
    }
    public void SetStress(int stress)
    {
        //_stress.value = (float)stress;
        StartCoroutine(AnimateSlider((float)stress, _stress));

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
        for (int i = 0; i < AdvancementButtons.Count; i++)
        {
            AdvancementButtons[i].gameObject.SetActive(value);
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

    private IEnumerator AnimateSlider(float targetValue, Slider slider)
    {
        float startTime = Time.time;
        float startValue = slider.value;

        // Briefly show afterimage of old value
        yield return new WaitForSeconds(0.1f);

        while (Time.time - startTime < animationDuration)
        {
            float elapsedTime = Time.time - startTime;
            float newValue = Mathf.Lerp(startValue, targetValue, elapsedTime / animationDuration);
            slider.value = newValue;
            yield return null;
        }

        // Ensure final value is set correctly
        slider.value = targetValue;
    }
}
