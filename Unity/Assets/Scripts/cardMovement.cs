using UnityEngine;
using UnityEngine.EventSystems;

public class cardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;
    private Vector3 originalScale;
    private int currentState;
    private Quaternion originalRotation;
    private Vector3 originalPosition;

    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private Vector2 cardPlay;
    [SerializeField] private Vector3 playPos;
    [SerializeField] private GameObject glowEffect;
    [SerializeField] private GameObject playArrow;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        originalScale = rectTransform.localScale;
        originalRotation = rectTransform.localRotation;
        originalPosition = rectTransform.localPosition;
    }

    void Update()
    {
        switch (currentState)
        {
            case 0:
                HandleHoverState();
                break; 
            case 1:
                HandleDragState();
                if (!Input.GetMouseButton(0))
                {
                    transitionToState0();
                }
                break;
            case 2:
                HandlePlayedState();
                if (!Input.GetMouseButton(0))
                {
                    transitionToState0();
                }
                break;
        }
    }

    private void transitionToState0()
    {
        currentState = 0;
        rectTransform.localScale = originalScale;
        rectTransform.localRotation = originalRotation;
        rectTransform.localPosition = originalPosition; 
        glowEffect.SetActive(false);
        playArrow.SetActive(false);
    }   
    
    private void HandleHoverState()
    {
        glowEffect.SetActive(true); 
        rectTransform.localScale = originalScale * selectScale;
    }

       private void HandleDragState()
    {
        rectTransform.localRotation = Quaternion.identity;
    }

    private void HandlePlayedState()
    {
        rectTransform.localPosition = playPos;
        rectTransform.localRotation = Quaternion.identity;

        if (Input.mousePosition.y < cardPlay.y)
        {
            currentState = 2;
            playArrow.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == 0)
        {
            originalPosition = rectTransform.localPosition;
            originalRotation = rectTransform.localRotation;
            originalScale = rectTransform.localScale;

            currentState = 1;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentState == 1)
        {
            transitionToState0();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentState == 1)
        {
            currentState = 2; 
            RectTransformUtility.ScreenPointToLocalPointInRectangle((canvas.GetComponent<RectTransform>()), eventData.position, eventData.pressEventCamera, out Vector2 localPoint);
            originalPanelLocalPosition = rectTransform.localPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentState == 2)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle((canvas.GetComponent<RectTransform>()), eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
            {
                localPoint /= canvas.scaleFactor;
                Vector3 offset = localPoint - originalLocalPointerPosition;
                rectTransform.localPosition = originalPanelLocalPosition + offset;
                
                if (rectTransform.localPosition.y > cardPlay.y)
                {
                    currentState = 3;
                    playArrow.SetActive(true);
                    rectTransform.localPosition = playPos;
                }
            }
        }
    }

}