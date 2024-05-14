using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class ActionUI : MonoBehaviour
{
    PointOfInterest.Action _action;

    [Header("UI References")]
    //TextMeshProUGUI _activeExplorerText;
    [SerializeField]
    public TextMeshProUGUI _description;
    [SerializeField]
    public TextMeshProUGUI _consequences;
    [SerializeField]
    public Image _diceImage;
    [SerializeField]
    public List<Sprite> _diceSprites;
    [SerializeField]
    public List<Sprite> _animSprites;
    [SerializeField]
    public Button _interact, _dice, _return;

    [Header("Clock UI References")]
    [SerializeField]
    public List<Sprite> _clockSprites;
    [SerializeField]
    public Image _activeClockImage, _activeClockFrame, _activeClockBackground;
    [SerializeField]

    public Sprite _clockFrameSprite, _clockBackgroundSprite;
    [SerializeField]
    public Color _baseColor, _filledColor, _countdownColor;
    
    [SerializeField]
    List<ExplorerItem> _explorerItems;
    [SerializeField]
    List<GameObject> _explorerFrames;

    public List<ExplorerItem> ExplorerItems { get => _explorerItems; set => _explorerItems = value; }
    public List<GameObject> ExplorerFrames { get => _explorerFrames; set => _explorerFrames = value; }
    public PointOfInterest.Action Action { get => _action; set => _action = value; }

    private void OnEnable()
    {
        MasterSingleton.Instance.InputManager.InputActions.Gameplay.Select.performed += DeselectExplorerItem;
    }
    private void OnDisable()
    {
        MasterSingleton.Instance.InputManager.InputActions.Gameplay.Select.performed -= DeselectExplorerItem;

    }

    public void LoadClockSprites(int segments)
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

    public void LoadDiceSprites()
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

    public void DisplayActionCanvas(bool value, PointOfInterest.Action action)
    {
        this.gameObject.SetActive(value);
        _action = action;
    }

    public void DisplayExplorerSlots(int slots)
    {
        for (int i = 0; i < ExplorerFrames.Count; i++)
        {
            if (i < slots)
            {
                ExplorerFrames[i].SetActive(true);
            }
            else
            {
                ExplorerFrames[i].SetActive(false);
            }
        }
    }

    public bool AddExplorerItem(ExplorerItem explorerItem, PointOfInterest.Action action)
    {
        Debug.Log("Add Explorer Item");

        if (action.ExplorerSlots <= _explorerItems.Count)
        {
            return false;
        }
        foreach (ExplorerItem item in _explorerItems)
        {
            Debug.Log(item.Explorer.Name);
            Debug.Log(explorerItem.Explorer.Name);
            if (item.Explorer == explorerItem.Explorer)
            {
               

                Debug.Log("Explorer already assigned");
                return false;
            }
        }
        _explorerItems.Add(explorerItem);
        return true;
    }

    public void DeselectExplorerItem(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_explorerItems.Count > 0)
        {
            foreach (ExplorerItem item in _explorerItems)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(item.gameObject.GetComponent<RectTransform>(), MasterSingleton.Instance.InputManager.InputActions.Gameplay.Mouse.ReadValue<Vector2>()))
                {
                    _explorerItems.Remove(item);
                    item.Explorer.SelectExplorer(false);
                    Destroy(item.gameObject);
                    return;
                }
            }
        }
    }

    public void RemoveExplorerItems()
    {
        foreach (ExplorerItem item in _explorerItems)
        {
                Destroy(item.gameObject);    
        }
        _explorerItems.Clear();
    }
}
