using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class ActionUI : MonoBehaviour
{
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

    public List<ExplorerItem> ExplorerItems { get => _explorerItems; set => _explorerItems = value; }

    private void OnEnable()
    {
        MasterSingleton.Instance.InputManager.InputActions.Gameplay.Deselect.performed += DeselectExplorerItem;
    }
    private void OnDisable()
    {
        MasterSingleton.Instance.InputManager.InputActions.Gameplay.Deselect.performed -= DeselectExplorerItem;

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

    public void DisplayActionCanvas(bool value)
    {
        this.gameObject.SetActive(value);
    }

    public bool AddExplorerItem(ExplorerItem explorerItem)
    {
        Debug.Log("Add Explorer Item");

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
                    Destroy(item.gameObject);
                    return;
                }
            }
        }
    }
}
