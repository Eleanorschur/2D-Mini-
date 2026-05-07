using UnityEngine;
using Unity.Cinemachine;

public class CameraBoundsController : MonoBehaviour
{
    [SerializeField] private BoxCollider2D cameraBounds;
    [SerializeField] private CinemachineConfiner2D confiner;

    [Header("카메라 Bounds 수동 보정값")]
    [SerializeField] private Vector2 boundsOffsetAdjust = Vector2.zero;

    public void SetBounds(int columnCount, int rowCount, float tileSize, Vector3 mapOrigin)
    {
        float mapWidth = columnCount * tileSize;
        float mapHeight = rowCount * tileSize;

        transform.position = mapOrigin;

        cameraBounds.offset = new Vector2(
            mapWidth / 2f + boundsOffsetAdjust.x,
            -mapHeight / 2f + boundsOffsetAdjust.y
        );

        cameraBounds.size = new Vector2(
            mapWidth,
            mapHeight
        );

        if (confiner != null)
        {
            confiner.InvalidateBoundingShapeCache();
            confiner.InvalidateLensCache();
        }

        Debug.Log($"Camera Bounds 설정 완료: Width={mapWidth}, Height={mapHeight}, Offset={cameraBounds.offset}");
    }
}