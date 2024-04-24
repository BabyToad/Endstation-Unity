using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class NE_Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltip;
    public Vector3 offset;
    public float displayDelay = 0.25f;
    public float movementThreshold = 5f; // Adjust this to control sensitivity

    Vector2 previousMousePosition;

    bool pointerIsIn =false;
    private Coroutine showCoroutine;


    private void OnEnable()
    {
        tooltip.SetActive(false);
        pointerIsIn = false;
    }
    private void Update()
    {
        Vector2 mousePos = MasterSingleton.Instance.InputManager.InputActions.Gameplay.Mouse.ReadValue<Vector2>();

        // Check for mouse movement
        if (pointerIsIn)
        {
            showCoroutine = StartCoroutine(ShowTooltip());
        }

        if (Vector2.Distance(mousePos, previousMousePosition) > movementThreshold)
        {
            // Mouse has moved significantly - hide the tooltip
            tooltip.SetActive(false);

            // Also stop the show coroutine if it's running
            if (showCoroutine != null)
            {
                StopCoroutine(showCoroutine);
                showCoroutine = null;
            }

            previousMousePosition = mousePos;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerIsIn = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (showCoroutine != null)
        {
            StopCoroutine(showCoroutine);
            showCoroutine = null;
        }
        tooltip.SetActive(false);
        pointerIsIn = false;

    }

    IEnumerator ShowTooltip()
    {
        yield return new WaitForSeconds(displayDelay);
       tooltip.transform.position = new Vector3(previousMousePosition.x, previousMousePosition.y, 0) + offset;
       tooltip.SetActive(true);
       
    }
}
