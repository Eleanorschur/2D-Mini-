using UnityEngine;

public class ZKey : MonoBehaviour
{
    private Transform targetTransform;

    private float popupAddY = 1.5f;

    // «Æø°º≠ ≤®≥æ ∂ß ≈∏∞Ÿ¿ª ¡ˆ¡§«ÿ¡‹
    public void Setup(Transform target)
    {
        targetTransform = target;
        UpdatePosition();
    }

    void Update()
    {
        if (targetTransform != null)
            UpdatePosition();
    }

    private void UpdatePosition()
    {
        transform.position = targetTransform.position + Vector3.up * popupAddY;
    }

    public void Hide()
    {
        targetTransform = null;
        ZKeyPool.Instance.ReturnZKey(this);
    }
}
