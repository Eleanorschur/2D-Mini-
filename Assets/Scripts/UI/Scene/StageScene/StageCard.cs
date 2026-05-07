using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageCard : MonoBehaviour
{
    public Image thumbnailImage;
    public TMP_Text stageNameText;
    public Image[] stars;           // 별 Image 3개
    public Sprite filledStar;
    public Sprite emptyStar;
    public GameObject lockOverlay;  // 잠금 오버레이
    public Button selectButton;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();    
    }

    public void Init(StageData data)
    {
        stageNameText.text = data.stageName;
        lockOverlay.SetActive(data.isLocked);
        selectButton.interactable = !data.isLocked;

        for (int i = 0; i < stars.Length; i++)
            stars[i].sprite = i < data.starCount ? filledStar : emptyStar;

        Sprite thumb = Resources.Load<Sprite>($"Thumbnails/Stage{data.stageIndex}");
        if (thumb != null)
            thumbnailImage.sprite = thumb;
    }

    public void PlaySelectAnimation()
    {
        animator.SetTrigger("Select");
    }


}