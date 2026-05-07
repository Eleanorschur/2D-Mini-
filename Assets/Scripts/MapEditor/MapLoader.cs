using UnityEngine;
using System.Collections.Generic;

public class MapLoader : MonoBehaviour
{
    [Header("프리팹 Resources 경로")]
    [SerializeField] private string tilePrefabFolderPath = "Prefabs";

    [Header("현재 Stage 이름")]
    [SerializeField] private string stageName = "Stage1";

    [Header("타일 크기")]
    [SerializeField] private float tileSize = 1f;

    [Header("맵 시작 위치")]
    [SerializeField] private Vector2 mapOrigin = Vector2.zero;

    [Header("맵 부모")]
    [SerializeField] private Transform mapRoot;

    [Header("자동 로드")]
    [SerializeField] private bool loadOnStart = true;

    [Header("기존 맵 삭제")]
    [SerializeField] private bool clearBeforeLoad = true;

    [Header("플레이어 코드")]
    [SerializeField] private string playerCode = "1";

    [Tooltip("맵 최소 가로 칸 수")]
    [SerializeField] private int minimumVisibleColumns = 30;

    [Tooltip("맵 최소 세로 칸 수")]
    [SerializeField] private int minimumVisibleRows = 18;

    [SerializeField] private float cameraPadding = 0.2f;

    private Transform playerTransform;
    private int currentColumnCount;
    private int currentRowCount;

    private readonly Dictionary<string, string> codeToPrefabName
    = new Dictionary<string, string>()
{
    { "1", "Player" },
    { "2", "Entrance" },
    { "3", "Exit" },
    { "4", "Lever" },
    { "5", "Red_liquid_Medicine" },
    { "6", "Blue_liquid_Medicine" },
    { "7", "Wall" },
    { "8", "Companion" },
    { "9", "CheckPoint" },

    { "10", "Stool_Grey_Center" },
    { "11", "Stool_Grey_Right" },
    { "12", "Stool_Grey_Left" },
    { "13", "Stool_Grey_One" },

    { "20", "Stool_Red_Center" },
    { "21", "Stool_Red_Right" },
    { "22", "Stool_Red_Left" },
    { "23", "Stool_Red_One" },

    { "30", "Stool_Blue_Center" },
    { "31", "Stool_Blue_Right" },
    { "32", "Stool_Blue_Left" },
    { "33", "Stool_Blue_One" },

    { "41", "EntranceRight" },
    { "42", "ExitLeft" }
};


    private void Start()
    {
        if (loadOnStart)
            LoadStage(stageName);
    }

    public void LoadStage(string targetStageName)
    {
        stageName = targetStageName;
        playerTransform = null;

        TextAsset jsonFile = Resources.Load<TextAsset>($"Maps/{stageName}");

        if (jsonFile == null)
        {
            Debug.LogError($"JSON 파일을 찾을 수 없습니다: Resources/Maps/{stageName}.json");
            return;
        }

        StageButtonData stageData = JsonUtility.FromJson<StageButtonData>(jsonFile.text);

        if (stageData == null || stageData.rows == null)
        {
            Debug.LogError($"스테이지 데이터가 잘못되었습니다: {stageName}");
            return;
        }

        if (mapRoot == null)
        {
            GameObject root = new GameObject("MapRoot");
            mapRoot = root.transform;
        }

        mapRoot.position = new Vector3(mapOrigin.x, mapOrigin.y, 0f);

        if (clearBeforeLoad)
            ClearMap();

        ApplyStageSize(stageData);
        BuildMap(stageData);

        Debug.Log($"맵 로드 완료: {stageName}");
    }

    private void ApplyStageSize(StageButtonData stageData)
    {
        int jsonColumnCount = stageData.mapColumnCount;
        int usedColumnCount = GetUsedColumnCount(stageData);
        int usedRowCount = GetUsedRowCount(stageData);

        currentColumnCount = Mathf.Max(jsonColumnCount, usedColumnCount, minimumVisibleColumns);
        currentRowCount = Mathf.Max(usedRowCount, minimumVisibleRows);

        Debug.Log($"스테이지 크기 적용 완료: Columns={currentColumnCount}, Rows={currentRowCount}");
    }

    private int GetUsedColumnCount(StageButtonData stageData)
    {
        int maxColumn = 0;

        for (int row = 0; row < stageData.rows.Count; row++)
        {
            MapRow mapRow = stageData.rows[row];

            if (mapRow == null || mapRow.cells == null)
                continue;

            for (int col = 0; col < mapRow.cells.Count; col++)
            {
                string cellCode = mapRow.cells[col];

                if (string.IsNullOrWhiteSpace(cellCode))
                    continue;

                cellCode = cellCode.Trim();

                if (cellCode == "0")
                    continue;

                maxColumn = Mathf.Max(maxColumn, col + 1);
            }
        }

        return maxColumn;
    }

    private int GetUsedRowCount(StageButtonData stageData)
    {
        int maxRow = 0;

        for (int row = 0; row < stageData.rows.Count; row++)
        {
            MapRow mapRow = stageData.rows[row];

            if (mapRow == null || mapRow.cells == null)
                continue;

            for (int col = 0; col < mapRow.cells.Count; col++)
            {
                string cellCode = mapRow.cells[col];

                if (string.IsNullOrWhiteSpace(cellCode))
                    continue;

                cellCode = cellCode.Trim();

                if (cellCode == "0")
                    continue;

                maxRow = Mathf.Max(maxRow, row + 1);
            }
        }

        return maxRow;
    }

    private void BuildMap(StageButtonData stageData)
    {
        int rowLimit = Mathf.Min(stageData.rows.Count, currentRowCount);

        for (int row = 0; row < rowLimit; row++)
        {
            MapRow mapRow = stageData.rows[row];

            if (mapRow == null || mapRow.cells == null)
                continue;

            int colLimit = Mathf.Min(mapRow.cells.Count, currentColumnCount);

            for (int col = 0; col < colLimit; col++)
            {
                string cellCode = mapRow.cells[col];

                if (string.IsNullOrWhiteSpace(cellCode))
                    continue;

                cellCode = cellCode.Trim();

                if (cellCode == "0")
                    continue;

                string[] codes = cellCode.Split('+');

                foreach (string rawCode in codes)
                {
                    string code = rawCode.Trim();

                    if (string.IsNullOrWhiteSpace(code) || code == "0")
                        continue;

                    CreateObject(code, col, row);
                }
            }
        }
    }

    private void CreateObject(string code, int col, int row)
    {
        if (!codeToPrefabName.TryGetValue(code, out string prefabName))
        {
            Debug.LogWarning($"등록되지 않은 코드입니다: {code}");
            return;
        }

        string prefabPath = $"{tilePrefabFolderPath}/{prefabName}";

        GameObject prefab = Resources.Load<GameObject>(prefabPath);

        if (prefab == null)
        {
            Debug.LogWarning($"프리팹을 찾을 수 없습니다: Resources/{prefabPath}");
            return;
        }

        GameObject obj = Instantiate(prefab, mapRoot);

        obj.transform.localPosition = GetLocalPosition(col, row);
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;

        if (code == playerCode)
        {
            playerTransform = obj.transform;
            obj.name = "Player";
        }
    }

    private Vector3 GetLocalPosition(int col, int row)
    {
        float x = col * tileSize;
        float y = -row * tileSize;

        return new Vector3(x, y, 0f);
    }

    public void ClearMap()
    {
        if (mapRoot == null)
            return;

        for (int i = mapRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(mapRoot.GetChild(i).gameObject);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            LoadStage("Stage1");

        if (Input.GetKeyDown(KeyCode.Alpha2))
            LoadStage("Stage2");

        if (Input.GetKeyDown(KeyCode.Alpha3))
            LoadStage("Stage3");
    }
}