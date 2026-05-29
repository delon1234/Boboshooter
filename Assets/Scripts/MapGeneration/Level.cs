using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using Random = UnityEngine.Random;

// stores level information globally for the other scripts to use
public static class Level
{
    // default map size
    public static float Height = 500;
    public static float Width = 500;

    // map, icon, padding scale to default
    public static float Scale = 1f;
    public static float IconScale = 0.05f;
    public static float Padding = 0.005f;

    // chance for each adjacent room to spawn
    public static float RoomGenerationChance = 0.15f;
    public static int MaxRoomAway = 8;
    public static int MinRoomCount = 20;
    public static int MaxRoomCount = 30;

    
    public static Sprite TreasureRoomIcon;
    public static Sprite BossRoomIcon;
    public static Sprite ShopRoomIcon;
    public static Sprite UnexploredRoomIcon;
    public static Sprite DefaultRoomIcon;
    public static Sprite CurrentRoomIcon;

    public static List<Room> Rooms = new List<Room>();
    public static Room CurrentRoom;

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public static bool CheckIfRoomExists(Vector2 pos)
    {
        return (Rooms.Exists(x => x.getLocation() == pos));
    }

    public static int NumberOfRooms()
    {
        return Rooms.Count;    
    }

    public static List<Room> DeadendRooms()
    {
        return Rooms.Where(r => r.isDeadend()).ToList();
    }

    public static Room GetFurthestRoom()
    {
        if (Rooms.Count == 0)
            return null;

        int maxDist = Rooms.Max(r => r.getDistance());

        List<Room> furthestRooms = Rooms
            .Where(r => r.getDistance() == maxDist)
            .ToList();

        return furthestRooms[Random.Range(0, furthestRooms.Count)];
    }

    // logic to determine what makes a candidate room generatable
    public static bool isEligibleForGeneration(Vector2 pos, Direction direction)
    {
        // max of 10 away from spawn
        if (Math.Abs(pos.x) > MaxRoomAway || Math.Abs(pos.y) > MaxRoomAway) return false;
        // Max Room Count, stop generating
        if (NumberOfRooms() > MaxRoomCount) return false;

        switch(direction)
        {
            case Direction.Left:
                {
                    if (
                    CheckIfRoomExists(pos + new Vector2(-1,0)) ||
                    CheckIfRoomExists(pos + new Vector2(0,1)) ||
                    CheckIfRoomExists(pos + new Vector2(0,-1))
                    )
                    {
                        return false;
                    }
                    break;
                }
            case Direction.Right:
                {
                    if (
                    CheckIfRoomExists(pos + new Vector2(1,0)) ||
                    CheckIfRoomExists(pos + new Vector2(0,1)) ||
                    CheckIfRoomExists(pos + new Vector2(0,-1))
                    )
                    {
                        return false;
                    }
                    break;
                }
            case Direction.Up:
                {
                    if (
                    CheckIfRoomExists(pos + new Vector2(-1,0)) ||
                    CheckIfRoomExists(pos + new Vector2(1,0)) ||
                    CheckIfRoomExists(pos + new Vector2(0,1))
                    )
                    {
                        return false;
                    }
                    break;
                }
            case Direction.Down:
                {
                    if (
                    CheckIfRoomExists(pos + new Vector2(-1,0)) ||
                    CheckIfRoomExists(pos + new Vector2(1,0)) ||
                    CheckIfRoomExists(pos + new Vector2(0,-1))
                    )
                    {
                        return false;
                    }
                    break;
                }
        }
        return true;
    }
}

// each room will have their data
public class Room
{
    public static int RoomCount = 0;
    public int RoomNumber = 0;
    private Vector2 location;
    private Sprite roomImage;
    private int distAway;
    private bool deadend = false;

    public Room(Sprite s, Vector2 l, int distance)
    {
        roomImage = s;
        location = l;   
        distAway = distance;

        RoomNumber = RoomCount;
        RoomCount += 1;
    }

    public Vector2 getLocation()
    {
        return location;
    }

    public Sprite getIcon()
    {
        return roomImage;    
    }
    public void setIcon(Sprite icon)
    {
        roomImage = icon;
    }
    public int getRoomNumber()
    {
        return RoomNumber;
    }
    public bool isDeadend()
    {
        return deadend;
    }
    public void setDeadend()
    {
        deadend = true;
    }

    public int getDistance()
    {
        return distAway;
    }

    public void setRoom(Sprite icon)
    {
        roomImage = icon;
    }
}