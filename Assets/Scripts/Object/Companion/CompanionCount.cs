using System;
using UnityEngine;
using UnityEngine.UI;

public class CompanionCount : MonoBehaviour
{
    private Image image;
    [SerializeField] private Sprite companionOn;
    [SerializeField] private Sprite companionOff;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Start()
    {
        
    }

    public void ImageChange(bool value)
    {
        image.sprite = value ? companionOn : companionOff;
    }
}
