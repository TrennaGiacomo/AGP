using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MinimapManager : MonoBehaviour
{
    [Header("Keybinds")]
    [SerializeField] private KeyCode FullMinimapToggle = KeyCode.Tab;

    [Header("Room & Player Icons")]
    [SerializeField] private GameObject roomIconPrefab;
    [SerializeField] private GameObject playerIconPrefab;

    [Header("UI References")]
    [SerializeField] private RectTransform minimapContent;
    [SerializeField] private RectTransform fullMapContent;
    [SerializeField] private GameObject minimapUI;
    [SerializeField] private GameObject fullMapUI;

    [Header("Layout")]
    [SerializeField] private Vector2 roomSpacing = new(24, 24);
    [SerializeField] private int visibleRadius = 3;

    private Dictionary<Vector2Int, GameObject> minimapRoomIcons = new();
    private Dictionary<Vector2Int, GameObject> fullmapRoomIcons = new();
    private HashSet<Vector2Int> visitedRooms = new();

    private RectTransform playerIcon;
    private RectTransform activeContent;
    private bool isMinimapActive = true;

    private void Awake()
    {
        minimapUI.SetActive(true);
    }
    public void Init(Dictionary<Vector2Int, Room> placedRooms)
    {
        foreach (var kvp in placedRooms)
        {
            Vector2Int gridPos = kvp.Key;

            GameObject miniIcon = Instantiate(roomIconPrefab, minimapContent);
            miniIcon.GetComponent<RectTransform>().anchoredPosition = GridToPos(gridPos);
            minimapRoomIcons[gridPos] = miniIcon;

            GameObject fullIcon = Instantiate(roomIconPrefab, fullMapContent);
            fullIcon.GetComponent<RectTransform>().anchoredPosition = GridToPos(gridPos);
            fullmapRoomIcons[gridPos] = fullIcon;
        }

        playerIcon = Instantiate(playerIconPrefab, minimapContent).GetComponent<RectTransform>();
        activeContent = minimapContent;

        minimapUI.SetActive(true);
        fullMapUI.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(FullMinimapToggle))
        {
            ToggleMapView();
        }
    }

    public void UpdatePlayerPosition(Vector2Int playerGridPos)
    {
        Vector2 offset = -GridToPos(playerGridPos);
        activeContent.anchoredPosition = offset;

        if (playerIcon != null)
        {
            playerIcon.anchoredPosition = GridToPos(playerGridPos);
        }

        if (isMinimapActive)
        {
            foreach (var kvp in minimapRoomIcons)
            {
                Vector2Int roomPos = kvp.Key;
                bool visible = Mathf.Abs(roomPos.x - playerGridPos.x) <= visibleRadius &&
                               Mathf.Abs(roomPos.y - playerGridPos.y) <= visibleRadius;

                kvp.Value.SetActive(visible);
            }
        }
    }

    private void ToggleMapView()
    {
        isMinimapActive = !isMinimapActive;

        minimapUI.SetActive(isMinimapActive);
        fullMapUI.SetActive(!isMinimapActive);

        activeContent = isMinimapActive ? minimapContent : fullMapContent;
        playerIcon.SetParent(activeContent, false);
    }

    private Vector2 GridToPos(Vector2Int gridPos)
    {
        return new Vector2(
            gridPos.x * roomSpacing.x,
            gridPos.y * roomSpacing.y
        );
    }

    public void MarkRoomVisited(Vector2Int gridPos)
    {
        if(visitedRooms.Contains(gridPos)) return;
        visitedRooms.Add(gridPos);  

        if(minimapRoomIcons.TryGetValue(gridPos, out var miniIcon))
            if(miniIcon.TryGetComponent<UnityEngine.UI.Image>(out var image)) image.color = Color.yellow;
        

        if(fullmapRoomIcons.TryGetValue(gridPos, out var fullIcon))
            if(fullIcon.TryGetComponent<UnityEngine.UI.Image>(out var image)) image.color = Color.yellow;
    }
}
