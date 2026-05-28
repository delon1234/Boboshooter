using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public static float RoomGenerationChance = 0.05f;
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

    // logic to determine what makes a candidate room generatable
    public static bool isEligibleForGeneration(Vector2 pos, string direction)
    {
        // max of 10 away from spawn
        if (Math.Abs(pos.x) > MaxRoomAway || Math.Abs(pos.y) > MaxRoomAway) return false;
        // Max Room Count, stop generating
        if (NumberOfRooms() > MaxRoomCount) return false;

        switch(direction)
        {
            case "Left":
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
            case "Right":
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
            case "Up":
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
            case "Down":
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
    public int RoomNumber = 0;
    private Vector2 location;
    private Sprite roomImage;
    private bool deadend = false;

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

    public bool isDeadend()
    {
        return deadend;
    }
    public void setDeadend()
    {
        deadend = true;
    }
}