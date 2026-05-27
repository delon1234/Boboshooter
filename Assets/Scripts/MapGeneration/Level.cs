using System.Collections.Generic;
using UnityEngine;

// stores level information globally for the other scripts to use
public static class Level
{
    // default map size
    public static float Height = 500;
    public static float Width = 500;

    // map, icon, padding scale to default
    public static float Scale = 1f;
    public static float IconScale = 0.1f;
    public static float Padding = 0.01f;

    // chance for each adjacent room to spawn
    public static float RoomGenerationChance = 0.5f;
    
    public static Sprite TreasureRoomIcon;
    public static Sprite BossRoomIcon;
    public static Sprite ShopRoomIcon;
    public static Sprite UnexploredRoomIcon;
    public static Sprite DefaultRoomIcon;
    public static Sprite CurrentRoomIcon;

    public static List<Room> Rooms = new List<Room>();
    public static Room CurrentRoom;

    public static bool CheckIfRoomExists(Vector2 pos)
    {
        return (Rooms.Exists(x => x.getLocation() == pos));
    }
}

// each room will have their data
public class Room
{
    public int RoomNumber = 0;
    private Vector2 location;
    private Sprite roomImage;

    public Room(Sprite s, Vector2 l)
    {
        roomImage = s;
        location = l;   
    }

    public Vector2 getLocation()
    {
        return location;
    }

    public Sprite getIcon()
    {
        return roomImage;    
    }
    public Vector2 getLeftLocation()
    {
        return location + new Vector2(-1,0);
    }

    public Vector2 getRightLocation()
    {
        return location + new Vector2(1,0);
    }

    public Vector2 getUpLocation()
    {
        return location + new Vector2(0,1);
    }

    public Vector2 getDownLocation()
    {
        return location + new Vector2(0,-1);
    }
}