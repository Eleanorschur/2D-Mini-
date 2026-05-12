using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ImageSlider : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [Header("UI")]
    [SerializeField] private Image fillImage;
    [SerializeField] private RectTransform handle;

    [Header("Value")]
    [Range(0f, 1f)]
    [SerializeField] private float value = 1f;

    [Header("Event")]
    public UnityEvent<float> onValueChanged;

    private RectTransform trackArea;

    private void Awake()
    {
        trackArea = GetComponent<RectTransform>();
        ApplyValue();
    }

    private void Start()
    {
        ApplyValue();
        onValueChanged.Invoke(value);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateValue(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateValue(eventData);
    }

    private void UpdateValue(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            trackArea,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        );

        value = Mathf.InverseLerp(  trackArea.rect.xMin,
                                    trackArea.rect.xMax,
                                    localPoint.x);
        
        value = Mathf.Clamp01(value);

        ApplyValue();
        onValueChanged.Invoke(value);
    }

    private void ApplyValue()
    {
        if (fillImage == null || handle == null || trackArea == null)
            return;

        fillImage.fillAmount = value;

        float x = Mathf.Lerp(trackArea.rect.xMin, trackArea.rect.xMax, value);
        handle.anchoredPosition = new Vector2(x, 0f);
    }

    public void SetValue(float newValue)
    {
        value = Mathf.Clamp01(newValue);
        ApplyValue();
        onValueChanged.Invoke(value);
    }

    public float GetValue()
    {
        return value;
    }
}