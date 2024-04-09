using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
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
    TextMeshProUGUI _upperHoverInfo;
    bool _upperHover = false;
    [SerializeField]
    Button _lowerButton;
    [SerializeField]
    TextMeshProUGUI _lowerButtonText;
    [SerializeField]
    TextMeshProUGUI _lowerHoverInfo;
    bool _lowerHover = false;

    [SerializeField]
    Scrollbar _scrollbar;

    private void Start()
    {
        ShowEventCanvas(false);
        ResizeScrollBar();
    }

    private void OnEnable()
    {
        ResizeScrollBar();
        ResetScrollBar();
    }
    private void Update()
    {
        ResizeScrollBar();

        if (_upperHover)
        {
            _upperHoverInfo.enabled = true;
            Vector2 mousePos = MasterSingleton.Instance.InputManager.InputActions.Gameplay.Mouse.ReadValue<Vector2>();
            _upperHoverInfo.rectTransform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }
        else
        {
            _upperHoverInfo.enabled = false;
        }

        if (_lowerHover)
        {
            _lowerHoverInfo.enabled = true;
            Vector2 mousePos = MasterSingleton.Instance.InputManager.InputActions.Gameplay.Mouse.ReadValue<Vector2>();
            _lowerHoverInfo.rectTransform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }
        else
        {
            _lowerHoverInfo.enabled = false;
        }

    }
    public void ShowEventCanvas(bool value)
    {
        ResizeScrollBar();
        ResetScrollBar();
        _mainCanvas.gameObject.SetActive(value);
        _upperHover = false;
        _upperHoverInfo.enabled = false;
        _lowerHover = false;
        _lowerHoverInfo.enabled = false;
    }

    public void SetEventImage(Sprite sprite)
    {
        if (sprite == null)
        {
            _image.sprite = sprite;
            _image.color = Color.clear;
        }
        else
        {
            _image.sprite = sprite;
            _image.color = Color.white;
        }
    }

    public void SetEventName(string name)
    {
        if (_nameText != null)
        {
            _nameText.text = name;

        }
    }

    public void SetBodyText(string text)
    {
        _bodyText.text = text;
    }
    public void SetUpperButtonText(string text, string hoverInfo)
    {
        _upperButtonText.text = text;
        _upperHoverInfo.text = hoverInfo;
    }

    public void ShowLowerButton(bool value, string hoverInfo)
    {
        _lowerButton.gameObject.SetActive(value);
        _lowerHoverInfo.text = hoverInfo;

    }
    public void SetLowerButtonText(string text)
    {
        _lowerButtonText.text = text;
    }


    public void AddUpperButtonAction(UnityAction action)
    {
        _upperButton.onClick.RemoveAllListeners();
        _upperButton.onClick.AddListener(action);
        AudioManager.instance.PlayOneShot(FMODEvents.instance._uiClick);
    }
    public void AddLowerButtonAction(UnityAction action)
    {
        _lowerButton.onClick.RemoveAllListeners();
        _lowerButton.onClick.AddListener(action);
        AudioManager.instance.PlayOneShot(FMODEvents.instance._uiClick);
    }

    public void ResizeScrollBar()
    {
        _scrollbar.size = 0;
    }
    public void ResetScrollBar()
    {
        _scrollbar.value = 1;
    }

    public void OnUpperButtonEnter()
    {
        _upperHover = true;
        _upperHoverInfo.enabled = true; 
    }
    public void OnUpperButtonExit()
    {
        _upperHover = false;
        _upperHoverInfo.enabled = false;
    }

    public void OnLowerButtonEnter()
    {
        _lowerHover = true;
        _lowerHoverInfo.enabled = true;
    }
    public void OnLowerButtonExit()
    {
        _lowerHover = false;
        _lowerHoverInfo.enabled = false;
    }

}
