using UnityEngine;
using System.Collections.Generic;

public class ZKeyPool : MonoBehaviour
{
    // 싱글톤(Singleton) 인스턴스입니다.
    // ZKeyPool.Instance라는 한 줄로 어디서든 이 풀 매니저에 접근할 수 있게 해줍니다.
    // private set은 외부에서 이 값을 함부로 바꿀 수 없게 보호하는 역할을 합니다.
    public static ZKeyPool Instance { get; private set; }

    // 복사할 원본 프리팹을 담습니다.
    [SerializeField] private GameObject zKeyPrefab;
    // poolSize는 게임 시작 시 미리 만들어둘 ZKey의 개수입니다.
    [SerializeField] private int poolSize = 3;

    // 생성된 오브젝트들을 담아두는 '창고'입니다.
    // Queue(큐)는 '선입선출(먼저 들어온 게 먼저 나가는)' 방식의 자료구조입니다.
    // 풀링에서는 순서가 중요하기보다는 효율적인 넣고 빼기(Enqueue/Dequeue)를 위해 사용합니다.
    private Queue<ZKey> pool = new Queue<ZKey>();

    // 게임 시작 전 '창고'에 물건을 미리 채워둡니다.
    void Awake()
    {
        Instance = this;
        // for 루프를 통해 설정한 poolSize만큼 ZKey를 미리 생성합니다.
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewZKey();
        }
    }

    // 새로운 ZKey 오브젝트를 하나 만드는 과정입니다.
    private void CreateNewZKey()
    {
        // 프리팹을 복사하여 씬에 생성합니다. transform을 인자로 주어 이 풀 오브젝트의 자식으로 들어오게 합니다.
        GameObject obj = Instantiate(zKeyPrefab, transform);
        ZKey zKey = obj.GetComponent<ZKey>();
        // 당장 사용하지 않으므로 화면에서 꺼둡니다.
        obj.SetActive(false);
        // 생성된 물건을 창고(Queue)에 넣어 저장합니다.
        pool.Enqueue(zKey);
    }

    // 풀에서 ZKey 꺼내서 사용합니다.
    public ZKey GetZKey()
    {
        // 만약 준비된 ZKey가 다 떨어졌다면(Count == 0), 게임이 멈추지 않게 즉석에서 하나 더 만듭니다.
        if (pool.Count == 0) CreateNewZKey();
        // Dequeue()를 통해 창고에서 하나를 꺼낸 뒤,
        ZKey zKey = pool.Dequeue();
        // 화면에 보이게(SetActive(true)) 만들어 반환합니다.
        zKey.gameObject.SetActive(true);
        return zKey;
    }

    // 사용이 끝난 ZKey를 다시 창고로 회수합니다.
    public void ReturnZKey(ZKey zKey)
    {
        // SetActive(false)를 통해 화면에서 숨깁니다.
        zKey.gameObject.SetActive(false);
        // Enqueue(zKey)를 통해 창고 리스트에 다시 추가하여, 나중에 다른 곳에서 재사용할 수 있게 합니다.
        pool.Enqueue(zKey);
    }
}
