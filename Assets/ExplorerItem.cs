using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


public class ExplorerItem : MonoBehaviour
{
    [SerializeField]
    Image _explorerImage;
    [SerializeField]
    TextMeshProUGUI _explorerNameTextMesh;
    Explorer _explorer;

    public Explorer Explorer { get => _explorer; set => _explorer = value; }

    private void OnEnable()
    {
        MasterSingleton.Instance.InputManager.InputActions.Gameplay.Deselect.performed += DeselectExplorerItem;
    }
    private void OnDisable()
    {
        MasterSingleton.Instance.InputManager.InputActions.Gameplay.Deselect.performed -= DeselectExplorerItem;

    }
    public void LinkExplorer(Explorer explorer)
    {
        _explorer = explorer;
        _explorerNameTextMesh.text = explorer.Name;
    }


    public void DeselectExplorerItem(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        
        if (RectTransformUtility.RectangleContainsScreenPoint(gameObject.GetComponent<RectTransform>(), MasterSingleton.Instance.InputManager.InputActions.Gameplay.Mouse.ReadValue<Vector2>()))
        {
            Destroy(gameObject);
        }
    }
}