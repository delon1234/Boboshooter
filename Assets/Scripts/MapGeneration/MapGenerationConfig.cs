using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Config Class responsible solely for containing read-only config values
public class MapGenerationConfig
{
    public readonly float Height;
    public readonly float Width;
    public readonly float Scale;
    public readonly float IconScale;
    public readonly float Padding;
    public readonly float RoomGenerationChance;
    public readonly int MaxRoomAway;
    public readonly int MinRoomCount;
    public readonly int MaxRoomCount;

    public RoomIconEntry[] RoomIcons;
    public readonly Sprite UnexploredRoomIcon;
    public readonly Sprite ExploredRoomIcon;
    public readonly Sprite CurrentRoomIcon;
    private Dictionary<RoomType, Sprite> RoomIconsDict;

    public MapGenerationConfig(
        float height,
        float width,
        float scale,
        float iconScale,
        float padding,
        float roomGenerationChance,
        int maxRoomAway,
        int minRoomCount,
        int maxRoomCount,
        RoomIconEntry[] roomIcons,
        Sprite unexploredRoomIcon,
        Sprite exploredRoomIcon,
        Sprite currentRoomIcon)
    {
        Height = height;
        Width = width;
        Scale = scale;
        IconScale = iconScale;
        Padding = padding;
        RoomGenerationChance = roomGenerationChance;
        MaxRoomAway = maxRoomAway;
        MinRoomCount = minRoomCount;
        MaxRoomCount = maxRoomCount;

        RoomIcons = roomIcons;
        UnexploredRoomIcon = unexploredRoomIcon;
        ExploredRoomIcon = exploredRoomIcon;
        CurrentRoomIcon = currentRoomIcon;

        // Key, Elem converted to a nice Dict
        RoomIconsDict = RoomIcons.ToDictionary(entry => entry.Type, entry => entry.Icon);
    }

    public Sprite GetRoomIcon(RoomType type)
    {
        return RoomIconsDict[type];
    }
}