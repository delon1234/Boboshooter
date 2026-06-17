using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapRenderer
{
    private Transform parent;
    private MapGenerationConfig config;
    private MapGenerationState state;
    private Dictionary<int, Image> tilesByRoomNumber = new Dictionary<int, Image>();

    public MinimapRenderer(Transform parent, MapGenerationConfig config, MapGenerationState state)
    {
        this.parent = parent;
        this.config = config;
        this.state = state;
    }

    public void FreshRender(IEnumerable<Room> rooms)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            Object.Destroy(parent.GetChild(i).gameObject);
        }
        tilesByRoomNumber.Clear();

        foreach (Room room in rooms)
        {
            DrawIconOnMap(room);
        }
    }

    private void DrawIconOnMap(Room room)
    {
        GameObject mapTile = new GameObject("MapTile");
        mapTile.name = room.RoomNumber.ToString();

        Image roomImage = mapTile.AddComponent<Image>();
        roomImage.sprite = room.Icon;

        RectTransform rectTransform = roomImage.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(config.Height, config.Width) * config.IconScale;
        rectTransform.position = room.Location * ((config.IconScale + config.Padding) * config.Height * config.Scale);

        roomImage.transform.SetParent(parent, false);
        tilesByRoomNumber[room.RoomNumber] = roomImage;
    }

    public void UpdateRoomState(Room room, Room CurrentRoom, MapGenerationConfig config)
    {
        Image img;
        if (!tilesByRoomNumber.TryGetValue(room.RoomNumber, out img))
        {
            return;
        }
        
        // Set current room to current room icon
        if (room == CurrentRoom)
        {
            img.sprite = config.CurrentRoomIcon;
            return;
        }
        // Special rooms (boos, treasure, shop) will retain their icon
        if (!room.IsNormal)
        {
            img.sprite = room.Icon;
            return;
        }
        // Unvisited room will have unexplored room icon
        if (!room.IsVisited)
        {
            img.sprite = config.UnexploredRoomIcon;
            return;
        }
        // Visited room will have default room icon
        img.sprite = config.ExploredRoomIcon;
    }

    public void SubscribeOnRoomChanged(RoomManager RoomManager)
    {
        RoomManager.OnRoomChanged += OnRoomChanged;
    }

    private void OnRoomChanged(OnRoomChangedArgs args)
    {
        // Update CurrentRoom
        UpdateRoomState(args.EnteredRoom, args.EnteredRoom, config);
        
        // Update all Neighbours of CurrentRoom
        foreach (var neighbor in args.EnteredRoom.Neighbors.Values)
        {
            UpdateRoomState(neighbor, args.EnteredRoom, config);
        }
    }
}