using UnityEngine;
using TMPro;

public class TextBackgroundResizer : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public RectTransform backgroundRectTransform;

    [SerializeField] private float paddingLeft = 10f;
    [SerializeField] private float paddingRight = 10f;
    [SerializeField] private float paddingTop = 5f;
    [SerializeField] private float paddingBottom = 5f;

    private void Start()
    {
        if (textMeshPro == null || backgroundRectTransform == null)
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
        backgroundRectTransform.sizeDelta = new Vector2(newWidth, newHeight);

        // Position the text within the background
        textMeshPro.rectTransform.anchoredPosition = new Vector2(paddingLeft, -paddingTop);

        // Ensure the text size is updated
        textMeshPro.rectTransform.sizeDelta = new Vector2(textSize.x, textSize.y);
    }

    // Call this method whenever you change the text
    public void SetText(string newText)
    {
        textMeshPro.text = newText;
        ResizeTooltip();
    }
}