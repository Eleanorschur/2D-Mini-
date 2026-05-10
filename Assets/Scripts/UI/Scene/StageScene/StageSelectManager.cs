using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StageSelectManager : MonoBehaviour
{
    [Header("카드경로트랙")]
    public GameObject stageCardPrefab;
    public RectTransform cardTrack;
    public float cardWidth = 1000;

    [Header("방향")]
    public Button btnLeft;
    public Button btnRight;

    [Header("백버튼")]
    public Button btnBack;

    [SerializeField] float WaitTime = 0.0f;

    private StageData[] stageDatas;
    private StageCard[] cards;
    private int currentIndex = 0;
    private bool isMoving = false;

    void Start()
    {
        InitStageData();
        BuildCards();
        UpdateArrows();

        btnLeft.onClick.AddListener(() => Move(-1));
        btnRight.onClick.AddListener(() => Move(1));
        btnBack.onClick.AddListener(OnClickBack);
    }

    void InitStageData()
    {
        stageDatas = new StageData[]
        {
            new StageData { stageIndex = 1, stageName = "Stage 1", starCount = 0, isLocked = false },
            new StageData { stageIndex = 2, stageName = "Stage 2", starCount = 0, isLocked = true },
            new StageData { stageIndex = 3, stageName = "Stage 3", starCount = 0, isLocked = true },
        };

        for (int i = 0; i < stageDatas.Length; i++)
        {
            stageDatas[i].starCount = PlayerPrefs.GetInt($"Stage{i+1}_Stars", 0);
            stageDatas[i].isLocked = PlayerPrefs.GetInt($"Stage{i+1}_Locked", i == 0 ? 0 : 1) == 1;
        }
    }

    void BuildCards()
    {
        cards = new StageCard[stageDatas.Length];

        for (int i = 0; i < stageDatas.Length; i++)
        {
            GameObject obj = Instantiate(stageCardPrefab, cardTrack);
            StageCard card = obj.GetComponent<StageCard>();
            card.Init(stageDatas[i]);

            obj.AddComponent<CanvasGroup>();

            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(i * cardWidth, 0);

            int idx = i;
            card.selectButton.onClick.AddListener(() => OnSelectStage(idx));

            cards[i] = card;
        }

        cardTrack.anchoredPosition = Vector2.zero;
    }

    void Move(int dir)
    {
        if (isMoving) return;

        int next = currentIndex + dir;
        if (next < 0 || next >= stageDatas.Length) return;

        currentIndex = next;
        float targetX = -currentIndex * cardWidth;
        StartCoroutine(MoveTrack(targetX));
        UpdateArrows();
        UpdateCardVisuals();
    }

    IEnumerator MoveTrack(float targetX)
    {
        isMoving = true;

        float startX = cardTrack.anchoredPosition.x;
        float elapsed = 0f;
        float duration = 0.3f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = t * t * (3f - 2f * t);
            cardTrack.anchoredPosition = new Vector2(Mathf.Lerp(startX, targetX, t), 0);
            yield return null;
        }

        cardTrack.anchoredPosition = new Vector2(targetX, 0);
        isMoving = false;
    }

    void UpdateArrows()
    {
        btnLeft.interactable  = currentIndex > 0;
        btnRight.interactable = currentIndex < stageDatas.Length - 1;
    }

    void OnSelectStage(int idx)
    {
        foreach (var card in cards)
        {
            CanvasGroup cg = card.GetComponent<CanvasGroup>();
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }

        cards[idx].PlaySelectAnimation();
        StartCoroutine(FadeOutUI(idx));
        StartCoroutine(LoadAfterAnimation(idx));
    }

    IEnumerator FadeOutUI(int selectedIdx)
    {
        CanvasGroup cgLeft  = btnLeft.GetComponent<CanvasGroup>()  ?? btnLeft.gameObject.AddComponent<CanvasGroup>();
        CanvasGroup cgRight = btnRight.GetComponent<CanvasGroup>() ?? btnRight.gameObject.AddComponent<CanvasGroup>();
        CanvasGroup cgBack  = btnBack.GetComponent<CanvasGroup>()  ?? btnBack.gameObject.AddComponent<CanvasGroup>();

        List<CanvasGroup> sideCards = new List<CanvasGroup>();
        for (int i = 0; i < cards.Length; i++)
        {
            if (i != selectedIdx)
                sideCards.Add(cards[i].GetComponent<CanvasGroup>());
        }

        float duration = 0.3f;
        float elapsed  = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = 1f - (elapsed / duration);

            cgLeft.alpha  = alpha;
            cgRight.alpha = alpha;
            cgBack.alpha  = alpha;

            foreach (var cg in sideCards) cg.alpha = alpha;

            yield return null;
        }

        btnLeft.gameObject.SetActive(false);
        btnRight.gameObject.SetActive(false);
        btnBack.gameObject.SetActive(false);
    }

    void OnClickBack()
    {
        SceneManager.LoadScene("2_Title");
    }

    void UpdateCardVisuals()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            CanvasGroup cg = cards[i].GetComponent<CanvasGroup>();

            if (i == currentIndex)
            {
                cards[i].transform.localScale = Vector3.one;
                cg.alpha = 1f;
            }
            else
            {
                cards[i].transform.localScale = Vector3.one * 0.85f;
                cg.alpha = 0.5f;
            }
        }
    }

    IEnumerator LoadAfterAnimation(int idx)
    {
        Animator anim = cards[idx].GetComponent<Animator>();
        yield return null;
        yield return new WaitUntil(() =>
        {
            AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
            return state.IsName("Select") && state.normalizedTime >= 1f;
        });
        yield return new WaitForSeconds(WaitTime);
        PlayerPrefs.SetInt("SelectedStage", stageDatas[idx].stageIndex);
        SceneManager.LoadScene("5_Stage");
    }
}
