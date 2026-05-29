using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Random = UnityEngine.Random;

public class GenerateLevel : MonoBehaviour
{
    // Serialise the Assets via Unity
    public Sprite TreasureRoom;
    public Sprite BossRoom;
    public Sprite ShopRoom;
    public Sprite UnexploredRoom;
    public Sprite DefaultRoom;
    public Sprite CurrentRoom;

    // icon will specify the type of room
    // location will specify the Cartesian Coordinates of the Map 
    private void DrawIconOnMap(Room room)
    {   
        // create new MapTile (each icon is one MapTile)
        GameObject MapTile = new GameObject("MapTile");
        MapTile.name = room.getRoomNumber().ToString();

        // Add an Image component and assign the Icon
        Image RoomImage = MapTile.AddComponent<Image>();
        RoomImage.sprite = room.getIcon();

        // Draw the Icon on the specified Location
        RectTransform rectTransform = RoomImage.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(Level.Height, Level.Width) * Level.IconScale;
        rectTransform.position = room.getLocation() * ( (Level.IconScale + Level.Padding)  * Level.Height * Level.Scale);

        // Assign the newly created Icon to Canvas Map
        RoomImage.transform.SetParent(transform, false);
    }
    
    private void SetIconOnMap(Room room, Sprite icon)
    {
        room.setIcon(icon);

        String roomNumber = room.RoomNumber.ToString();
        Transform childMapTile = transform.Find(roomNumber);
        Image img = childMapTile.GetComponent<Image>();
        img.sprite = icon;
    }

    private void HandleNestedGeneration(Room room)
    {
        bool isDeadend = true;
        // Continue generation from already generared room?
        if (!Level.CheckIfRoomExists(room.getLocation()))
        {
            DrawIconOnMap(room);
            Level.Rooms.Add(room);
        } 

        List<(Level.Direction, Vector2)> directions = new List<(Level.Direction dir, Vector2 offset)>
        {
            (Level.Direction.Left, Vector2.left),
            (Level.Direction.Right, Vector2.right),
            (Level.Direction.Up, Vector2.up),
            (Level.Direction.Down, Vector2.down),
        };

        for (int i = directions.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (directions[i], directions[j]) = (directions[j], directions[i]);
        }

        // Attempt Generation for each direction
        foreach (var (name, offset) in directions)
        {
            if (Random.value >= Level.RoomGenerationChance)
                continue;

            Vector2 pos = room.getLocation() + offset;

            if (!Level.CheckIfRoomExists(pos) &&
                Level.isEligibleForGeneration(pos, name))
            {
                Room newRoom = new Room(DefaultRoom, pos, room.getDistance() + 1);
                HandleNestedGeneration(newRoom);
                isDeadend = false;
            }
        }

        if (isDeadend)
        {
            room.setDeadend();
        }
    }

    private void GenerateMap()
    {
        // Generate the First Room
        Room StartingRoom = new Room(CurrentRoom, new Vector2(0,0), 0);
        HandleNestedGeneration(StartingRoom);
        // Ensure it hits minimum room count
        while (Level.NumberOfRooms() < Level.MinRoomCount)
        {
            List<Room> deadendRooms = Level.DeadendRooms();
            Room selectedRoom = deadendRooms[Random.Range(0, deadendRooms.Count)];
            HandleNestedGeneration(selectedRoom);
        }

        // Set the furthest distance room into a boss room
        Room furthestRoom = Level.GetFurthestRoom();
        SetIconOnMap(furthestRoom, Level.BossRoomIcon);
    }

    private void Awake()
    {
        // Update Global Variables with assigned assets
        Level.TreasureRoomIcon = TreasureRoom;
        Level.BossRoomIcon = BossRoom;
        Level.ShopRoomIcon = ShopRoom;
        Level.UnexploredRoomIcon = UnexploredRoom;
        Level.DefaultRoomIcon = DefaultRoom;
        Level.CurrentRoomIcon = CurrentRoom;
    }
    private void Start()
    {
        GenerateMap();
    }


}
