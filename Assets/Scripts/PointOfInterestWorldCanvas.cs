using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PointOfInterestWorldCanvas : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    bool _worldSpace;
    Canvas _textCanvas;
    Canvas _masterCanvas;
    RectTransform _masterRectTransform;
    private void Awake()
    {
        _masterCanvas = GetComponent<Canvas>();
        _masterRectTransform = GetComponent<RectTransform>();

        _textCanvas = transform.Find("Text Canvas").GetComponent<Canvas>();

        //_masterRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 150);
        _textCanvas.enabled = false;
    }

    private void Update()
    {
        if (_worldSpace)
        {
            _masterCanvas.transform.LookAt(transform.position - (Camera.main.transform.position - (transform.position)));

        }
    }

    private void OnEnable()
    {
        //_masterRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 150);
        _textCanvas.enabled = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //_masterRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
        _textCanvas.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //_masterRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 150);
        _textCanvas.enabled = false;

    }
}


