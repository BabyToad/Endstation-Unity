using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TraitCanvas : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Trait trait;
    public Image image;

    [Header("Tooltip Settings")]
    public GameObject tooltipObject;
    public TextMeshProUGUI tooltipText;
    public float hoverDelay = 0.5f;

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
        if (tooltipObject != null && tooltipText != null && trait != null)
        {
            tooltipText.text = trait.description; // Assuming Trait has a description field
            tooltipObject.SetActive(true);
        }
    }

    private void HideTooltip()
    {
        if (tooltipObject != null)
            tooltipObject.SetActive(false);
    }
}