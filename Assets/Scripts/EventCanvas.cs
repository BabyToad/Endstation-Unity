using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EventCanvas : MonoBehaviour
{
    [SerializeField]
    Canvas _mainCanvas;
    [SerializeField]
    Image _image;
    [SerializeField]
    TextMeshProUGUI _bodyText;
    [SerializeField]
    TextMeshProUGUI _upperButtonText;
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

    public void SetBodyText(string text)
    {
        _bodyText.text = text;
    }
    public void SetUpperButtonText(string text)
    {
        _upperButtonText.text = text;
    }
    public void SetLowerButtonText(string text)
    {
        _lowerButtonText.gameObject.SetActive(false);

    }
}
