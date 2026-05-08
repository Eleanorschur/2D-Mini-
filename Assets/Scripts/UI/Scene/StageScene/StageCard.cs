using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StageCard : MonoBehaviour
{
    public Image thumbnailImage;
    public TMP_Text stageNameText;
    public Image[] stars;
    public Sprite filledStar;
    public Sprite emptyStar;
    public GameObject lockOverlay;
    public Button selectButton;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();    
    }

    public void Init(StageData data)
    {
        lockOverlay.SetActive(data.isLocked);

        thumbnailImage.gameObject.SetActive(!data.isLocked);
        stageNameText.gameObject.SetActive(!data.isLocked);
        foreach (var star in stars)
            star.gameObject.SetActive(!data.isLocked);
        selectButton.gameObject.SetActive(!data.isLocked);

        if (!data.isLocked)
        {
            stageNameText.text = data.stageName;

            for (int i = 0; i < stars.Length; i++)
                stars[i].sprite = i < data.starCount ? filledStar : emptyStar;

            Sprite thumb = Resources.Load<Sprite>($"Thumbnails/Stage{data.stageIndex}");
            if (thumb != null)
                thumbnailImage.sprite = thumb;
        }
    }

    public void PlaySelectAnimation()
    {
        animator.SetTrigger("Select");
    }


}