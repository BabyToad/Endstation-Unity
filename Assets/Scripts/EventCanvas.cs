using System.Collections;
using System.Linq;
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

    int textSnippetIterator;
    string[] _textSnippets;
    

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
    [SerializeField]
    ScrollRect scrollRect;
    private void Start()
    {
        ShowEventCanvas(false);
        ResizeScrollBar();

    }

    private void OnEnable()
    {
        ResizeScrollBar();
        ResetScrollBar();

        if (MasterSingleton.Instance != null)
        {
            MasterSingleton.Instance.InputManager.InputActions.Gameplay.Select.performed += SelectWord;

        }
    }

    private void OnDisable()
    {
        MasterSingleton.Instance.InputManager.InputActions.Gameplay.Select.performed -= SelectWord;

    }
    private void Update()
    {
        ResizeScrollBar();
    }
    public void ShowEventCanvas(bool value)
    {
        ResizeScrollBar();
        ResetScrollBar();
        _mainCanvas.gameObject.SetActive(value);
        HideButtons();
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
        SliceTextIntoSnippets(text);
        textSnippetIterator = 0;
        _bodyText.text = _textSnippets[textSnippetIterator] + "\nContinue...";
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
       
        _lowerButton.interactable = value;
        

    }

    public void HideButtons()
    {
        _upperButton.gameObject.SetActive(false);
        _lowerButton.gameObject.SetActive(false);
    }

    public void ShowButtons()
    {
        _upperButton.gameObject.SetActive(true);
        if (_lowerButton.interactable)
        {
            _lowerButton.gameObject.SetActive(true);
        }
        
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

    void SelectWord(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Vector2 mousePos = MasterSingleton.Instance.InputManager.InputActions.Gameplay.Mouse.ReadValue<Vector2>();
        int clickedWordIndex = TMP_TextUtilities.FindIntersectingWord(_bodyText, new Vector3(mousePos.x, mousePos.y, 0), null);
        Debug.Log("Tried to Select Word");
        if (clickedWordIndex != -1)
            {
                string clickedWord = _bodyText.textInfo.wordInfo[clickedWordIndex].GetWord();
                Debug.Log(clickedWord);
                if (clickedWord == "Continue")
                {
                    textSnippetIterator++;
                    ExpandText();
                }
            }
    }

    void SliceTextIntoSnippets(string text)
    {
        string[] unfilteredTextSnippets = text.Split("\n", System.StringSplitOptions.RemoveEmptyEntries);
        foreach (var item in unfilteredTextSnippets)
        {
            Debug.Log(item.Length);
            Debug.Log(item);
        }
        _textSnippets = unfilteredTextSnippets.Where(str => str.Length > 1).ToArray();

    }


    void ExpandText()
    {
        if (textSnippetIterator < _textSnippets.Length)
        {
            _bodyText.text += "\n" + _textSnippets[textSnippetIterator];
            _bodyText.text = RemoveContinue(_bodyText.text);
            if (textSnippetIterator + 1 < _textSnippets.Length)
            {
                _bodyText.text += "\nContinue...";
            }
            else
            {
                ShowButtons();
            }
            StartCoroutine(ScrollToBottomAfterUpdate());
           
        }

    }

    string RemoveContinue(string text)
    {
        string[] lines = text.Split('\n');
        var filteredLines = new System.Text.StringBuilder();

        foreach (string line in lines)
        {
            if (!line.EndsWith("Continue..."))
            {
                filteredLines.AppendLine(line);
            }
        }

        return filteredLines.ToString();
    }

    IEnumerator ScrollToBottomAfterUpdate()
    {
        yield return null; // Wait for layout to be recalculated

        // Configuration
        float scrollDuration = 0.5f;  // Duration in seconds
        int steps = 30;               // Number of steps for smoothness

        // Calculate movement per step
        float scrollStep = 1.0f / steps;

        // Scroll over multiple frames
        for (int i = 0; i < steps; i++)
        {
            scrollRect.verticalNormalizedPosition -= scrollStep;
            yield return null; // Wait for the next frame
        }

        // Ensure exact final position
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
