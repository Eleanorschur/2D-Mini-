using UnityEngine;

public class SpriteChange : MonoBehaviour
{
    private Transform bodyRtf;
    private Transform bodyGtf;
    private Transform bodyBtf;

    private string bodyR = "BodyR";
    private string bodyG = "BodyG";
    private string bodyB = "BodyB";

    void Awake()
    {
        bodyRtf = transform.Find(bodyR);
        bodyGtf = transform.Find(bodyG);
        bodyBtf = transform.Find(bodyB);
    }

    void Start()
    {
        ChangeForm(0);
    }

    public void ChangeForm(int status)
    {
        if (bodyRtf == null || bodyGtf == null || bodyBtf == null)
        {
            Debug.LogError("BodyR, BodyG, BodyB 중 하나를 찾지 못했습니다. Player 프리팹 자식 이름을 확인하세요.");
            return;
        }

        switch (status)
        {
            case 0:
                bodyRtf.gameObject.SetActive(false);
                bodyGtf.gameObject.SetActive(true);
                bodyBtf.gameObject.SetActive(false);
                break;

            case 1:
                bodyRtf.gameObject.SetActive(true);
                bodyGtf.gameObject.SetActive(false);
                bodyBtf.gameObject.SetActive(false);
                break;

            case 2:
                bodyRtf.gameObject.SetActive(false);
                bodyGtf.gameObject.SetActive(false);
                bodyBtf.gameObject.SetActive(true);
                break;
        }
    }
}