using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TraitCanvas : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Trait trait;
    public Image image;
    public TooltipCanvas tooltipCanvasScript;
    [Header("Tooltip Settings")]
    public GameObject tooltipObject;
    public TextMeshProUGUI tooltipText;
    public RectTransform tooltipRectTransform;
    public float hoverDelay = 0.5f;
    public Vector2 offset;
    private Coroutine showTooltipCoroutine;

    private void Start()
    {
        // Ensure the tooltip is hidden at start
        if (tooltipObject != null)
            tooltipObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Start the coroutine to show the tooltip after a delay
        showTooltipCoroutine = StartCoroutine(ShowTooltipDelayed());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Stop the coroutine if it's running and hide the tooltip
        if (showTooltipCoroutine != null)
            StopCoroutine(showTooltipCoroutine);

        HideTooltip();
    }

    private IEnumerator ShowTooltipDelayed()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(hoverDelay);

        // Show the tooltip
        ShowTooltip();
    }

    private void ShowTooltip()
    {
        if (tooltipObject != null && tooltipText != null && trait != null && tooltipRectTransform != null)
        {
            tooltipText.text = trait.description; // Assuming Trait has a description field
            tooltipCanvasScript.ResizeTooltip();

            tooltipObject.SetActive(true);

            // Update tooltip position
            UpdateTooltipPosition();
        }
    }

    private void HideTooltip()
    {
        if (tooltipObject != null)
            tooltipObject.SetActive(false);
    }

    private void UpdateTooltipPosition()
    {
        if (tooltipRectTransform != null)
        {
            // Force the tooltip to update its layout
            LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipRectTransform);

            // Get the current mouse position
            Vector2 mousePosition = Mouse.current.position.ReadValue();

            // Set the position of the tooltip
            tooltipRectTransform.position = mousePosition + offset;

            // Adjust the pivot to the lower left corner
            tooltipRectTransform.pivot = new Vector2(0, 0);

           
        }
    }

    private void Update()
    {
        // Continuously update the tooltip position if it's active
        if (tooltipObject != null && tooltipObject.activeSelf)
        {
            UpdateTooltipPosition();
        }
    }
}