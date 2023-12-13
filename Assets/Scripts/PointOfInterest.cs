using Cinemachine;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointOfInterest : MonoBehaviour
{
    [SerializeField]
    bool _active;
    [System.Serializable]
    class Action
    {
        [System.Serializable]
        public class Result
        {
            [SerializeField]
            float _cred;
            [SerializeField]
            int _stress;
            [SerializeField]
            int _hp;
            [SerializeField]
            float _xp;

            public float Cred { get => _cred; set => _cred = value; }
            public int Stress { get => _stress; set => _stress = value; }
            public int Hp { get => _hp; set => _hp = value; }
            public float Xp { get => _xp; set => _xp = value; }

            public void Apply()
            {
                MasterSingleton.Instance.Guild.AddCred(Cred);
                MasterSingleton.Instance.Guild.SelectedExplorer.AddStress(Stress);
                MasterSingleton.Instance.Guild.SelectedExplorer.AddHealth(Hp);

            }
        }

        [SerializeField]
        Result _fail, _partial, _success;

        public Result Fail { get => _fail; set => _fail = value; }
        public Result Partial { get => _partial; set => _partial = value; }
        public Result Success { get => _success; set => _success = value; }


    }
    [SerializeField]
    Action _mainAction;

    [SerializeField]
    List<ProgressClock> _clocks;
    int _activeClock;
    [SerializeField]
    CinemachineVirtualCamera _vcam;
    [SerializeField]
    Canvas _activeCanvas, _worldCanvas;
    [SerializeField]
    TextMeshProUGUI _activeExplorerText;
    [SerializeField]
    TextMeshProUGUI _description;
    [SerializeField]
    TextMeshProUGUI _consequences;

    CinemachineBrain _cmbrain;

    bool _isSelected;

    bool _mouseIsOverUI;

    //Clock
    [SerializeField]
    Image _activeClockImage, _activeClockFrame, _activeClockBackground, _worldClockImage, _worldClockFrame, _worldClockBackground;
    [SerializeField]
    List<Sprite> _clockSprites;
    Sprite _clockFrameSprite, _clockBackgroundSprite;

    [SerializeField]
    Color _baseColor, _filledColor, _countdownColor;

    [SerializeField]
    MoreMountains.Feedbacks.MMF_Player _actionFeedback, _actionFailedFeedback; 
    
    //UI Selection
    GraphicRaycaster _graphicsRaycasterWorldCanvas;

    public bool IsSelected { get => _isSelected; set => _isSelected = value; }

    private void OnEnable()
    {
        Guild.OnEndCycle += CountDownClock; 
        

        LoadClockSprites(_clocks[_activeClock].Segments);
        DisplayClock(_clocks[_activeClock].Fill);
        
        if (IsSelected)
        {
            Select();
        }
        else
        {
            DeSelect();
        }
    }
    private void Start()
    {
        MasterSingleton.Instance.InputManager.InputActions.Gameplay.Select.performed += Select_performed;
        _cmbrain = Camera.main.GetComponent<CinemachineBrain>();
        _graphicsRaycasterWorldCanvas = _worldCanvas.GetComponent<GraphicRaycaster>();
        RegisterWithUIHandler();
        SetActive(_active);
    }

    private void Update()
    {
        _mouseIsOverUI = IsMouseOverUI();

        

        if (_activeCanvas.gameObject.activeSelf)
        {
            
            if (MasterSingleton.Instance.Guild.SelectedExplorer != null)
            {
                _activeExplorerText.text = MasterSingleton.Instance.Guild.SelectedExplorer.Name;
            }
            else
            {
                _activeExplorerText.text = "";
            }
        }


        if (_cmbrain.ActiveVirtualCamera.Name == "WorldCam" && !_cmbrain.IsBlending)
        {
            DisplayWorldUI(true);
        }
        else
        {
            DisplayWorldUI(false);
        }
    }

    

    void RegisterWithUIHandler()
    {
        if (!MasterSingleton.Instance.UIManger.PointsOfInterestList.Contains(this))
        {
            MasterSingleton.Instance.UIManger.PointsOfInterestList.Add(this);
        }
        else
        {
            Debug.LogWarning("UIManager Points of Interest List already contains " + this.name);
        }
    }
    private void Select_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (!_mouseIsOverUI)
        {
            Ray mouseClick = Camera.main.ScreenPointToRay(MasterSingleton.Instance.InputManager.InputActions.Gameplay.Mouse.ReadValue<Vector2>());
            RaycastHit hit;
            Physics.Raycast(mouseClick, out hit);
            DeSelect();

            if (hit.transform == this.transform)
            {
                Select();
            }
        }
        GraphicRaycast();
    }

    private void OnDisable()
    {
        MasterSingleton.Instance.InputManager.InputActions.Gameplay.Select.performed -= Select_performed;
        Guild.OnEndCycle -= CountDownClock;

    }


    public void Select()
    {
        if (_active)
        {
            _vcam.Priority = 100;
            DisplaySelectUI(true);

            IsSelected = true;
            Debug.Log("Selected " + this.name);
        }
        
    }

    public void DeSelect()
    {
        _vcam.Priority = 1;
        DisplaySelectUI(false);
        IsSelected = false;
        Debug.Log("Deselected "+ this.name);
    }
    private bool IsMouseOverUI() 
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    void GraphicRaycast()
    {
        Debug.Log("Raycast!");
        PointerEventData m_PointerEventData = new PointerEventData(EventSystem.current);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = MasterSingleton.Instance.InputManager.InputActions.Gameplay.Mouse.ReadValue<Vector2>();

        List<RaycastResult> results = new List<RaycastResult>();

        _graphicsRaycasterWorldCanvas.Raycast(m_PointerEventData, results);

        foreach (RaycastResult result in results)
        {
            Debug.Log("Hit " + result.gameObject.name);
        }
    }
    
    void LoadClockSprites(int segments)
    {
        _clockSprites.Clear();

        if (segments == 0)
        {
            _clockFrameSprite = Resources.Load<Sprite>("UI/Progress Clocks/New0Clock/Frame/0Frame");
            
            foreach (Sprite sprite in Resources.LoadAll<Sprite>("UI/Progress Clocks/New0Clock"))
            {
                _clockSprites.Add(sprite);
            }
            _clockSprites.Reverse();
        }

        if (segments == 4)
        {
            _clockFrameSprite = Resources.Load<Sprite>("UI/Progress Clocks/New4Clock/Frame/4Frame");

            foreach (Sprite sprite in Resources.LoadAll<Sprite>("UI/Progress Clocks/New4Clock"))
            {
                _clockSprites.Add(sprite);
            }
            
        }
        if (segments == 6)
        {
            _clockFrameSprite = Resources.Load<Sprite>("UI/Progress Clocks/New6Clock/Frame/6Frame");

            foreach (Sprite sprite in Resources.LoadAll<Sprite>("UI/Progress Clocks/New6Clock"))
            {
                _clockSprites.Add(sprite);
            }

        }
        if (segments == 8)
        {
            _clockFrameSprite = Resources.Load<Sprite>("UI/Progress Clocks/New8Clock/Frame/8Frame");
            foreach (Sprite sprite in Resources.LoadAll<Sprite>("UI/Progress Clocks/New8Clock"))
            {
                _clockSprites.Add(sprite);
            }
        }
        _clockBackgroundSprite = _clockSprites[0];
        _clockSprites.Reverse();
    }

    void DisplayWorldUI(bool value)
    {
        _worldClockImage.sprite = _clockSprites[_clocks[_activeClock].Fill];
        _worldClockBackground.sprite = _clockBackgroundSprite;
        _worldClockFrame.sprite = _clockFrameSprite;
        RecolorClock();

        _worldCanvas.gameObject.SetActive(value);
    }

    void DisplayClock(int fill)
    {
        _activeClockImage.sprite = _clockSprites[fill];
        _activeClockFrame.sprite = _clockFrameSprite;
        _activeClockBackground.sprite = _clockBackgroundSprite;
        _worldClockImage.sprite = _clockSprites[fill];
        _worldClockBackground.sprite = _clockBackgroundSprite;
        _worldClockFrame.sprite = _clockFrameSprite;
        _description.text = _clocks[_activeClock].Description;
        RecolorClock();
        _consequences.text = "Attribute: " + _clocks[_activeClock].ActionAttribute.ToString() + "\n" + ActionToStringDescription();
    }

    void RecolorClock()
    {
        if (0 == _clocks[_activeClock].Segments)
        {
            _worldClockImage.color = _filledColor;
            _activeClockImage.color = _filledColor;
        }
        else if (_clocks[_activeClock].IsCountdown)
        {
            _worldClockImage.color = _countdownColor;
            _activeClockImage.color = _countdownColor;
        }
        else
        {
            _worldClockImage.color = _baseColor;
            _activeClockImage.color = _baseColor;
        }
    }

    public void SetActive(bool value)
    {
        _active = value;
        _activeCanvas.transform.parent.gameObject.SetActive(value);
        _worldCanvas.transform.parent.gameObject.SetActive(value);
        
        if (value)
        {
            RegisterWithUIHandler();
        }
    }

    string ActionToStringDescription()
    {
        string description ="";
        if (_mainAction.Fail.Cred > 0 || _mainAction.Partial.Cred > 0 )
        {
            description += "Cred+ ";
        }
        if (_mainAction.Fail.Cred < 0 || _mainAction.Partial.Cred < 0 )
        {
            description += "Cred- ";
        }

        if (_mainAction.Fail.Stress > 0 || _mainAction.Partial.Stress > 0 )
        {
            description += "Stress+ ";
        }
        if (_mainAction.Fail.Stress < 0 || _mainAction.Partial.Stress < 0 )
        {
            description += "Stress- ";
        }

        if (_mainAction.Fail.Hp > 0 || _mainAction.Partial.Hp > 0 )
        {
            description += "HP+ ";
        }
        if (_mainAction.Fail.Hp < 0 || _mainAction.Partial.Hp < 0 )
        {
            description += "HP- ";
        }

        return description;
    
    }
    public void DisplaySelectUI(bool value)
    {
        _activeCanvas.gameObject.SetActive(value);
    }

    void NextClock()
    {
        _activeClock++;
        LoadClockSprites(_clocks[_activeClock].Segments);
        _clocks[_activeClock].Fill = 0;
        DisplayClock(_clocks[_activeClock].Fill);
    }

    public void UseAction()
    {
        if (!MasterSingleton.Instance.Guild.SelectedExplorer.Exhausted && !_clocks[_activeClock].IsCountdown)
        {



            int diceResult = MasterSingleton.Instance.Guild.SelectedExplorer.RollDice(_clocks[_activeClock].ActionAttribute);

            if (diceResult <= 3)
            {
                _clocks[_activeClock].ChangeFill(1);
                _activeClockImage.sprite = _clockSprites[_clocks[_activeClock].Fill];
                _activeClockFrame.sprite = _clockFrameSprite;
                _mainAction.Fail.Apply();
            }
            else if (diceResult <= 5)
            {
                _clocks[_activeClock].ChangeFill(2);
                _activeClockFrame.sprite = _clockFrameSprite;
                _activeClockImage.sprite = _clockSprites[_clocks[_activeClock].Fill];
                _mainAction.Partial.Apply();

            }
            else if (diceResult == 6)
            {
                _clocks[_activeClock].ChangeFill(3);
                _activeClockImage.sprite = _clockSprites[_clocks[_activeClock].Fill];
                _activeClockFrame.sprite = _clockFrameSprite;
                _mainAction.Success.Apply();

            }

            MasterSingleton.Instance.Guild.SelectedExplorer.Exhaust();

            

            if (_clocks[_activeClock].Fill >= _clocks[_activeClock].Segments)
            {
                NextClock();
            }
            else
            {
                if (MasterSingleton.Instance.Guild.IsRosterExhausted())
                {
                    DeSelect();
                }
            }

            Debug.Log(MasterSingleton.Instance.Guild.SelectedExplorer.Name + " used the Action at " + this.name);

          

            _actionFeedback.PlayFeedbacks();
        }
        else if (_clocks[_activeClock].IsCountdown)
        {
            Debug.LogWarning("This is a Countdown Clock.");
        }
        else
        {
            Debug.LogWarning("Explorer is exhausted.");
        }
        
    }

    void CountDownClock()
    {
        if (_clocks[_activeClock].IsCountdown && _active)
        {
            _clocks[_activeClock].ChangeFill(1);
            _activeClockImage.sprite = _clockSprites[_clocks[_activeClock].Fill];
            
            if (_clocks[_activeClock].Fill >= _clocks[_activeClock].Segments)
            {
                NextClock();
            }

        }

    }
}


    

