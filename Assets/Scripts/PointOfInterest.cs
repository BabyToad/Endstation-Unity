using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class PointOfInterest : MonoBehaviour
{

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

    CinemachineBrain _cmbrain;

    bool _selected;

    bool _mouseIsOverUI;

    //Clock
    [SerializeField]
    Image _clockImage, _worldClockImage;
    [SerializeField]
    List<Sprite> _clockSprites;


    //UI Selection
    GraphicRaycaster _graphicsRaycasterWorldCanvas;

    private void OnEnable()
    {
        Guild.OnEndCycle += CountDownClock; 
        

        LoadClockSprites(_clocks[_activeClock].Segments);
        DisplayClock(_clocks[_activeClock].Fill);

        if (_selected)
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
        _vcam.Priority = 100;
        _activeCanvas.gameObject.SetActive(true);
        Debug.Log("Selected " + this.name);

    }

    public void DeSelect()
    {
        _vcam.Priority = 1;
        _activeCanvas.gameObject.SetActive(false);

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

        if (segments == 4)
        {
            foreach (Sprite sprite in Resources.LoadAll<Sprite>("UI/Progress Clocks/4Clock"))
            {
                _clockSprites.Add(sprite);
            }
        }
        if (segments == 6)
        {
            foreach (Sprite sprite in Resources.LoadAll<Sprite>("UI/Progress Clocks/6Clock"))
            {
                _clockSprites.Add(sprite);
            }
        }
        if (segments == 8)
        {
            foreach (Sprite sprite in Resources.LoadAll<Sprite>("UI/Progress Clocks/8Clock"))
            {
                _clockSprites.Add(sprite);
            }
        }
    }

    void DisplayWorldUI(bool value)
    {
        _worldClockImage.sprite = _clockSprites[_clocks[_activeClock].Fill];
        _worldCanvas.gameObject.SetActive(value);
    }

    void DisplayClock(int fill)
    {
        _clockImage.sprite = _clockSprites[fill];
        _description.text = _clocks[_activeClock].Description;
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
        if (!MasterSingleton.Instance.Guild.SelectedExplorer.Exhausted || !_clocks[_activeClock].IsCountdown)
        {
            int diceResult = MasterSingleton.Instance.Guild.SelectedExplorer.RollDice(_clocks[_activeClock].ActionAttribute);

            if (diceResult <= 3)
            {
                _clocks[_activeClock].ChangeFill(1);
                _clockImage.sprite = _clockSprites[_clocks[_activeClock].Fill];
                _mainAction.Fail.Apply();
            }
            else if (diceResult <= 5)
            {
                _clocks[_activeClock].ChangeFill(2);
                _clockImage.sprite = _clockSprites[_clocks[_activeClock].Fill];
                _mainAction.Partial.Apply();

            }
            else if (diceResult == 6)
            {
                _clocks[_activeClock].ChangeFill(3);
                _clockImage.sprite = _clockSprites[_clocks[_activeClock].Fill];
                _mainAction.Success.Apply();

            }

            if (_clocks[_activeClock].Fill >= _clocks[_activeClock].Segments)
            {
                NextClock();
            }

            Debug.Log(MasterSingleton.Instance.Guild.SelectedExplorer.Name + " used the Action at " + this.name);

            MasterSingleton.Instance.Guild.SelectedExplorer.Exhaust();
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
        if (_clocks[_activeClock].IsCountdown)
        {
            _clocks[_activeClock].ChangeFill(1);
            _clockImage.sprite = _clockSprites[_clocks[_activeClock].Fill];
            
            if (_clocks[_activeClock].Fill >= _clocks[_activeClock].Segments)
            {
                NextClock();
            }

        }

    }
}


    

