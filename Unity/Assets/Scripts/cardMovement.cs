using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class cardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private enum cardState {Idle, Hover, Drag}
    private cardState currentState = cardState.Idle;

    private RectTransform rectTransform;
    private RectTransform parentRect;
    private Canvas canvas;
    private handManager hand;

    private Vector2 pointerStartPos;
    private Vector3 cardStartPos;
    private Vector3 originalScale;

    public Vector3 targetPos;
    private Quaternion targetRot;

    private bool keepOnTop;

    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private GameObject glowEffect;

    public int layerPriority
    {
        get
        {
            switch (currentState)
            {
                case cardState.Drag:
                    return 3;
                case cardState.Hover:
                    return 2;
                default:
                    return keepOnTop ? 1 : 0;
            }
        }
    }

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRect = rectTransform.parent as RectTransform;
        hand = FindAnyObjectByType<handManager>();
        canvas = GetComponentInParent<Canvas>();

        originalScale = rectTransform.localScale;
        targetPos = rectTransform.localPosition;
        targetRot = rectTransform.localRotation;

        if (glowEffect != null) glowEffect.SetActive(false);
    }

    public void setTarget(Vector3 pos, Quaternion rot)
    {
        targetPos = pos;
        targetRot = rot;
    }

    void Update()
    {
        if (currentState == cardState.Hover && !PointerIsOverCard())
        {
            returnToHand();
        } else if (currentState == cardState.Drag && (Mouse.current == null || !Mouse.current.leftButton.isPressed))
        {
            returnToHand();
        }

        float t = 1f - Mathf.Exp(-moveSpeed * Time.deltaTime);

        Vector3 goalScale = currentState == cardState.Idle ? originalScale : originalScale * selectScale;
        Quaternion goalRot = currentState == cardState.Drag ? Quaternion.identity : targetRot;

        rectTransform.localScale = Vector3.Lerp(rectTransform.localScale, goalScale, t);
        rectTransform.localRotation = Quaternion.Slerp(rectTransform.localRotation, goalRot, t);

        if (currentState != cardState.Drag)
        {
            rectTransform.localPosition = Vector3.Lerp(rectTransform.localPosition, targetPos, t);
            rectTransform.localRotation = Quaternion.Slerp(rectTransform.localRotation, targetRot, t);
        }

        if (keepOnTop && currentState == cardState.Idle)
        {
            bool arrived = (rectTransform.localPosition - targetPos).sqrMagnitude < 1f && (rectTransform.localScale - goalScale).sqrMagnitude < 0.0001f; 
            if (arrived)
            {
                keepOnTop = false;
                if (hand != null) hand.refreshLayers();
            }
        }

    }

    void OnDisable()
    {
        currentState = cardState.Idle;
        keepOnTop = false;
        if (glowEffect != null) glowEffect.SetActive(false);
        if (hand != null && hand.draggedCard == gameObject) hand.draggedCard = null;
    }

    private bool PointerIsOverCard()
    {
        if (Mouse.current == null) return false;

        Camera cam = (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            ? canvas.worldCamera
            : null;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, mousePos, cam);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState != cardState.Idle) return;
        if (hand != null && hand.draggedCard != null) return;

        currentState = cardState.Hover;
        rectTransform.localScale = originalScale * selectScale;
        
        if (glowEffect != null) glowEffect.SetActive(true);
        if (hand != null) hand.refreshLayers();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentState == cardState.Hover)
        {
            returnToHand();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentState != cardState.Hover) return;

        currentState = cardState.Drag; 

        if (hand != null) 
        {
            hand.draggedCard = gameObject;
            hand.refreshLayers();
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, eventData.position, eventData.pressEventCamera, out pointerStartPos);
        cardStartPos = rectTransform.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (currentState != cardState.Drag) return;
        
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRect, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
        {
            Vector2 offset = localPoint - pointerStartPos;
            rectTransform.localPosition = cardStartPos + (Vector3)offset;
            if (hand != null) hand.UpdateDragOrder(gameObject);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (currentState == cardState.Drag)
            returnToHand();
    }

    private void returnToHand()
    {
        currentState = cardState.Idle;
        keepOnTop = true;
        
        rectTransform.localScale = originalScale;
        if (glowEffect != null) glowEffect.SetActive(false);
        
        if (hand != null) {
            if (hand.draggedCard == gameObject) hand.draggedCard = null;
            hand.updateHandLayout();
        }
    }

}