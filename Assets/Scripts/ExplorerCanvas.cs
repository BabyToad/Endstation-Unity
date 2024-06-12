using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ExplorerCanvas : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // Start is called before the first frame update
    Explorer _explorer;
    [SerializeField] Text _name;
    [SerializeField] Image _image, _draggableImage;
    [SerializeField] Slider _health, _stress, _xp;
    [SerializeField] Text _insight, _prowess, _resolve;
    [SerializeField] TextMeshProUGUI _traits;
    [SerializeField] Button _selectExplorer;
    [SerializeField] List<Button> _advancementButtons;
    [SerializeField] Image _background, _backgroundSelected;
    [SerializeField] Color _normalColor, _selectedColor, _exhaustedColor, _selectedExhaustedColor;

    //Drag Explorer Stuff
    public Image characterImage; // The image representing the character
    [SerializeField] GameObject characterUIElementPrefab; // The prefab for the draggable UI element
 
    private GameObject currentDraggedObject;
    private Canvas parentCanvas;

    public Button SelectExplorer { get => _selectExplorer; set => _selectExplorer = value; }
    public List<Button> AdvancementButtons { get => _advancementButtons; set => _advancementButtons = value; }

    public float animationDuration = 0.5f;
    private float currentValue;


    private void Awake()
    {
        _normalColor = _background.color;
        _selectedColor = _backgroundSelected.color;
        ShowAdvancementButtons(false);
    }

    private void Start()
    {
        characterUIElementPrefab = Resources.Load<GameObject>("Explorer UI Dragable");
        parentCanvas = transform.parent.gameObject.GetComponent<Canvas>();
    }


    public void ReferenceExplorer(Explorer explorer)
    {
        _explorer = explorer;
    }
    public void SetName(string name)
    {
        _name.text = name;
    }

    public void SetImage(Sprite sprite)
    { 
        _image.sprite = sprite;
        _draggableImage.sprite = sprite;
    }

    public void SetHealth(int health)
    {
        //_health.value = (float)health;
        StartCoroutine(AnimateSlider((float)health, _health));
    }
    public void SetStress(int stress)
    {
        //_stress.value = (float)stress;
        StartCoroutine(AnimateSlider((float)stress, _stress));

    }

    public void SetXP(int xp)
    {
        StartCoroutine(AnimateSlider((float)xp, _xp));
    }


    public void SetInsight(int insight, bool isInjured, bool isTrauma)
    {
        SetStatColor(_insight, isInjured, isTrauma);
        _insight.text = "Insight: " + insight;
    }
    public void SetProwess(int prowess, bool isInjured, bool isTrauma)
    {
        SetStatColor(_prowess, isInjured, isTrauma);

        _prowess.text = "Prowess: " + prowess;
    }
    public void SetResolve(int resolve, bool isInjured, bool isTrauma)
    {
        SetStatColor(_resolve, isInjured, isTrauma);

        _resolve.text = "Resolve: " + resolve;
    }
    public void SetTraits(Trait[] traits)
    {
        _traits.text = "";
        foreach (Trait t in traits)
        {
            _traits.text += t.name + " ";
            if (t is Relationship relationship)
            {
                _traits.text += relationship._strength + " ";
            }
        }
    }

    public void ShowAdvancementButtons(bool value)
    {
        for (int i = 0; i < AdvancementButtons.Count; i++)
        {
            AdvancementButtons[i].gameObject.SetActive(value);
        }
    }

    void SetStatColor(Text stat, bool isInjured, bool isTrauma)
    {
        if (isInjured && isTrauma)
        {
            stat.color = Color.red;
        }
        else if (isInjured || isTrauma)
        {
            stat.color = Color.yellow;
        }
        else if (!isInjured && !isTrauma)
        {
            stat.color = Color.white;
        }
    }
    public void DisplayExhaustion(bool value)
    {
        if (value)
        {
            _background.color = _exhaustedColor;
            _backgroundSelected.color = _selectedExhaustedColor;
        }
        else
        {
            _background.color = _normalColor;
            _backgroundSelected.color = _selectedColor;

        }
    }

    private IEnumerator AnimateSlider(float targetValue, Slider slider)
    {
        float startTime = Time.time;
        float startValue = slider.value;

        // Briefly show afterimage of old value
        yield return new WaitForSeconds(0.1f);

        while (Time.time - startTime < animationDuration)
        {
            float elapsedTime = Time.time - startTime;
            float newValue = Mathf.Lerp(startValue, targetValue, elapsedTime / animationDuration);
            slider.value = newValue;
            yield return null;
        }

        // Ensure final value is set correctly
        slider.value = targetValue;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        currentDraggedObject = Instantiate(characterUIElementPrefab, parentCanvas.transform);
        ExplorerItem explorerItem = currentDraggedObject.GetComponent<ExplorerItem>();
        explorerItem.LinkExplorer(_explorer);
        explorerItem.SetImage(_image.sprite);
        currentDraggedObject.AddComponent<LayoutElement>().ignoreLayout = true;
        //currentDraggedObject.GetComponent<Image>().sprite = characterImage.sprite;



        // Makes the dragged object follow the pointer
        currentDraggedObject.transform.SetAsLastSibling(); // Ensures it's on top of UI
        BlockRaycasts(true); // Temporarily prevent clicks through the dragged object
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentDraggedObject != null)
        {
            currentDraggedObject.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (currentDraggedObject != null)
        {
            Debug.Log(eventData.pointerCurrentRaycast.gameObject);
            if (eventData.pointerCurrentRaycast.gameObject?.name == "Explorer Group Canvas")
            {
                // Successfully dropped on an Expedition Canvas

                GameObject characterUIObject = Instantiate(characterUIElementPrefab, eventData.pointerCurrentRaycast.gameObject.transform);
                characterUIObject.GetComponent<ExplorerItem>().SetImage(_image.sprite);

                ExplorerItem expItem = characterUIObject.GetComponent<ExplorerItem>();

                expItem.LinkExplorer(_explorer);
                ActionUI actionUI = eventData.pointerCurrentRaycast.gameObject.transform.parent.transform.parent.transform.parent.transform.parent.GetComponent<ActionUI>();
                
                ;
                bool wasAdded = actionUI.AddExplorerItem(expItem, actionUI.Action);

                if (wasAdded)
                {
                    
                    _explorer.SelectExplorer(true);
                }
                else
                {
                    Destroy(characterUIObject);

                }
            }
            Destroy(currentDraggedObject);
        }
        BlockRaycasts(false); // Re-enable clicks
    }

    private void BlockRaycasts(bool block)
    {
        // Use a graphic raycaster on the canvas
        GraphicRaycaster raycaster = parentCanvas.GetComponent<GraphicRaycaster>();
        if (raycaster != null)
        {
            raycaster.enabled = !block;
        }
    }
}
