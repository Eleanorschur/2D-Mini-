using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class StageSelectManager : MonoBehaviour 
{

    [Header("카드")]
    public GameObject stageCardPrefab;
    public RectTransform cardTrack;
    public float cardWidth = 1000; 

    [Header("화살표")]
    public Button btnLeft;
    public Button btnRight;

    [Header("뒤로가기")]
    public Button btnBack;


    [SerializeField] float AnimationSpeed = 0.5f;

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

    void InitStageData() {
        // 스테이지 추가할 때 여기만 수정
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

    void BuildCards() {
        cards = new StageCard[stageDatas.Length];

        for (int i = 0; i < stageDatas.Length; i++) 
        {
            GameObject obj = Instantiate(stageCardPrefab, cardTrack);
            StageCard card = obj.GetComponent<StageCard>();
            card.Init(stageDatas[i]);

            RectTransform rt = obj.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(i * cardWidth, 0);

            // 선택 버튼 이벤트
            int idx = i;
            card.selectButton.onClick.AddListener(() => OnSelectStage(idx));

            cards[i] = card;
        }

        // track 초기 위치
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
        btnLeft.interactable = currentIndex > 0;
        btnRight.interactable = currentIndex < stageDatas.Length - 1;
    }

    void OnSelectStage(int idx)
    {
        cards[idx].PlaySelectAnimation();
        StartCoroutine(LoadAfterAnimation(idx));
    }


    void OnClickBack() 
    {
        SceneManager.LoadScene("TitleScene");
    }

    void UpdateCardVisuals()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            RectTransform rt = cards[i].GetComponent<RectTransform>();
            CanvasGroup cg = cards[i].GetComponent<CanvasGroup>();

            // CanvasGroup이 없으면 추가
            if (cg == null) cg = cards[i].gameObject.AddComponent<CanvasGroup>();

            if (i == currentIndex)
            {
                rt.localScale = Vector3.one;
                cg.alpha = 1f;
            }
            else
            {
                rt.localScale = Vector3.one * 0.85f;
                cg.alpha = 0.5f;
            }
        }
    }


    IEnumerator LoadAfterAnimation(int idx)
    {
        yield return new WaitForSeconds(AnimationSpeed);
        PlayerPrefs.SetInt("SelectedStage", stageDatas[idx].stageIndex);
        SceneManager.LoadScene("5_Stage");
    }




    //void Start()
    //{
    //    // StageSelectScene에서 저장한 번호 읽기
    //    int stageIndex = PlayerPrefs.GetInt("SelectedStage", 1);

    //    // 읽은 번호로 JSON 파일 로드
    //    string jsonPath = $"Stage{stageIndex}";
    //    // 이후 기존 JSON 로드 코드 그대로 사용
    //}




}