using Cinemachine;
using System.Collections;
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
    public class Action
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
            int _xp;

            public float Cred { get => _cred; set => _cred = value; }
            public int Stress { get => _stress; set => _stress = value; }
            public int Hp { get => _hp; set => _hp = value; }
            public int Xp { get => _xp; set => _xp = value; }

            public void Apply()
            {
                MasterSingleton.Instance.Guild.AddCred(Cred);
                foreach (Explorer explorer in MasterSingleton.Instance.Guild.SelectedExplorers)
                {
                    explorer.AddStress(Stress);
                    explorer.AddHealth(Hp);
                    explorer.AddExperience(Xp);
                }
            }
        }
        [SerializeField]
        bool _enabled;
        [SerializeField]
        Result _fail, _partial, _success;

        [SerializeField]
        int _explorerSlots;

        [SerializeField]
        List<ProgressClock> _clocks;
        int _activeClock;

        [SerializeField]
        ActionUI _actionUI;

        public Result Fail { get => _fail; set => _fail = value; }
        public Result Partial { get => _partial; set => _partial = value; }
        public Result Success { get => _success; set => _success = value; }
        public List<ProgressClock> Clocks { get => _clocks; set => _clocks = value; }
        public int ActiveClock { get => _activeClock; set => _activeClock = value; }
        public ActionUI ActionUI { get => _actionUI; set => _actionUI = value; }
        public bool Enabled { get => _enabled; set => _enabled = value; }
        public int ExplorerSlots { get => _explorerSlots; set => _explorerSlots = value; }
    }

    [SerializeField]

    Action[] _actions;


    [SerializeField]
    List<ProgressClock> _clocks;
    int _activeClock;

    bool _rollingDice = false;

    [Header("Cam Reference")]
    [SerializeField]
    CinemachineVirtualCamera _vcam;
    CinemachineBrain _cmbrain;


    [Header("UI References")]
    [SerializeField]
    Canvas _actionsCanvas;
    [SerializeField]
    Canvas _activeCanvas;
    [SerializeField]
    Canvas _worldCanvas;
    [SerializeField]
    TextMeshProUGUI _activeExplorerText;
    [SerializeField]
    TextMeshProUGUI _description;
    [SerializeField]
    TextMeshProUGUI _consequences;
    [SerializeField]
    Image _diceImage;
    [SerializeField]
    List<Sprite> _diceSprites;
    [SerializeField]
    List<Sprite> _animSprites;
    [SerializeField]
    Button _interact;

    bool _isSelected;

    bool _mouseIsOverUI;

    [Header("Clock UI References")]
    [SerializeField]
    List<Sprite> _clockSprites;
    [SerializeField]
    Image _activeClockImage, _activeClockFrame, _activeClockBackground, _worldClockImage, _worldClockFrame, _worldClockBackground;

    Sprite _clockFrameSprite, _clockBackgroundSprite;

    [SerializeField]
    Color _baseColor, _filledColor, _countdownColor;

    [Header("Feedbacks")]
    [SerializeField]
    MoreMountains.Feedbacks.MMF_Player _applyRollFeedback, _actionFailedFeedback;

    //UI Selection
    GraphicRaycaster _graphicsRaycasterWorldCanvas;

    [SerializeField]
    List<GameObject> _modelDetails;

    public bool IsSelected { get => _isSelected; set => _isSelected = value; }

    private void OnEnable()
    {
        Guild.OnNewCycle += CountDownClock;
        LoadDiceSprites();
        LoadClockSprites(_actions[0].Clocks[_actions[0].ActiveClock].Segments);

        AddActionUIs();

        if (IsSelected)
        {
            Select();
        }
        else
        {
            DeSelect();
        }
    }

    private void OnDisable()
    {
        MasterSingleton.Instance.InputManager.InputActions.Gameplay.Select.performed -= Select_performed;
        MasterSingleton.Instance.InputManager.InputActions.Gameplay.Deselect.performed -= Deselect_performed;

        Guild.OnNewCycle -= CountDownClock;

    }
    private void Start()
    {
        MasterSingleton.Instance.InputManager.InputActions.Gameplay.Select.performed += Select_performed;
        MasterSingleton.Instance.InputManager.InputActions.Gameplay.Deselect.performed += Deselect_performed;

        _cmbrain = Camera.main.GetComponent<CinemachineBrain>();
        _graphicsRaycasterWorldCanvas = _worldCanvas.GetComponent<GraphicRaycaster>();
        RegisterWithUIHandler();
        SetActive(_active);
    }
    private void Update()
    {
        _mouseIsOverUI = IsMouseOverUI();

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
        if (!_mouseIsOverUI && MasterSingleton.Instance.StateManager.CurrentState != GameplayStateManager.GameplayState.NarrativeEvent)
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

    private void Deselect_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (MasterSingleton.Instance.StateManager.CurrentState != GameplayStateManager.GameplayState.NarrativeEvent)
        {
            DeSelect();
        }

    }

    public void Select()
    {
        if (_active)
        {
            _vcam.Priority = 100;
            DisplaySelectUI(true);

            IsSelected = true;
            Debug.Log("Selected " + this.name);
            AudioManager.instance.PlayOneShot(FMODEvents.instance._uiClick);
            AudioManager.instance.PlayOneShot(FMODEvents.instance._cameraIn);


            switch (this.name)
            {

                case "Old Gods POI":
                    //Debug.Log("play temple layer");
                    AudioManager.instance.SetGlobalParameter("_Location", 3.0f);
                    break;

                //_Location value of 4.0f for more action temple layer

                case "Oasis POI":
                    //Debug.Log("play oasis layer");
                    AudioManager.instance.SetGlobalParameter("_Location", 5.0f);

                    break;

                case "Creditorium POI":
                    //Debug.Log("play creditorium layer");
                    AudioManager.instance.SetGlobalParameter("_Location", 2.0f);

                    break;

                case "Roots POI":
                    //Debug.Log("play roots layer");
                    AudioManager.instance.SetGlobalParameter("_Location", 6.0f);

                    break;

                case "Endstation POI":
                    //Debug.Log("play endstation layer");
                    AudioManager.instance.SetGlobalParameter("_Location", 1.0f);

                    break;
            }

        }

    }

    public void DeSelect()
    {
        _vcam.Priority = 1;
        DisplaySelectUI(false);
        IsSelected = false;
        Debug.Log("Deselected " + this.name);
        AudioManager.instance.PlayOneShot(FMODEvents.instance._cameraOut);
        AudioManager.instance.SetGlobalParameter("_Location", 0.0f);
        
        foreach (Action action in _actions)
        {
            action.ActionUI.RemoveExplorerItems();
        }
        MasterSingleton.Instance.Guild.ClearSelectedExplorers();

    }
    private bool IsMouseOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }

    void GraphicRaycast()
    {
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

    void LoadDiceSprites()
    {
        if (_diceSprites != null)
        {
            _diceSprites.Clear();
        }
        foreach (Sprite sprite in Resources.LoadAll<Sprite>("UI/Dice"))
        {
            _diceSprites.Add(sprite);
            _animSprites.Add(sprite);
        }
    }

    void DisplayWorldUI(bool value)
    {

        _worldClockImage.sprite = _actions[0].ActionUI._clockSprites[_actions[0].Clocks[_actions[0].ActiveClock].Fill];
        _worldClockBackground.sprite = _actions[0].ActionUI._clockBackgroundSprite;
        _worldClockFrame.sprite = _actions[0].ActionUI._clockFrameSprite;

        RecolorClock(_actions[0]);

        _worldCanvas.gameObject.SetActive(value);
    }


    void DisplayClock(int fill, Action action)
    {

        action.ActionUI._activeClockImage.sprite = action.ActionUI._clockSprites[fill];
        action.ActionUI._activeClockFrame.sprite = action.ActionUI._clockFrameSprite;
        action.ActionUI._activeClockBackground.sprite = action.ActionUI._clockBackgroundSprite;
        _worldClockImage.sprite = _actions[0].ActionUI._clockSprites[fill];
        _worldClockBackground.sprite = _actions[0].ActionUI._clockBackgroundSprite;
        _worldClockFrame.sprite = _actions[0].ActionUI._clockFrameSprite;
        action.ActionUI._description.text = action.Clocks[action.ActiveClock].Description;
        RecolorClock(action);

        if (action.Clocks[action.ActiveClock].Segments != 0)
        {
            action.ActionUI._consequences.text = "Attribute: " + action.Clocks[action.ActiveClock].ActionAttribute.ToString() + "\n" + ActionToStringDescription(action);
        }
        else
        {
            action.ActionUI._consequences.text = "";
        }
    }

    void RecolorClock()
    {
        if (0 == _clocks[_activeClock].Segments)
        {
            _worldClockImage.color = _filledColor;
        }
        else if (_clocks[_activeClock].IsCountdown)
        {
            _worldClockImage.color = _countdownColor;
        }
        else
        {
            _worldClockImage.color = _baseColor;
        }
    }
    void RecolorClock(Action action)
    {

        if (0 == action.Clocks[action.ActiveClock].Segments)
        {
            _worldClockImage.color = _filledColor;
            action.ActionUI._activeClockImage.color = _filledColor;
        }
        else if (action.Clocks[action.ActiveClock].IsCountdown)
        {
            _worldClockImage.color = _countdownColor;
            action.ActionUI._activeClockImage.color = _countdownColor;
        }
        else
        {
            _worldClockImage.color = _baseColor;
            action.ActionUI._activeClockImage.color = _baseColor;
        }
    }

    public void SetActive(bool value)
    {
        _active = value;
        _actionsCanvas.transform.parent.gameObject.SetActive(value);
        _worldCanvas.transform.parent.gameObject.SetActive(value);

        if (value)
        {
            RegisterWithUIHandler();
        }
    }

    string ActionToStringDescription(Action action)
    {
        string description = "";

        float avgCred = (action.Fail.Cred + action.Partial.Cred + action.Success.Cred) / 3f;
        if (Mathf.Abs(avgCred) > 0)
        {
            description += "Cred" + DetermineChangeSymbol(avgCred);
        }
        if (Mathf.Abs(avgCred) > 1)
        {
            description += DetermineChangeSymbol(avgCred);
        }
        if (Mathf.Abs(avgCred) > 2)
        {
            description += DetermineChangeSymbol(avgCred);
        }
        description += " ";

        float avgStress = (action.Fail.Stress + action.Partial.Stress + action.Success.Stress) / 3f;
        if (Mathf.Abs(avgStress) > 0)
        {
            description += "Stress" + DetermineChangeSymbol(avgStress);
        }
        if (Mathf.Abs(avgStress) > 1)
        {
            description += DetermineChangeSymbol(avgStress);
        }
        if (Mathf.Abs(avgStress) > 2)
        {
            description += DetermineChangeSymbol(avgStress);
        }
        description += " ";


        float avgHp = (action.Fail.Hp + action.Partial.Hp + action.Success.Hp) / 3f;
        if (Mathf.Abs(avgHp) > 0)
        {
            description += "Vigor" + DetermineChangeSymbol(avgHp);
        }
        if (Mathf.Abs(avgHp) > 1)
        {
            description += DetermineChangeSymbol(avgHp);
        }
        if (Mathf.Abs(avgHp) > 2)
        {
            description += DetermineChangeSymbol(avgHp);
        }

        return description;

    }

    string DetermineChangeSymbol(float avgChangeValue)
    {
        string changeSymbol = "";
        if (avgChangeValue < 0)
        {
            changeSymbol = "↓";
        }
        else if (avgChangeValue > 0)
        {
            changeSymbol = "↑";
        }
        return changeSymbol;
    }

    public void DisplaySelectUI(bool value)
    {
        if (_actionsCanvas != null)
        {
            foreach (Action action in _actions)
            {
                if (action.Enabled)
                {
                    action.ActionUI.DisplayActionCanvas(value, action);
                    action.ActionUI.DisplayExplorerSlots(action.ExplorerSlots);
                }
                else
                {
                    action.ActionUI.DisplayActionCanvas(false, action);
                    action.ActionUI.DisplayExplorerSlots(action.ExplorerSlots);
                }

            }
        }
    }

    void NextClock(Action action)
    {
        action.ActiveClock++;
        action.ActionUI.LoadClockSprites(action.Clocks[action.ActiveClock].Segments);
        action.Clocks[action.ActiveClock].Fill = 0;
        DisplayInteractButton(action);
        DisplayClock(action.Clocks[action.ActiveClock].Fill, action);
    }
    void DisplayInteractButton(Action action)
    {
        if (action.Clocks[action.ActiveClock].IsCountdown)
        {
            action.ActionUI._interact.gameObject.SetActive(false);
        }
        else
        {
            action.ActionUI._interact.gameObject.SetActive(true);
        }
    }

    public void UseAction(Action action)
    {
        Explorer[] selectedExplorers = MasterSingleton.Instance.Guild.SelectedExplorers.ToArray();

        if (selectedExplorers.Length < 1)
        {
            Debug.LogWarning("No explorer selected.");
            //MasterSingleton.Instance.Guild.SelectAvailableExplorer();
            return;
        }

        bool noExplorerExhausted = true;
        foreach (Explorer explorer in selectedExplorers)
        {
            if (explorer.Exhausted)
            {
                noExplorerExhausted = false;
                return;
            }   
        }

        if (action.Clocks[action.ActiveClock].Segments == action.Clocks[action.ActiveClock].Fill && noExplorerExhausted)
        {
            action.Clocks[action.ActiveClock].CompletionCheck();
            LoadNewClockCheck(action);
            DeselectDueToExhaustionCheck();
            MasterSingleton.Instance.Guild.ClearSelectedExplorers();
            action.ActionUI.RemoveExplorerItems();
            
            MasterSingleton.Instance.Guild.ContinueCycle(1);

        }
        else if (noExplorerExhausted && !action.Clocks[action.ActiveClock].IsCountdown && !_rollingDice)
        {

            int diceResult = 0;
            foreach (Explorer explorer in selectedExplorers)
            {
                
                int newDiceResult = explorer.RollDice(action.Clocks[action.ActiveClock].ActionAttribute);
                Debug.Log(explorer.Name + " rolled a " + newDiceResult);
                if (newDiceResult > diceResult)
                {
                    diceResult = newDiceResult;
                }
            }
            Debug.Log("The final roll is a " + diceResult);
            StartDiceRoll(diceResult, action);
            MasterSingleton.Instance.Guild.ContinueCycle(1);

        }
        else if (action.Clocks[action.ActiveClock].IsCountdown)
        {
            Debug.LogWarning("This is a Countdown Clock.");
        }
        else if (_rollingDice)
        {
            Debug.LogWarning("Already rolling Dice.");
        }
        else
        {
            Resources.Load<NarrativeEvent>("Narrative Events/NE_Exhausted").Trigger();
        }


        action.ActionUI.RemoveExplorerItems();
        //MasterSingleton.Instance.Guild.SelectedExplorers.Clear();
        
    }

    void StartDiceRoll(int result, Action action)
    {
        _rollingDice = true;
        StartCoroutine(DiceRoll(result, action));
    }
    IEnumerator DiceRoll(int result, Action action)
    {
        action.ActionUI._animSprites.Shuffle();

        AudioManager.instance.PlayOneShot(FMODEvents.instance._dice);

        foreach (Sprite sprite in action.ActionUI._animSprites)
        {
            action.ActionUI._diceImage.sprite = sprite;
            yield return new WaitForSeconds(.1f);
        }
        yield return new WaitForSeconds(.1f);
        action.ActionUI._diceImage.sprite = action.ActionUI._diceSprites[result - 1];


        ApplyRoll(result, action);
        _applyRollFeedback.PlayFeedbacks();

        //ExhaustSelectedExplorer();
        yield return new WaitForSeconds(.25f * 3 + .1f);
        bool clockComplete = action.Clocks[action.ActiveClock].CompletionCheck();
        LoadNewClockCheck(action);

        if (!clockComplete)
        {
            DeselectDueToExhaustionCheck();
            MasterSingleton.Instance.Guild.ClearSelectedExplorers();
            action.ActionUI.RemoveExplorerItems();
        }

        _rollingDice = false;

        //Tutorial PopUp
        if (!MasterSingleton.Instance.Guild.DiceNEHasTriggerd)
        {
            MasterSingleton.Instance.Guild.DiceNE.Trigger();
            MasterSingleton.Instance.Guild.DiceNEHasTriggerd = true;
        }
    }

    void ApplyRoll(int diceResult, Action action)
    {
        int oldFill = action.Clocks[action.ActiveClock].Fill;

        if (diceResult <= 3)
        {
            action.Clocks[action.ActiveClock].ChangeFill(1);
            StartCoroutine(AnimateClock(action.ActionUI._activeClockImage, oldFill, action.Clocks[action.ActiveClock].Fill, action));
            action.ActionUI._activeClockFrame.sprite = action.ActionUI._clockFrameSprite;
            action.Fail.Apply();
        }
        else if (diceResult <= 5)
        {
            action.Clocks[action.ActiveClock].ChangeFill(2);
            StartCoroutine(AnimateClock(action.ActionUI._activeClockImage, oldFill, action.Clocks[action.ActiveClock].Fill, action));
            action.ActionUI._activeClockFrame.sprite = action.ActionUI._clockFrameSprite;
            action.Partial.Apply();

        }
        else if (diceResult == 6)
        {
            action.Clocks[action.ActiveClock].ChangeFill(3);
            StartCoroutine(AnimateClock(action.ActionUI._activeClockImage, oldFill, action.Clocks[action.ActiveClock].Fill, action));
            action.ActionUI._activeClockFrame.sprite = action.ActionUI._clockFrameSprite;
            action.Success.Apply();
        }
    }

    private IEnumerator AnimateClock(Image clock, int oldFill, int newFill, Action action)
    {
        for (int i = oldFill; i < newFill + 1; i++)
        {
            clock.sprite = action.ActionUI._clockSprites[i];
            yield return new WaitForSeconds(0.25f);
        }

        yield return null;
    }



    void CountDownClock()
    {
        foreach (Action action in _actions)
        {
            if (action.Clocks[action.ActiveClock].IsCountdown && _active)
            {
                action.Clocks[action.ActiveClock].ChangeFill(1);
                action.Clocks[action.ActiveClock].CompletionCheck();
                action.ActionUI._activeClockImage.sprite = action.ActionUI._clockSprites[action.Clocks[action.ActiveClock].Fill];

                if (action.Clocks[action.ActiveClock].Fill >= action.Clocks[action.ActiveClock].Segments)
                {
                    NextClock(action);
                }
            }
        }
    }

    void ExhaustSelectedExplorers()
    {
        foreach (Explorer explorer in MasterSingleton.Instance.Guild.SelectedExplorers)
        {
            explorer.Exhaust();
        }
    }

    void DeselectDueToExhaustionCheck()
    {
        if (MasterSingleton.Instance.Guild.IsRosterExhausted() && MasterSingleton.Instance.StateManager.CurrentState == GameplayStateManager.GameplayState.FreePlay)
        {
            StartCoroutine(DeselectAfter(1f));
        }
    }

    IEnumerator DeselectAfter(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        DeSelect();
    }


    void LoadNewClockCheck(Action action)
    {
        if (action.Clocks[action.ActiveClock].Fill >= action.Clocks[action.ActiveClock].Segments)
        {
            NextClock(action);
        }
    }
    public void OverideAction(Action newAction, int actionIndex)
    {
        _actions[actionIndex] = newAction;
    }

    public void EnableAction(int actionIndex, bool value)
    {
        _actions[actionIndex].Enabled = value;
    }

    void AddActionUIs()
    {
        if (_actionsCanvas != null)
        {
            if (_actions.Length == 0)
            {
                Debug.LogError(name + ": No Actions.");
                return;
            }
            if (_actionsCanvas.transform.childCount < 2)
            {
                foreach (Action action in _actions)
                {
                    GameObject actionCanvas = Instantiate(Resources.Load<GameObject>("Action Canvas"), _actionsCanvas.transform);
                    action.ActionUI = actionCanvas.GetComponent<ActionUI>();
                    action.ActionUI.LoadClockSprites(action.Clocks[action.ActiveClock].Segments);
                    action.ActionUI.LoadDiceSprites();
                    action.ActionUI._interact.onClick.AddListener(() => UseAction(action));
                    action.ActionUI._dice.onClick.AddListener(() => UseAction(action));
                    action.ActionUI._return.onClick.AddListener(DeSelect);
                    DisplayClock(action.Clocks[action.ActiveClock].Fill, action);
                }
            }

        }

    }

    public void SetGameDetailsActive(params int[] indices)
    {
        foreach (GameObject detail in _modelDetails)
        {
            detail.SetActive(false);
        }

        foreach (int index in indices)
        {
            if (index >= 0 && index < _modelDetails.Count)
            {
                _modelDetails[index].SetActive(true);
            }
            else
            {
                Debug.LogWarning("Index out of range: " + index);
            }
        }
    }



}




