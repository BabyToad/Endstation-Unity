using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipCanvas : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public RectTransform tooltipCanvasRectTransform;

    [SerializeField] private float paddingLeft = 10f;
    [SerializeField] private float paddingRight = 10f;
    [SerializeField] private float paddingTop = 5f;
    [SerializeField] private float paddingBottom = 5f;

    private void Start()
    {
        if (textMeshPro == null || tooltipCanvasRectTransform == null)
        {
            Debug.LogError("Please assign TextMeshPro and background RectTransform in the inspector.");
            return;
        }

        ResizeTooltip();
    }

    public void ResizeTooltip()
    {
        // Force text to update its layout
        textMeshPro.ForceMeshUpdate();

        // Get the text bounds
        Vector2 textSize = textMeshPro.GetRenderedValues(false);

        // Calculate the new width and height
        float newWidth = textSize.x + paddingLeft + paddingRight;
        float newHeight = textSize.y + paddingTop + paddingBottom;

        // Set the background size
        tooltipCanvasRectTransform.sizeDelta = new Vector2(newWidth, newHeight);
        //textMeshPro.rectTransform.anchoredPosition = new Vector3(newWidth/2, 0);


    }

    // Call this method whenever you change the text
    public void SetText(string newText)
    {
        textMeshPro.text = newText;
        ResizeTooltip();
    }
}
