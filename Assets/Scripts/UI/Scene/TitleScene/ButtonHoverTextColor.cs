using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ButtonHoverTextColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TMP_Text buttonText;

    [Header("Text Colors")]
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color hoverColor = Color.black;

    [Header("Scale")]
    [SerializeField] private float hoverScale = 1.05f;

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = hoverColor;

        transform.localScale = originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = normalColor;

        transform.localScale = originalScale;
    }
}