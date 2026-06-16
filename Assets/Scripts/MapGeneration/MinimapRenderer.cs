using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapRenderer
{
    private Transform parent;
    private MapGenerationConfig config;
    private Dictionary<int, Image> tilesByRoomNumber = new Dictionary<int, Image>();

    public MinimapRenderer(Transform parent, MapGenerationConfig config)
    {
        this.parent = parent;
        this.config = config;
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

    public void UpdateRoomIcon(Room room)
    {
        if (tilesByRoomNumber.TryGetValue(room.RoomNumber, out Image image))
        {
            image.sprite = room.Icon;
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

        if (room == CurrentRoom)
        {
            img.sprite = config.CurrentRoomIcon;
            return;
        }

        else if (!room.IsNormal)
        {
            img.sprite = room.Icon;
            return;
        }

        else if (!room.IsVisited)
        {
            img.sprite = config.UnexploredRoomIcon;
            return;
        }

        else
        {
            img.sprite = config.DefaultRoomIcon;
        }
    }
}