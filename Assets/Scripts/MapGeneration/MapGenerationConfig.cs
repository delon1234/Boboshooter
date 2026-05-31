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

    public readonly Sprite TreasureRoomIcon;
    public readonly Sprite BossRoomIcon;
    public readonly Sprite ShopRoomIcon;
    public readonly Sprite UnexploredRoomIcon;
    public readonly Sprite DefaultRoomIcon;
    public readonly Sprite CurrentRoomIcon;

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
        Sprite treasureRoomIcon,
        Sprite bossRoomIcon,
        Sprite shopRoomIcon,
        Sprite unexploredRoomIcon,
        Sprite defaultRoomIcon,
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

        TreasureRoomIcon = treasureRoomIcon;
        BossRoomIcon = bossRoomIcon;
        ShopRoomIcon = shopRoomIcon;
        UnexploredRoomIcon = unexploredRoomIcon;
        DefaultRoomIcon = defaultRoomIcon;
        CurrentRoomIcon = currentRoomIcon;
    }
}