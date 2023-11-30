using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PointOfInterestWorldCanvas : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    Canvas _textCanvas;
    Canvas _masterCanvas;
    RectTransform _masterRectTransform;
    private void Awake()
    {
        _masterCanvas = GetComponent<Canvas>();
        _masterRectTransform = GetComponent<RectTransform>();

        _textCanvas = transform.Find("Text Canvas").GetComponent<Canvas>();

        _masterRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 150);
        _textCanvas.gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        _masterRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 150);
        _textCanvas.gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _masterRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
        _textCanvas.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _masterRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 150);
        _textCanvas.gameObject.SetActive(false);

    }
}


