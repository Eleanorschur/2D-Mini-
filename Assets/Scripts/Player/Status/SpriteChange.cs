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

    }

    void Start()
    {
        bodyRtf = transform.Find(bodyR);
        bodyGtf = transform.Find(bodyG);
        bodyBtf = transform.Find(bodyB);

        ChangeForm(0);
    }

    public void ChangeForm(int status)
    {
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
