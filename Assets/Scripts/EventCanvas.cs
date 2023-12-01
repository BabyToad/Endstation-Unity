using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;


public class EventCanvas : MonoBehaviour
{
    [SerializeField]
    Canvas _mainCanvas;
    [SerializeField]
    Image _image;
    [SerializeField]
    TextMeshProUGUI _nameText;
    [SerializeField]
    TextMeshProUGUI _bodyText;
    [SerializeField]
    Button _upperButton;
    [SerializeField]
    TextMeshProUGUI _upperButtonText;
    [SerializeField]
    Button _lowerButton;
    [SerializeField]
    TextMeshProUGUI _lowerButtonText;

    private void Start()
    {
        ShowEventCanvas(false);
    }

    public void ShowEventCanvas(bool value)
    {
        _mainCanvas.gameObject.SetActive(value);
    }

    public void SetEventImage(Sprite sprite)
    {
        _image.sprite = sprite;
    }

    public void SetEventName(string name)
    {
        _nameText.text = name;
    }

    public void SetBodyText(string text)
    {
        _bodyText.text = text;
    }
    public void SetUpperButtonText(string text)
    {
        _upperButtonText.text = text;
    }

    public void ShowLowerButton(bool value)
    {
        _lowerButton.gameObject.SetActive(value);
    }
    public void SetLowerButtonText(string text)
    {
        _lowerButtonText.text = text;
    }

    public void AddUpperButtonAction(UnityAction action)
    {
        _upperButton.onClick.RemoveAllListeners();
        _upperButton.onClick.AddListener(action);
    }
    public void AddLowerButtonAction(UnityAction action)
    {
        _lowerButton.onClick.RemoveAllListeners();
        _lowerButton.onClick.AddListener(action);
    }

}
